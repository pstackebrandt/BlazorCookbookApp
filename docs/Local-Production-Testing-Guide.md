# Local Production Testing Guide

This guide shows how to test your Blazor application in production mode locally before Azure deployment.

## 🎯 Overview

Testing production builds locally helps identify configuration issues, performance problems,
and deployment-specific behavior before going live.

This is a critical step in the deployment process.

## 🔧 Testing Methods

**💡 All methods should be run from the solution root directory: `/BlazorCookbookApp/`**

### Method 1: Test Published Output (Recommended)

```powershell
# 1. Clean previous builds
dotnet clean

# 2. Publish like you would for Azure
dotnet publish BlazorCookbookApp --configuration Release --output ./publish

# 3. Run the published version
cd publish
dotnet BlazorCookbookApp.dll --environment Production

# 4. Navigate back when done
cd ..
```

**Best for**: Testing the exact output that will be deployed to Azure

### Method 2: Environment Variable Approach

```powershell
# 1. Clean previous builds
dotnet clean

# 2. Set environment to Production
$env:ASPNETCORE_ENVIRONMENT="Production"

# 3. Build in Release configuration
dotnet build --configuration Release

# 4. Run with production settings
dotnet run --project BlazorCookbookApp --configuration Release
```

**Best for**: Quick production configuration testing with clean build

### Method 3: Explicit Environment Parameter

```powershell
# 1. Clean previous builds
dotnet clean

# 2. Build for production
dotnet build --configuration Release

# 3. Run with explicit environment (without setting env variable)
dotnet run --project BlazorCookbookApp --configuration Release --environment Production
```

**Best for**: Testing without modifying environment variables

## 📋 Testing Checklist

### Essential Functionality Tests

- [ ] **Application Starts**: No startup errors or exceptions
- [ ] **Version Display**: "Version 1.0.0" appears in navigation footer
- [ ] **All Pages Load**: Home, Recipes, Resources, Learning Journey, Legal Notice
- [ ] **Navigation**: All menu items and links work correctly
- [ ] **Recipe Overview**: Browse recipes functionality works
- [ ] **WebAssembly Components**: Interactive components function correctly
- [ ] **Static Assets**: Images, CSS, JavaScript load properly
- [ ] **Mobile Responsiveness**: Test responsive design

### Production-Specific Behavior

- [ ] **Logging Level**: Console shows only Warning/Error messages (not Information)
- [ ] **Error Handling**: Generic error messages displayed (no stack traces)
- [ ] **HTTPS Redirection**: Should NOT redirect locally (development-only feature)
- [ ] **Static Files**: MapStaticAssets() works correctly
- [ ] **Performance**: Check load times and asset optimization

### Browser Testing

- [ ] **Console Errors**: No JavaScript errors in browser dev tools
- [ ] **Network Tab**: All assets load successfully (no 404s)
- [ ] **WebAssembly Loading**: Check `_framework/` files load correctly
- [ ] **Service Worker**: Verify if applicable
- [ ] **Cache Headers**: Check static asset caching

## 🚨 Common Issues to Watch For

### Configuration Problems
- **Wrong appsettings loaded**: Verify `appsettings.Production.json` is used
- **Missing environment variables**: Check required settings
- **Logging configuration**: Ensure production logging levels

### Asset Loading Issues
- **Static files missing**: Check wwwroot folder in publish output
- **WebAssembly files missing**: Verify `_framework/` folder contents
- **CSS/JS not loading**: Check file paths and compression
- **Images not displaying**: Verify image optimization and paths

### Performance Issues
- **Slow startup**: Check for development-only code running
- **Large bundle size**: Review WebAssembly payload
- **Unoptimized assets**: Verify compression and minification

## 🔍 Debugging Tips

### Check Published Output Structure
```powershell
# After publish, verify file structure
dir publish -Recurse
```

**Expected structure**:
```
publish/
├── BlazorCookbookApp.dll
├── BlazorCookbookApp.Client.dll
├── appsettings.json
├── appsettings.Production.json
└── wwwroot/
    ├── _framework/
    │   ├── *.wasm files
    │   ├── *.dll files
    │   └── blazor.boot.json
    ├── css/
    ├── lib/
    └── favicon.png
```

### Console Log Analysis
```powershell
# Monitor application logs during testing
dotnet BlazorCookbookApp.dll --environment Production --verbose
```

### Browser Developer Tools
1. **F12** → **Console**: Check for JavaScript errors
2. **Network Tab**: Monitor asset loading
3. **Application Tab**: Check service workers and cache
4. **Performance Tab**: Analyze load times

## 📊 Performance Baseline

### Metrics to Record
- **Application startup time**: Time from launch to ready
- **First page load**: Time to display home page
- **WebAssembly load time**: Time for interactive components
- **Bundle size**: Total download size
- **Memory usage**: Browser memory consumption

### Comparison Points
- **Development vs Production**: Performance differences
- **Before vs After optimization**: Improvement measurements
- **Local vs Azure**: Expected performance variations

## 🎯 Success Criteria

### Ready for Deployment
- ✅ All functionality works identically to development
- ✅ No console errors or asset loading failures
- ✅ Performance meets expectations
- ✅ Production configuration loads correctly
- ✅ Version management works correctly

### Common Fixes Before Deployment
- Update missing production settings
- Optimize large static assets
- Fix asset path issues
- Resolve configuration conflicts
- Address performance bottlenecks

## 🚀 Next Steps After Testing

### If Testing Succeeds
1. Document performance baseline
2. Proceed with Azure deployment
3. Compare production results with local testing

### If Issues Found
1. Fix identified problems
2. Re-test in production mode
3. Compare with development mode to isolate issues
4. Update configuration as needed

## 📝 Notes

- **Environment Reset**: Remember to reset environment variable after testing
- **Clean Builds**: `dotnet clean` ensures no stale artifacts from previous builds with different configurations
- **Browser Cache**: Clear browser cache between tests for accurate results
- **Port Conflicts**: Ensure no other applications using same ports
- **Build Reliability**: Always clean before production testing to ensure known build state

## 🔄 Reset Environment

After testing, reset your environment:

```powershell
# Reset environment variable
$env:ASPNETCORE_ENVIRONMENT=""

# Or restart PowerShell/terminal
```

This ensures development mode returns to normal behavior.