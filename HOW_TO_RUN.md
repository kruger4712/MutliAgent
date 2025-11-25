# Running the Application

## Two Ways to Run

The Multi-Agent Learning System now has **two interfaces**:

### 1. ?? WPF Desktop UI (Recommended)
**Modern graphical interface with real-time agent tracking**

**To Run:**
```bash
# From solution root
dotnet run --project MultiAgentLearning.UI\MultiAgentLearning.UI.csproj
```

**Or in Visual Studio:**
1. Right-click `MultiAgentLearning.UI` project
2. Select "Set as Startup Project"
3. Press F5

**Features:**
- ? Visual agent status tracking
- ? Color-coded progress indicators
- ? Copy results to clipboard
- ? Clean, modern interface
- ? Real-time updates

**See:** [UI Quick Start Guide](docs/UI/QUICK_START_UI.md)

---

### 2. ?? Console Application
**Traditional command-line interface**

**To Run:**
```bash
# From solution root
dotnet run --project src\MultiAgentLearning.csproj
```

**Or in Visual Studio:**
1. Right-click `MultiAgentLearning` project
2. Select "Set as Startup Project"
3. Press F5

**Features:**
- ? Lightweight and fast
- ? Works over SSH/remote
- ? Script-friendly
- ? No GUI dependencies

---

## First-Time Setup

**Both interfaces require an API key:**

### Using User Secrets (Recommended)
```bash
# For UI
cd MultiAgentLearning.UI
dotnet user-secrets set "Gemini:ApiKey" "your-api-key"

# For Console
cd src
dotnet user-secrets set "Gemini:ApiKey" "your-api-key"
```

### Using Environment Variable
```powershell
# Windows PowerShell
$env:GEMINI_API_KEY = "your-api-key"

# Linux/Mac
export GEMINI_API_KEY="your-api-key"
```

---

## Quick Comparison

| Feature | WPF UI | Console |
|---------|--------|---------|
| **Visual Design** | ? Modern, colorful | ? Text-based |
| **Agent Tracking** | ? Real-time progress | ? Text output |
| **Copy Results** | ? One-click | ? Manual selection |
| **Multi-Query** | ? Easy clear/rerun | ? Loop prompt |
| **Resource Usage** | Medium | Low |
| **Remote Access** | ? Local only | ? SSH-friendly |
| **Learning Curve** | Easy | Easy |

---

## Recommended Usage

- **New Users**: Start with WPF UI for best experience
- **Learning**: Use WPF UI to visualize agent flow
- **Development**: Use Console for quick testing
- **Remote/Automation**: Use Console for scripts
- **Demos**: Use WPF UI for presentations

---

## Troubleshooting

### "API key not found"
Configure user secrets or environment variable (see First-Time Setup above)

### WPF UI won't start
- Ensure Windows desktop environment
- Check .NET 8.0 SDK installed
- Verify project reference paths

### Console won't start
- Check .NET 8.0 SDK installed
- Verify API key configuration

### Build errors
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

---

## More Information

- [UI Documentation](docs/UI/README.md)
- [Console Quick Start](docs/QUICK_START.md)
- [Getting Started Guide](docs/GETTING_STARTED.md)
- [Architecture Overview](docs/ARCHITECTURE.md)

---

**Need help?** Check the [main README](README.md) or open an issue!
