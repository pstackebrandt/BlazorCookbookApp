# Production Build & Deploy Guide

This document is the **single source of truth** for creating a production
package and deploying *BlazorCookbookApp* to Azure App Service.

---

## 1  Prerequisites

* Windows 11 / WSL2 with PowerShell 7 or Bash
* .NET SDK ≥ 9.0.4 ( `dotnet --version` )
* Visual Studio Code with **Azure App Service** extension
* Azure CLI (optional) – sign in via `az login --use-device-code`
* An existing Azure App Service (Linux, .NET 9 stack)

### One-off App Service settings

| Setting                            | Value                          |
| ---------------------------------- | ------------------------------ |
| ASPNETCORE_ENVIRONMENT             | Production                     |
| WEBSITE_RUN_FROM_PACKAGE           | 1                              |
| SCM_DO_BUILD_DURING_DEPLOYMENT     | false                          |
| Startup Command (General Settings) | `dotnet BlazorCookbookApp.dll` |

---

## 2  Build & Package

```powershell
# Clean old output (optional)
Remove-Item -Recurse -Force .\bin\Publish -ErrorAction SilentlyContinue

# 1) Publish the server project in Release mode
 dotnet publish BlazorCookbookApp/BlazorCookbookApp.csproj `
              -c Release `
              -o .\bin\Publish

# 2) The .razor files are copied automatically (quick-fix for RecipeScanner)
#    thanks to <Content CopyToPublishDirectory="Always"/> in the csproj.

# 3) (Optional) create a zip for CLI deployments
Compress-Archive -Path '.\bin\Publish\*' `
                 -DestinationPath '.\blazor-recipes.zip' -Force
```

> **Gotchas**
> * Ensure no `global.json` is present in *bin\Publish* – it will break the
>   runtime image.
> * Exclude test artefacts: `<IsPublishable>false>` in the test project.

---

## 3  Deploy

### 3.1 Preferred – VS Code Azure extension

1. Right-click the target App Service → **Deploy to Web App…**
2. Select the folder **bin\Publish**.
3. Wait for *Deployment completed*; the extension mounts the zip automatically.

### 3.2 Alternative – Azure CLI

```powershell
az webapp deploy `
  --resource-group <RG> `
  --name <AppServiceName> `
  --type zip `
  --src-path .\blazor-recipes.zip
```

---

## 4  Verify deployment

1. Portal → **Log stream** → look for
   `Executing startup command: dotnet BlazorCookbookApp.dll` and
   `Now listening on: http://0.0.0.0:8080`.
2. Browse to `https://<app>.azurewebsites.net/recipes` – the list must show data.
3. Test a few direct recipe routes (e.g. `/ch01r04`).
4. Health check: `https://<app>.azurewebsites.net/health` → 200 OK.

---

## 5  Next steps

* Script the build + deploy commands in **publish.ps1** for convenience.
* Investigate a build-time manifest (# T18.3) to remove the need to copy
  `.razor` sources.
* Enable container log streaming (optional):

  ```powershell
  az webapp log config --name <App> --resource-group <RG> \
                       --docker-container-logging filesystem
  ```

---

_Last updated 2025-07-10_ 