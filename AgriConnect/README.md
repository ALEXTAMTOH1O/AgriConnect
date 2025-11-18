# AgriConnect

Application .NET MAUI (net10) multiplateforme — Android, iOS, Mac Catalyst et Windows. Ce dépôt contient l'application mobile/desktop AgriConnect et la configuration de base du projet.

## Description
AgriConnect est une application .NET MAUI ciblant `.NET 10`. Le projet utilise une structure Single Project et des propriétés standards (voir `AgriConnect.csproj`) pour les icônes, splash, images et polices.

## Prérequis
- __Visual Studio 2026__ (avec le support .NET MAUI installé)
- .NET 10 SDK
- Workloads MAUI (si nécessaire) : dotnet workload install maui

- Accès aux SDKs/IDEs pour les plateformes ciblées (Android SDK, Xcode pour iOS/Mac Catalyst, etc.)

## Installation (local)
1. Cloner le dépôt : git clone <url-du-depot> cd AgriConnect
2. Restaurer et construire :dotnet restore dotnet build -f net10.0-android   # exemple pour Android
3. Ouvrir la solution dans __Visual Studio__ : __File > Open > Project/Solution__ puis sélectionner le projet `AgriConnect.csproj`.

## Exécution
- Depuis __Visual Studio__ : choisir la plateforme cible (Android/iOS/Windows/MacCatalyst) et lancer via __Debug > Start Debugging__ ou __Build > Run__.
- Depuis la CLI (exemple Android) : dotnet build -f net10.0-android -c Debug dotnet publish -f net10.0-android -c Release -o ./publish/android


## Versioning et publication sur GitHub — Processus recommandé

1. Politique de version
   - Utiliser Semantic Versioning (MAJOR.MINOR.PATCH), ex. `v1.2.3`.
   - Mettre à jour `ApplicationDisplayVersion` (version lisible) et `ApplicationVersion` (entier) dans `AgriConnect.csproj` avant la création d'une release.

2. Branching model simple
   - `main` : code stable et releases.
   - `develop` : intégration continue des features (optionnel).
   - `feature/<nom>` : développement de nouvelles fonctionnalités.
   - Toutes les merges vers `main` passent par une Pull Request (revue + tests).

3. Étapes pour créer une release (manuelle)
   a. Créer une branche de release ou travailler depuis `main` : git checkout -b release/vX.Y.Z
b. Mettre à jour les versions dans `AgriConnect.csproj` :
- Modifier `<ApplicationDisplayVersion>` à `X.Y.Z`
- Modifier `<ApplicationVersion>` (incrémenter l'entier)
c. Commit et push :git add AgriConnect.csproj git commit -m "chore(release): vX.Y.Z" git push origin release/vX.Y.Z
d. Ouvrir une Pull Request vers `main`, faire la revue et merger.
e. Sur `main`, taguer la release :git checkout main git pull origin main git tag -a vX.Y.Z -m "Release vX.Y.Z" git push origin vX.Y.Z
f. Créer la Release GitHub (interface web ou CLI) : gh release create vX.Y.Z --title "vX.Y.Z" --notes "Notes de release..."