# Production Build & Deploy Guide

This document is the **single source of truth** for creating a production
package and deploying *BlazorCookbookApp* to Azure App Service.

## Table of Contents

- [Production Build \& Deploy Guide](#production-build--deploy-guide)
  - [Table of Contents](#table-of-contents)
  - [1  Prerequisites](#1--prerequisites)
    - [One-off App Service settings](#one-off-app-service-settings)
  - [2  Build \& Package](#2--build--package)
    - [Recipe Manifest Generation](#recipe-manifest-generation)
  - [3  Test locally (optional)](#3--test-locally-optional)
  - [4  Deploy](#4--deploy)
    - [4.1 Preferred – VS Code Azure extension](#41-preferred--vs-code-azure-extension)
    - [4.2 Alternative – Azure CLI](#42-alternative--azure-cli)
  - [5  Verify deployment](#5--verify-deployment)
  - [6  Next steps](#6--next-steps)

---

## 1  Prerequisites

- Windows 11 / WSL2 with PowerShell 7 or Bash
- .NET SDK ≥ 9.0.4 ( `dotnet --version` )
- Visual Studio Code with **Azure App Service** extension
- Azure CLI (optional) – sign in via `az login --use-device-code`
- An existing Azure App Service (Linux, .NET 9 stack)

### One-off App Service settings

| Setting                            | Value                          |
| ---------------------------------- | ------------------------------ |
| ASPNETCORE_ENVIRONMENT             | Production                     |
| WEBSITE_RUN_FROM_PACKAGE           | 1                              |
| SCM_DO_BUILD_DURING_DEPLOYMENT     | false                          |
| Startup Command (General Settings) | `dotnet BlazorCookbookApp.dll` |

---

## 2  Build & Package

**Run these commands from the workspace root directory** (where `BlazorCookbookApp.sln` is located):

```powershell
## Clean old output (optional)
Remove-Item -Recurse -Force .\bin\Publish -ErrorAction SilentlyContinue

## 1) Generate recipe manifest (required for Browse Recipes page)
dotnet run --project Tools/RecipeManifestGenerator

## 2) Publish the server project in Release mode
dotnet publish BlazorCookbookApp/BlazorCookbookApp.csproj -c Release -o .\bin\Publish

## 3) (Optional) create a zip for CLI deployments only
## Note: VS Code Azure extension uses the folder directly - no zip needed
Compress-Archive -Path '.\bin\Publish\*' -DestinationPath '.\blazor-recipes.zip' -Force
```

### Recipe Manifest Generation

**Purpose**: The manifest generation step creates `recipe-manifest.json` containing metadata for all recipes in the application. This solves the issue where the Browse Recipes page appears empty in production because `.razor` source files are not included in published output.

**What it does**:
- Scans all recipe pages for metadata (title, summary, stars, visibility)
- Generates `recipe-manifest.json` in the project root
- File gets included in `dotnet publish` and deployed to production
- Web application loads recipe data from JSON instead of scanning source files

**Output example**:
```
Found 13 recipes: 10 visible + 3 hidden
Generated manifest: recipe-manifest.json (5,638 bytes)
```

> **Gotchas**
> - Ensure no `global.json` is present in *bin\Publish* – it will break the
>   runtime image.
> - Exclude test artefacts: `<IsPublishable>false>` in the test project.

---

## 3  Test locally (optional)

To verify the published output works correctly before deploying to Azure:

```powershell
# Navigate to the publish folder
cd .\bin\Publish

# Run the published application
dotnet BlazorCookbookApp.dll

# The app will start on http://localhost:5000 and https://localhost:5001
```

**Manual test checklist:**
- Browse to `http://localhost:5000` → Home page loads correctly
- Browse to `http://localhost:5000/recipes` → Browse Recipes page shows recipe list (10 visible recipes)
- Test direct recipe routes: `/ch01r04`, `/ch01r04s`, `/ch01r04a`, `/ch01r04w`
- Health check: `http://localhost:5000/health` → returns `Healthy`
- Check console output for any errors or warnings
- Verify JSON manifest loading: Look for console message "Found X visible recipes (Y hidden)"

Press `Ctrl+C` to stop the application when finished testing.

---

## 4  Deploy

### 4.1 Preferred – VS Code Azure extension

1. Right-click the target App Service → **Deploy to Web App…**
2. Select the folder **bin\Publish** (no zip file needed).
3. Wait for *Deployment completed*; the extension handles packaging automatically.

### 4.2 Alternative – Azure CLI

```powershell
az webapp deploy `
  --resource-group <RG> `
  --name <AppServiceName> `
  --type zip `
  --src-path .\blazor-recipes.zip
```

---

## 5  Verify deployment

1. Portal → **Log stream** → look for
   `Executing startup command: dotnet BlazorCookbookApp.dll` and
   `Now listening on: http://0.0.0.0:8080`.
2. Browse to `https://<app>.azurewebsites.net/recipes` → **Browse Recipes page should show 10 visible recipes** (not empty).
3. Test a few direct recipe routes (e.g. `/ch01r04`).
4. Health check: `https://<app>.azurewebsites.net/health` → 200 OK.
5. Check application logs for JSON manifest loading confirmation.

---

## 6  Next steps

- Script the build + deploy commands in **publish.ps1** for convenience.
- Consider automating manifest generation with MSBuild tasks (future enhancement).
- Enable container log streaming (optional):

  ```powershell
  az webapp log config --name <App> --resource-group <RG> \
                       --docker-container-logging filesystem
  ```

---

*Last updated 2025-01-15 - Added recipe manifest generation step*
