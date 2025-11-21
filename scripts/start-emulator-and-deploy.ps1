<#
PowerShell helper script pour démarrer un AVD Android et déployer l'APK MAUI (net10.0-android).
Usage: Ouvrir PowerShell à la racine du dépôt et exécuter:
  Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
  .\scripts\start-emulator-and-deploy.ps1

Le script suppose que l'Android SDK est installé et que ANDROID_SDK_ROOT ou %LOCALAPPDATA%\Android\Sdk existe.
Ce script affiche désormais des diagnostics plus détaillés quand il ne trouve pas les outils.
#>

param(
    [string]$ProjectDir = ".\AgriConnect",
    [string]$Config = "Debug",
    [string]$TFM = "net10.0-android"
)

function Find-AndroidSdkRoot {
    # Priorité: ANDROID_SDK_ROOT, ANDROID_HOME, emplacement par défaut LocalAppData, autres chemins usuels
    $candidates = @()
    if ($env:ANDROID_SDK_ROOT) { $candidates += $env:ANDROID_SDK_ROOT }
    if ($env:ANDROID_HOME) { $candidates += $env:ANDROID_HOME }
    $candidates += Join-Path $env:LOCALAPPDATA "Android\Sdk"
    $candidates += "C:\Android\Sdk"
    $candidates += "C:\Program Files\Android\Android SDK"
    foreach ($p in $candidates) {
        if ($p -and (Test-Path $p)) { return (Get-Item $p).FullName }
    }
    return $null
}

$sdk = Find-AndroidSdkRoot
if (-not $sdk) {
    Write-Error "Android SDK introuvable. Installez l'Android SDK (via Visual Studio Installer ou Android Studio) ou définissez la variable d'environnement ANDROID_SDK_ROOT."
    Write-Host "Vérifiez les chemins suivants (exemples) :"
    Write-Host " - %LOCALAPPDATA%\Android\Sdk"
    Write-Host " - C:\Android\Sdk"
    Write-Host " - C:\Program Files\Android\Android SDK"
    Write-Host "Exemples de commandes pour Windows (session actuelle) :"
    Write-Host "  $env:LOCALAPPDATA\Android\Sdk -> PowerShell: `$env:ANDROID_SDK_ROOT = \"$env:LOCALAPPDATA\Android\Sdk\""
    Write-Host "Ajoutez aussi emulator et platform-tools au PATH :"
    Write-Host "  $env:Path += ';' + (Join-Path $env:LOCALAPPDATA 'Android\Sdk\emulator') + ';' + (Join-Path $env:LOCALAPPDATA 'Android\Sdk\platform-tools')"
    exit 1
}

Write-Host "Android SDK trouvé : $sdk"

# localiser outils
$emulator = Join-Path $sdk "emulator\emulator.exe"
$adb = Join-Path $sdk "platform-tools\adb.exe"
$avdmanager = Join-Path $sdk "cmdline-tools\latest\bin\avdmanager.bat"

# essayer autres emplacements pour cmdline-tools
if (-not (Test-Path $avdmanager)) {
    $possible = Get-ChildItem -Path (Join-Path $sdk "cmdline-tools") -Directory -ErrorAction SilentlyContinue | Select-Object -ExpandProperty FullName -ErrorAction SilentlyContinue
    foreach ($dir in $possible) {
        $candidate = Join-Path $dir "bin\avdmanager.bat"
        if (Test-Path $candidate) { $avdmanager = $candidate; break }
    }
}

$missing = @()
if (-not (Test-Path $emulator)) { $missing += 'emulator' }
if (-not (Test-Path $adb)) { $missing += 'adb (platform-tools)' }
if ($missing.Count -gt 0) {
    Write-Error "Outils manquants : $($missing -join ', ') dans le SDK ($sdk)."
    Write-Host "Vérifiez que les composants Android Emulator et Android SDK Platform-Tools sont installés."
    Write-Host "PATH actuel (début): $([Environment]::GetEnvironmentVariable('PATH') -split ';' | Select-Object -First 10 -Join ';')"
    Write-Host "Si vous utilisez Visual Studio, installez le workload 'Mobile development with .NET' via le Visual Studio Installer."
    exit 1
}

Write-Host "Recherche des AVDs disponibles..."
$avds = & $emulator -list-avds 2>$null | Where-Object { $_ -ne "" }

if (-not $avds -or $avds.Count -eq 0) {
    Write-Host "Aucun AVD trouvé."
    if (Test-Path $avdmanager) {
        Write-Host "Vous pouvez lister/installer des images via avdmanager (exemple) :"
        Write-Host "  `"$avdmanager`" list avd"
        Write-Host "Ou ouvrez Android Studio -> Device Manager -> Create Virtual Device"
    }
    exit 1
}

Write-Host "AVDs trouvés :"
$avds | ForEach-Object { Write-Host " - $_" }

$avdName = Read-Host "Entrez le nom de l'AVD à lancer (Entrée pour prendre le premier)"
if (-not $avdName) { $avdName = $avds[0] }

Write-Host "Démarrage de l'émulateur : $avdName"
Start-Process -FilePath $emulator -ArgumentList "-avd $avdName" -NoNewWindow

# Attendre que l'émulateur soit prêt
Write-Host "Attente que l'émulateur soit prêt (cela peut prendre 30-120s)..."
$maxTries = 60
$try = 0
while ($try -lt $maxTries) {
    $devicesRaw = & $adb devices 2>$null
    $devices = $devicesRaw | Select-String "emulator" | ForEach-Object { $_.ToString().Trim() }
    if ($devices) { break }
    Start-Sleep -Seconds 2
    $try++
}

if ($try -ge $maxTries) {
    Write-Error "Émulateur non détecté après $($maxTries*2) secondes. Abandon." 
    Write-Host "Sortie `adb devices` :"; & $adb devices
    exit 1
}
Write-Host "Émulateur prêt."

# Build / publish APK
$publishDir = Join-Path $ProjectDir "publish"
if (Test-Path $publishDir) { Remove-Item -Recurse -Force $publishDir }
New-Item -ItemType Directory -Path $publishDir | Out-Null

Write-Host "Publication de l'application (génération de l'APK)..."
$publishCmd = "dotnet publish `"$ProjectDir`" -f $TFM -c $Config -p:AndroidPackageFormat=apk -o `"$publishDir`""
Write-Host $publishCmd
$publishResult = & dotnet publish $ProjectDir -f $TFM -c $Config -p:AndroidPackageFormat=apk -o $publishDir
if ($LASTEXITCODE -ne 0) {
    Write-Error "Échec du dotnet publish. Vérifiez la sortie ci-dessus."
    exit 1
}

# Trouver l'APK
$apk = Get-ChildItem -Path $publishDir -Filter *.apk -Recurse | Select-Object -First 1
if (-not $apk) {
    Write-Error "APK introuvable dans $publishDir. La publication n'a peut-être pas généré un .apk."
    exit 1
}

Write-Host "APK trouvé : $($apk.FullName)"

Write-Host "Installation de l'APK sur l'émulateur via adb..."
& $adb install -r $apk.FullName
if ($LASTEXITCODE -ne 0) {
    Write-Error "Échec de l'installation de l'APK via adb. Sortie adb ci-dessous :"; & $adb install -r $apk.FullName
    exit 1
}

Write-Host "Installation terminée. Vous pouvez maintenant lancer l'application depuis l'émulateur."
Write-Host "Si vous préférez, lancez Visual Studio et appuyez sur F5 pour déboguer directement sur l'émulateur."
