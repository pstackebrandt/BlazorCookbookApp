# Blazor Cookbook App

![Version](https://img.shields.io/badge/Version-1.0.0-blue)
![.NET](https://img.shields.io/badge/.NET-9.0-purple)
![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-orange)
![Azure](https://img.shields.io/badge/Azure-Ready-lightblue)

A comprehensive Blazor application showcasing various recipes and techniques for
building modern web applications with Blazor WebAssembly and Server.

## Features

- **Overview**: Dynamic content overview.
- **Recipe4**: Render mode demonstrations (WebAssembly, Server, Auto)
  with authentic timing data and component lifecycle insights
- **WebAssembly Demo**: Interactive demonstration page showcasing WebAssembly
  capabilities with real-time performance metrics and hands-on examples

## Quick Start

1. **Run the application:**

   ```bash
   cd BlazorCookbookApp
   dotnet run
   ```

2. **Browse recipes:** Navigate to home page for complete recipe overview

3. **Add new recipes:** Follow `/ch##r##` naming pattern for auto-discovery

## Version Management

Version information is managed centrally and displayed in the application:

- **Current Version**: 1.0.0 (visible in navigation footer)
- **Central Management**: Version defined in `Directory.Build.props`
- **Runtime Access**: Version service provides version information to UI
- **Deployment Tracking**: Version verification included in deployment checklist

For version updates, see [VERSIONING.md](VERSIONING.md) for detailed workflow.

## Project Structure

- **`BlazorCookbookApp/`** - Server project with shared components
- **`BlazorCookbookApp.Client/`** - Client project with WebAssembly components
- **`docs/`** - Feature documentation and development guides
  - `Recipe4-Optimization-Plan.md` - Code duplication reduction and truthful
    state implementation
  - `Truthful-State-Design-Principle.md` - Revolutionary approach to authentic
    component education
  - `Educational-Delay-Design-Principle.md` - Real delay implementation
    standards (superseded)
  - `BlazorCookbook-Style-Guide.md` - UI consistency, color semantics, and
    coding standards

## Sources

Based on: [Blazor Web Development Cookbook](https://github.com/PacktPublishing/Blazor-Web-Development-Cookbook)
