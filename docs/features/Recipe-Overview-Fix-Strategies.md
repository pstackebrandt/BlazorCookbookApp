# Fixing the "Browse Recipes" Page

## Background

The **Browse Recipes** page shows an empty list after a deployment to Azure.
Locally everything works because `RecipeScanner` reads the original `.razor` source files from disk.
During `dotnet publish`, however, the Razor compiler turns every component into IL and
**does not copy the `.razor` files** into the publish folder.
The deployed package therefore contains only DLLs, so the scanner finds nothing and returns an empty list.

## Viable Strategies to restore the recipe catalogue

| #   | Approach                      | What you do                                                                                                                                                                                       | Pros                                                                             | Cons                                                                                                                                    |
| --- | ----------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------- |
| 1   | **Build-time manifest**       | During the build create a JSON (or C# class) that contains all recipe metadata and copy only that file to the publish folder. `RecipeScanner` loads the JSON instead of scanning the file system. | - Small package.<br>- No source exposure.<br>- Zero runtime I/O; fastest option. | - Needs a custom MSBuild task or a small console tool.<br>- New build step to maintain.                                                 |
| 2   | **Reflection at runtime**     | Iterate over all loaded assemblies and read the `RouteAttribute` plus custom attributes (or static fields) for title/summary/stars.                                                               | - No extra files; nothing exposed.<br>- Works with IL only.                      | - Reflection cost at startup.<br>- Must add attributes or static properties to every page.<br>- Logic slightly more complex than regex. |
| 3   | **Source-generator manifest** | Use a Roslyn Source-Generator to emit a static `RecipeCatalog` class at compile time.                                                                                                             | - No external files.<br>- Fast lookup, strongly-typed.                           | - Requires familiarity with source generators.<br>- Adds build complexity.                                                              |

## Rejected Strategies

### Strategy: Ship the source files (attempted quick fix)

**What was tried:** Mark all required `.razor` files as `<Content CopyToPublishDirectory="Always"/>` in the *server* project.

**Why it failed:** The .NET SDK automatically includes all `.razor` files as Content items for build-time processing. When manual `<Content Include="...">` entries are added to copy files to the publish directory, it creates duplicate Content items, causing build error **NETSDK1022**.

**The fundamental conflict:**
- **SDK's intent:** Compile `.razor` files into IL, exclude source files from production for security and package size
- **RecipeScanner's need:** Access original source files at runtime to extract metadata
- **Result:** Both SDK and manual includes claim ownership of the same files

**Lessons learned:**
- The SDK's implicit Content inclusion is designed for build-time processing only
- Overriding SDK behavior with manual includes creates conflicts
- This approach fundamentally opposes the SDK's compilation philosophy

## Why the original solution breaks after deployment

1. `dotnet publish` **excludes** all `.razor` files from the output.
2. On Azure the `RecipeScanner` looks under `/home/site/wwwroot/Components/**.razor` etc. and finds nothing.
3. It returns an empty list; the UI falls back to *"No recipes found."*
4. while direct navigation still works because routing is compiled into the DLL.

## How you could detect the issue locally

1. **Run the published output** locally:

   ```powershell
   dotnet publish BlazorCookbookApp -c Release -o ./tmp/publish
   cd ./tmp/publish
   dotnet BlazorCookbookApp.dll
   ```

   Browse to `http://localhost:5000/recipes` – the list would already be empty.
2. **Unit / integration test** that asserts `RecipeScanner.GetRecipesAsync()` returns >0 after a publish build.
3. **Container test**: build the same App Service base image locally and run the site in Docker.

## Recommendation for the project roadmap

- **Short term**: Implement approach #1 (build-time JSON manifest) – balances performance & simplicity
- **Mid term**: Evaluate approach #2 (reflection) if build-time manifest proves insufficient
- **Long term**: Consider approach #3 (source generator) once the codebase stabilizes

---

*Document updated 15 Jan 2025 to reflect SDK conflict discoveries and move rejected strategies.*