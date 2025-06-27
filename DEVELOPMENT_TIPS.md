# Development Tips

## Blazor Render Modes

### InteractiveWebAssembly Components

- **Service Registration**: InteractiveWebAssembly components require service registration in both server and client projects
  - Components pre-render on the server first, then hydrate on the client
  - Server needs the service for pre-rendering phase
  - Client needs the service for interactive phase
  - Register the same service in both `BlazorCookbookApp/Program.cs` and `BlazorCookbookApp.Client/Program.cs`

## Best Practices

### Service Implementation

- Use interfaces for dependency injection to enable testing and flexibility
- Add comprehensive XML documentation for IntelliSense support
- Handle error cases gracefully (e.g., URL parsing failures)

### Project Structure

- Keep shared services in appropriate namespaces
- Add global using statements in `_Imports.razor` for commonly used namespaces
- Use scoped services for request-specific data that doesn't need to persist
