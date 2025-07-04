# Cursor AI Terminal Command Handling Guide

## 🎯 **Overview**
This guide explains how to handle terminal commands in Cursor AI when they appear to hang, take a long time, or don't provide clear completion signals.

## ⚡ **The Shift+Skip Solution**

When a terminal command appears to be hanging or taking too long, **Shift+Skip** is often the best solution.

### **When to Use Shift+Skip:**

✅ **Good situations to use Shift+Skip:**
- Long-running commands that are working but taking time
- Build processes that are successful but verbose
- When you see the output you need and don't want to wait for completion
- Terminal commands that are "hanging" but actually working
- Commands with extensive output that you don't need to see completely
- Development server starts that show "listening on port..." but continue running

### **Examples of Commands That Often Need Shift+Skip:**
```powershell
# These commands often run indefinitely or have long output
dotnet run                    # Starts development server
dotnet build                  # Can have verbose output
npm install                   # Long dependency installation
git log --oneline            # Can have many entries
dotnet test                   # Extensive test output
```

## 🔄 **Alternative Approaches**

### **1. Background Commands**
For long-running processes, use `is_background: true`:
```powershell
# This allows the command to run while continuing work
dotnet run --project BlazorCookbookApp/BlazorCookbookApp.csproj
# Set is_background: true in the AI tool call
```

### **2. Specific Project Paths**
For dotnet commands, be more specific to avoid ambiguity:
```powershell
# Instead of generic commands
dotnet build

# Use specific project paths
dotnet build BlazorCookbookApp/BlazorCookbookApp.csproj
dotnet run --project BlazorCookbookApp/BlazorCookbookApp.csproj
```

### **3. Shorter Verification Commands**
Use simpler commands for quick verification:
```powershell
# Instead of full builds
dotnet build

# Use quick checks
dotnet --version
dotnet list package
ls -la
```

### **4. Output Limiting**
Add output limiting to verbose commands:
```powershell
# Limit output with head/tail
git log --oneline -10
dotnet build | head -20

# On Windows PowerShell
git log --oneline | Select-Object -First 10
```

## 🚨 **Common Scenarios**

### **Scenario 1: Development Server Start**
```
Command: dotnet run
Output: "Now listening on: https://localhost:5001"
Status: Command appears to hang
Solution: Use Shift+Skip - the server is running correctly
```

### **Scenario 2: Build Process**
```
Command: dotnet build
Output: Shows build progress, then seems to stop
Status: Build completed but waiting for user input
Solution: Use Shift+Skip - build was successful
```

### **Scenario 3: Package Installation**
```
Command: npm install
Output: Shows package downloads, then long pause
Status: Installation working but taking time
Solution: Use Shift+Skip or set is_background: true
```

## ⚠️ **When NOT to Use Shift+Skip**

❌ **Avoid Shift+Skip when:**
- Command shows obvious error messages
- You need to see the complete output for debugging
- Command is asking for user input (password, confirmation)
- You're troubleshooting a specific issue

## 🛠️ **Best Practices**

### **For AI Assistants:**
1. **Use Background Flag**: Set `is_background: true` for long-running commands
2. **Be Specific**: Use full project paths in dotnet commands
3. **Limit Output**: Add output limiting to verbose commands
4. **Explain Expectations**: Tell users what to expect from commands

### **For Users:**
1. **Monitor Output**: Watch for success indicators before using Shift+Skip
2. **Check Results**: Verify the command worked after using Shift+Skip
3. **Be Patient**: Some commands legitimately take time
4. **Use Judgment**: Learn to recognize when commands are working vs. stuck

## 📋 **Quick Reference**

| Command Type   | Typical Behavior             | Recommended Action                      |
| -------------- | ---------------------------- | --------------------------------------- |
| `dotnet run`   | Starts server, keeps running | Shift+Skip after "listening on" message |
| `dotnet build` | Shows build progress         | Shift+Skip after "Build succeeded"      |
| `npm install`  | Downloads packages           | Background or Shift+Skip                |
| `git log`      | Shows commit history         | Limit output or Shift+Skip              |
| `dotnet test`  | Runs tests                   | Shift+Skip after test summary           |

## 🎯 **Key Takeaway**

**Shift+Skip is a powerful tool for maintaining productivity when working with Cursor AI terminal commands. Use it confidently when you can see the command has accomplished its purpose, even if it hasn't formally "completed."**

---

*This guide helps optimize the development workflow in Cursor AI by providing clear guidance on when and how to handle terminal command situations effectively.* 