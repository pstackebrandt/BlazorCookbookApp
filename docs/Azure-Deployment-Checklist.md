# Azure Deployment Checklist - Blazor Cookbook App

## 🎯 **Pre-Deployment Preparation**

### **✅ Step 1: Verify Local Configuration**
- [ ] **appsettings.Production.json** exists with optimized logging
- [ ] **Program.cs** has proper WebAssembly and static file configuration
- [ ] **All appsettings files** are committed to version control
- [ ] **Project builds** successfully in Release mode
- [ ] **WebAssembly files** are included in publish output

### **✅ Step 2: Create Production Build**
```bash
# Navigate to main project directory
cd BlazorCookbookApp

# Clean previous builds
dotnet clean

# Restore dependencies
dotnet restore

# Build in Release mode
dotnet build --configuration Release

# Publish for production
dotnet publish --configuration Release --output ../publish
```

### **✅ Step 3: Verify Publish Output**
- [ ] **publish/** folder created successfully
- [ ] **wwwroot/_framework/** contains WebAssembly files (.wasm, .js)
- [ ] **appsettings.Production.json** included in publish output
- [ ] **Compressed files** (.br, .gz) present for performance
- [ ] **All static assets** (CSS, images) included

### **✅ Step 4: Create Deployment Package**
```bash
# Navigate to publish folder
cd publish

# Create ZIP package (Windows PowerShell)
Compress-Archive -Path * -DestinationPath ../BlazorCookbookApp-Deploy.zip

# Alternative: Use file explorer to create ZIP of all contents in publish folder
```

## 🚀 **Azure App Service Setup**

### **✅ Step 5: Create Azure App Service**
1. **Login to Azure Portal**: https://portal.azure.com
2. **Create Resource** → **Web App**
3. **Configure Basic Settings**:
   - **Subscription**: Your Azure subscription
   - **Resource Group**: Create new → `BlazorCookbookApp-RG`
   - **Name**: `blazor-cookbook-[yourname]` (must be globally unique)
   - **Publish**: Code
   - **Runtime stack**: .NET 9 (STS)
   - **Operating System**: Linux
   - **Region**: West Europe (or your preferred region)

4. **Configure Pricing**:
   - **Pricing Tier**: F1 Free (0 cost)
   - **Linux Plan**: Create new or use existing

5. **Review and Create**
   - [ ] Verify all settings are correct
   - [ ] Click **Create** and wait for deployment

### **✅ Step 6: Configure App Service Settings**
1. **Navigate to App Service** → **Configuration** → **Application settings**
2. **Add Required Settings**:

   **Essential Settings (Add these):**
   ```
   ASPNETCORE_ENVIRONMENT = Production
   WEBSITE_RUN_FROM_PACKAGE = 1
   SCM_DO_BUILD_DURING_DEPLOYMENT = false
   ```

   **Optional Performance Settings:**
   ```
   WEBSITE_ENABLE_SYNC_UPDATE_SITE = true
   WEBSITE_RUN_FROM_PACKAGE_BLOB = false
   WEBSITE_HTTPLOGGING_RETENTION_DAYS = 3
   WEBSITES_ENABLE_APP_SERVICE_STORAGE = false
   ```

3. **Click Save** and wait for restart

### **✅ Step 7: Deploy Application**
1. **Go to Deployment Center** → **Manual Deployment (Local Git/ZIP)**
2. **Upload ZIP Package**:
   - Click **Browse** and select `BlazorCookbookApp-Deploy.zip`
   - Click **Upload**
   - Wait for deployment to complete

3. **Alternative: Using Azure CLI**:
   ```bash
   az webapp deployment source config-zip \
     --resource-group BlazorCookbookApp-RG \
     --name blazor-cookbook-[yourname] \
     --src BlazorCookbookApp-Deploy.zip
   ```

## 🔍 **Post-Deployment Verification**

### **✅ Step 8: Basic Functionality Test**
- [ ] **App loads** at `https://blazor-cookbook-[yourname].azurewebsites.net`
- [ ] **Home page** displays correctly
- [ ] **Navigation menu** works (hamburger menu on mobile)
- [ ] **Browse Recipes** page loads with recipe table
- [ ] **Recipe pages** load (test a few different recipes)
- [ ] **No console errors** in browser developer tools

### **✅ Step 9: WebAssembly Functionality Test**
- [ ] **WebAssembly pages** load correctly (`/ch01r04`)
- [ ] **Interactive features** work (buttons, state changes)
- [ ] **Render mode detection** shows correct values
- [ ] **Component lifecycle** tracking works
- [ ] **No WebAssembly loading errors** in browser console

### **✅ Step 10: Mobile Responsiveness Test**
- [ ] **Mobile navigation** (hamburger menu) works
- [ ] **Recipe table** scrolls horizontally on mobile
- [ ] **Column priority** system works (only essential columns on mobile)
- [ ] **Touch interactions** work properly
- [ ] **Text readability** is acceptable on mobile

### **✅ Step 11: Performance Verification**
- [ ] **Initial page load** < 5 seconds (F1 Free tier)
- [ ] **WebAssembly load** < 10 seconds (first visit)
- [ ] **Navigation** between pages is responsive
- [ ] **Static assets** load correctly (CSS, images)
- [ ] **Compressed files** are being served (.br, .gz)

## 📊 **Monitoring Setup (Optional)**

### **✅ Step 12: Enable Application Insights**
1. **Go to App Service** → **Application Insights**
2. **Turn on Application Insights**
3. **Create new resource** or use existing
4. **Configure settings**:
   - **Sampling**: 100% (low traffic expected)
   - **Profiler**: Enabled
   - **Snapshot Debugger**: Enabled

### **✅ Step 13: Configure Alerts**
1. **Go to Application Insights** → **Alerts**
2. **Create alert rules**:
   - **High response time** (> 10 seconds)
   - **High error rate** (> 5%)
   - **App down** (availability < 95%)

## 🛠️ **Troubleshooting Common Issues**

### **App Won't Start**
- [ ] Check **ASPNETCORE_ENVIRONMENT** is set to "Production"
- [ ] Verify **appsettings.Production.json** exists in deployment
- [ ] Check **App Service Logs** for startup errors
- [ ] Ensure **.NET 9** runtime is selected

### **WebAssembly Not Working**
- [ ] Verify **WebAssembly files** (.wasm) are in `wwwroot/_framework/`
- [ ] Check **browser console** for loading errors
- [ ] Ensure **MIME types** are properly configured (automatic on Linux)
- [ ] Test with **different browsers** (Chrome, Firefox, Safari)

### **Performance Issues**
- [ ] **Cold start** expected on F1 Free (first request after 20 min idle)
- [ ] Check **Application Insights** for performance bottlenecks
- [ ] Verify **compressed files** are being served
- [ ] Consider **warming up** with external monitoring

### **Mobile Issues**
- [ ] Test **actual mobile devices** vs browser emulation
- [ ] Check **viewport meta tag** is present
- [ ] Verify **Bootstrap responsive** classes are working
- [ ] Test **touch interactions** on real devices

## 📝 **Post-Deployment Tasks**

### **✅ Step 14: Update Documentation**
- [ ] **Update README.md** with live URL
- [ ] **Document deployment process** for future reference
- [ ] **Update project status** in documentation
- [ ] **Add deployment badge** to README (optional)

### **✅ Step 15: Final Verification**
- [ ] **Share URL** with others for testing
- [ ] **Test from different networks** (mobile data, different ISPs)
- [ ] **Monitor for 24-48 hours** for stability
- [ ] **Check Azure costs** (should be $0 for F1 Free)

## 🎉 **Deployment Success Criteria**

### **✅ Minimum Success Requirements**
- [ ] App loads at Azure URL
- [ ] All pages accessible
- [ ] WebAssembly interactivity works
- [ ] Mobile responsiveness functional
- [ ] No critical errors in logs

### **✅ Optimal Success Requirements**
- [ ] Page load times acceptable
- [ ] All features working as expected
- [ ] Monitoring configured
- [ ] Performance metrics baseline established
- [ ] Documentation updated

---

## 📞 **Support Resources**

- **Azure App Service Documentation**: https://docs.microsoft.com/en-us/azure/app-service/
- **Blazor Hosting Documentation**: https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models
- **Azure Free Tier Limits**: https://azure.microsoft.com/en-us/pricing/details/app-service/windows/

## 🔄 **Rollback Plan**

If deployment fails:
1. **Keep previous deployment package** as backup
2. **Document any configuration changes** made
3. **Use Azure deployment slots** for safe deployment (if available)
4. **Have local development environment** ready for quick fixes

---

**Estimated Total Time**: 1-2 hours for first deployment, 15-30 minutes for subsequent deployments