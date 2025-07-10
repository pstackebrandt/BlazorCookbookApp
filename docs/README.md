# Documentation Directory

**Blazor Cookbook App Documentation** - Organized by topic with time-based archive

## 📁 Directory Structure

### **Active Documentation**

#### **📁 deployment/** - Production deployment guides
- **Purpose**: Creating production builds and deploying to Azure
- **Use when**: Building for production, deploying to Azure App Service, configuring production settings
- **Key files**: `Azure-Deployment-Checklist.md`, `Local-Production-Testing-Guide.md`

#### **📁 development/** - Development guides and standards  
- **Purpose**: Coding standards, design principles, development tools
- **Use when**: Setting up development environment, following coding standards, implementing features
- **Key files**: `BlazorCookbook-Style-Guide.md`, `Truthful-State-Design-Principle.md`

#### **📁 features/** - Feature planning and implementation
- **Purpose**: Planning and implementing new features and pages
- **Use when**: Adding new features, planning page content, implementing UI components
- **Key files**: Feature-specific implementation plans and content strategies

#### **📁 project-management/** - Project coordination
- **Purpose**: Project status tracking and organizational documentation
- **Use when**: Checking project status, understanding current priorities, coordinating work
- **Key files**: `Current-Status-Summary.md`

### **Historical Documentation**

#### **📁 archive/** - Completed and deferred work
- **📁 2025-q1/completed-features/** - Completed feature documentation
- **📁 2025-q1/deferred-projects/** - Deferred/postponed work  
- **📁 2025-q1/documentation-history/** - Documentation reorganization records
- **📁 2025-q1/exported-chats/** - Chat exports and conversations

#### **📁 logs/** - Development logs and traces

## 🔍 Quick Navigation

### **I want to...**

#### **Deploy to production**
→ `deployment/Azure-Deployment-Checklist.md` - Step-by-step deployment guide  
→ `deployment/Local-Production-Testing-Guide.md` - Test production builds locally

#### **Follow coding standards**
→ `development/BlazorCookbook-Style-Guide.md` - UI consistency and coding standards  
→ `development/Truthful-State-Design-Principle.md` - Core design principle

#### **Implement a new feature**
→ `features/` - Check existing feature implementation plans  
→ `development/BlazorCookbook-Style-Guide.md` - Follow coding standards

#### **Check project status**
→ `project-management/Current-Status-Summary.md` - Current status and priorities

#### **Find completed work**
→ `archive/2025-q1/completed-features/` - Completed feature documentation

## 📝 File Naming Conventions

- **kebab-case**: `Azure-Deployment-Checklist.md`
- **Descriptive purpose**: `Implementation-Plan`, `Testing-Guide`, `Style-Guide`
- **Series prefixes**: `Recipe4-*`, `Azure-*`
- **Date organizational docs**: `Documentation-Reorganization-2025.md`

## 🔄 Document Lifecycle

### **Active → Archive**
1. **Active**: Current work and reference documentation
2. **Completed**: Feature implemented, planning document archived
3. **Archived**: Moved to `archive/YYYY-qN/completed-features/`

### **When to Archive**
- ✅ Feature implementation completed
- ✅ Documentation superseded by newer version
- ✅ Project deferred to future quarter
- ✅ Historical reorganization records

## 📋 Contributing Documentation

### **Adding New Documentation**
1. **Identify purpose**: deployment, development, feature, or project management
2. **Choose folder**: Place in appropriate topic-based folder
3. **Follow naming**: Use consistent kebab-case naming
4. **Update README**: Add to quick navigation if significant

### **Updating Existing Documentation**
- Keep active documentation current and relevant
- Archive completed planning documents
- Update cross-references when moving files

---

**Last updated**: January 10, 2025 - Documentation reorganization to topic-based structure 