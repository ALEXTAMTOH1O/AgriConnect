<#
PowerShell helper script pour démarrer un AVD Android et déployer l'APK MAUI (net10.0-android).
Usage: Ouvrir PowerShell à la racine du dépôt et exécuter:
  Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
  .\scripts\start-emulator-and-deploy.ps1

Le script suppose que l'Android SDK est installé et que ANDROID_SDK_ROOT ou %LOCALAPPDATA%\Android\Sdk existe.
#>

param(
    [string]$ProjectDir = ".\AgriConnect",
    [string]$Config = "Debug",
    [string]$TFM = "net10.0-android"
)

# Résolution du SDK Android
$sdk = $env:ANDROID_SDK_ROOT
if (-not $sdk -or -not (Test-Path $sdk)) {
    $sdk = Join-Path $env:LOCALAPPDATA "Android\Sdk"
}

if (-not (Test-Path $sdk)) {
    Write-Error "Android SDK introuvable. Installez l'Android SDK ou définissez ANDROID_SDK_ROOT."
    exit 1
}

$emulator = Join-Path $sdk "emulator\emulator.exe"
$adb = Join-Path $sdk "platform-tools\adb.exe"
$avdmanager = Join-Path $sdk "cmdline-tools\latest\bin\avdmanager.bat"

if (-not (Test-Path $emulator)) {
    Write-Error "emulator.exe introuvable dans $sdk\emulator. Assurez-vous que le SDK est correctement installé."
    exit 1
}
if (-not (Test-Path $adb)) {
    Write-Error "adb introuvable dans $sdk\platform-tools. Assurez-vous que platform-tools est installé."
    exit 1
}

Write-Host "Recherche des AVDs disponibles..."
$avds = & $emulator -list-avds 2>$null | Where-Object { $_ -ne "" }

if (-not $avds -or $avds.Count -eq 0) {
    Write-Host "Aucun AVD trouvé. Ouvrez Android Studio -> Device Manager pour en créer un, ou installez un AVD via avdmanager."
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
    $devices = & $adb devices | Select-String "emulator" | ForEach-Object { $_.ToString().Trim() }
    if ($devices) { break }
    Start-Sleep -Seconds 2
    $try++
}

if ($try -ge $maxTries) {
    Write-Error "Émulateur non détecté après $($maxTries*2) secondes. Abandon." 
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
    Write-Error "Échec de l'installation de l'APK via adb."
    exit 1
}

Write-Host "Installation terminée. Vous pouvez maintenant lancer l'application depuis l'émulateur."
Write-Host "Si vous préférez, lancez Visual Studio et appuyez sur F5 pour déboguer directement sur l'émulateur."
