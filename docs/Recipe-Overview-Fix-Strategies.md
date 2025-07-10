# Fixing the "Browse Recipes" Page

## Background

The **Browse Recipes** page shows an empty list after a deployment to Azure.
Locally everything works because `RecipeScanner` reads the original `.razor` source files from disk.
During `dotnet publish`, however, the Razor compiler turns every component into IL and
**does not copy the `.razor` files** into the publish folder.
The deployed package therefore contains only DLLs, so the scanner finds nothing and returns an empty list.

## Strategies to restore the recipe catalogue

| #   | Approach                              | What you do                                                                                                                                                                                       | Pros                                                                                  | Cons                                                                                                                                    |
| --- | ------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------- |
| 1   | **Ship the source files** (quick fix) | Mark all required `.razor` files as `<Content CopyToPublishDirectory="Always"/>` in the *server* project.                                                                                         | * Very little code change.<br>* Works immediately.<br>* Keeps existing runtime logic. | * ZIP grows (≈ 200–500 KB per recipe).<br>* Source code becomes publicly accessible.<br>* Still performs I/O + regex at runtime.        |
| 2   | **Build-time manifest**               | During the build create a JSON (or C# class) that contains all recipe metadata and copy only that file to the publish folder. `RecipeScanner` loads the JSON instead of scanning the file system. | * Small package.<br>* No source exposure.<br>* Zero runtime I/O; fastest option.      | * Needs a custom MSBuild task or a small console tool.<br>* New build step to maintain.                                                 |
| 3   | **Reflection at runtime**             | Iterate over all loaded assemblies and read the `RouteAttribute` plus custom attributes (or static fields) for title/summary/stars.                                                               | * No extra files; nothing exposed.<br>* Works with IL only.                           | * Reflection cost at startup.<br>* Must add attributes or static properties to every page.<br>* Logic slightly more complex than regex. |
| 4   | **Source-generator manifest**         | Use a Roslyn Source-Generator to emit a static `RecipeCatalog` class at compile time.                                                                                                             | * No external files.<br>* Fast lookup, strongly-typed.                                | * Requires familiarity with source generators.<br>* Adds build complexity.                                                              |

## Why the original solution breaks after deployment

1. `dotnet publish` **excludes** all `.razor` files from the output.
2. On Azure the `RecipeScanner` looks under `/home/site/wwwroot/Components/**.razor` etc. and finds nothing.
3. It returns an empty list; the UI falls back to *“No recipes found.”*
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

* **Short term** (already implemented): approach #1 – copy `.razor` files. Quick fix, unblocks the demo.
* **Mid term**: evaluate approach #2 (build-time JSON) – balances performance & simplicity.
* **Long term**: if you want compile-time safety, consider approach #4 (source generator) once the codebase stabilises.

---

*Document created 9 Jul 2025 to capture the reasoning and options for future work.*