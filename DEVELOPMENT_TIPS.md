# Development Tips

## Blazor Project Structure

### Project Reference Structure (Microsoft Recommended)

**Server project should reference Client project** - This is Microsoft's official recommendation for Blazor Web Apps:

- **Deployment**: Server hosts and serves the client application
- **Bundling**: Server includes the client's WebAssembly bundle in its output  
- **Shared Types**: Server can access types defined in client project for API contracts
- **Architecture**: Follows typical web application pattern where server is the entry point

### Avoiding Duplicate Files

- **Shared Types**: Place shared types (like `RenderAction.cs`) in the client project only
- **Server Access**: Server project can access client types through project reference
- **Maintenance**: Eliminates duplicate code and maintenance burden
- **Rule**: Since `InteractiveAuto` components must be in client project, keep shared types there too

## Blazor Render Modes

### Component Project Placement Rules

| Render Mode              | Required Location  | Reason                                        |
| ------------------------ | ------------------ | --------------------------------------------- |
| `Static`                 | Server Project     | No client execution needed                    |
| `InteractiveServer`      | Server Project     | Executes only on server                       |
| `InteractiveWebAssembly` | **Client Project** | Must be in WASM bundle                        |
| `InteractiveAuto`        | **Client Project** | Must support both server and client execution |

**Key Rule**: `InteractiveAuto` and `InteractiveWebAssembly` components MUST be in the client project to be included in the WebAssembly bundle for client-side execution.

### InteractiveWebAssembly & InteractiveAuto Components

- **Service Registration**: Require service registration in both server and client projects
  - Components pre-render on the server first, then hydrate on the client
  - Server needs the service for pre-rendering phase
  - Client needs the service for interactive phase
  - Register the same service in both `BlazorCookbookApp/Program.cs` and `BlazorCookbookApp.Client/Program.cs`

## Testing

### Recipe4 Optimization Testing Commands

```bash
# When ready to run tests (after stopping Blazor app):
dotnet test BlazorCookbookApp.Tests

# To run tests continuously during development:
dotnet test BlazorCookbookApp.Tests --watch

# To see detailed test output:
dotnet test BlazorCookbookApp.Tests --verbosity normal
```

**Important**: Stop the running Blazor application before testing to avoid file lock issues.

## Best Practices

### Service Implementation

- Use interfaces for dependency injection to enable testing and flexibility
- Add comprehensive XML documentation for IntelliSense support
- Handle error cases gracefully (e.g., URL parsing failures)

### Project Structure

- Keep shared services in appropriate namespaces
- Add global using statements in `_Imports.razor` for commonly used namespaces
- Use scoped services for request-specific data that doesn't need to persist
