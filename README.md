# Blazor Cookbook App

![Version](https://img.shields.io/badge/Version-1.0.2-blue)
![.NET](https://img.shields.io/badge/.NET-9.0-purple)
![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-orange)
![Azure](https://img.shields.io/badge/Azure-Ready-lightblue)

Educational Blazor application demonstrating real-world patterns and techniques.
Based on "Blazor Web Development Cookbook" with enhanced examples and production-ready features.

## 📋 Table of Contents

- [Blazor Cookbook App](#blazor-cookbook-app)
  - [📋 Table of Contents](#-table-of-contents)
  - [🌐 Live Demo](#-live-demo)
  - [What You'll Find](#what-youll-find)
  - [Quick Start](#quick-start)
  - [Version Management](#version-management)
  - [Technology Stack](#technology-stack)
  - [Project Structure](#project-structure)
  - [Sources & Attribution](#sources--attribution)
  - [About the Developer](#about-the-developer)

## 🌐 Live Demo

[Live Demo](https://blazor-recipes-bwaabwhrejgqehgp.germanywestcentral-01.azurewebsites.net/)

## What You'll Find

- **Recipe Browser**: Browse 15+ interactive Blazor examples
- **Render Mode Comparison**: See Server vs WebAssembly vs Auto modes in action
- **Performance Insights**: Real-time metrics and component lifecycle visualization
- **Mobile Responsive**: Optimized for all device sizes
- **Production Ready**: Version management, deployment configuration, comprehensive testing

## Quick Start

1. **Run the application:**

   ```bash
   cd BlazorCookbookApp
   dotnet run
   ```

2. **Browse recipes:** Navigate to home page for complete recipe overview

3. **Add new recipes:** Follow `/ch##r##` naming pattern for auto-discovery

## Version Management

Version is managed centrally in `Directory.Build.props`. See [docs/project-management/Versioning-Strategy.md](docs/project-management/Versioning-Strategy.md) for update workflow. The version badge above reflects the current release. (No need to update version numbers manually in this file.)

## Technology Stack

- **.NET 9.0** - Latest framework features
- **Blazor WebAssembly & Server** - Hybrid rendering approach
- **Bootstrap 5** - Responsive UI components
- **xUnit & bUnit** - Comprehensive testing (96 tests)
- **Azure App Service** - Production deployment target

## Project Structure

- **`BlazorCookbookApp/`** - Server project with shared components
- **`BlazorCookbookApp.Client/`** - WebAssembly client project
- **`BlazorCookbookApp.Tests/`** - Comprehensive test suite
- **`docs/`** - Development guides and deployment documentation

## Sources & Attribution

- **Primary Source:** [Blazor Web Development Cookbook](https://www.packtpub.com/product/blazor-web-development-cookbook/9781803241524) by Pawel Bazyluk (Packt Publishing)
- **Enhancements:** Production features, comprehensive testing, mobile optimization
- **Purpose:** Educational reference for Blazor development patterns
- **Repository:** [GitHub](https://github.com/pstackebrandt/BlazorCookbookApp)

## About the Developer

Peter Stackebrandt  
[Homepage](https://pstackebrandt.github.io/)  
[GitHub](https://github.com/pstackebrandt)  
Email: peter.stackebrandt [at] gmail [dot] com
