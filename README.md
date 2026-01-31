<div align="center">

# 🏭 SoftBetaMX Logix Core

**A powerful .NET library for communicating with Allen-Bradley MicroLogix PLCs**

[![NuGet Version](https://img.shields.io/nuget/v/SoftBetaMxLogix.svg?style=flat-square)](https://www.nuget.org/packages/SoftBetaMxLogix/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/SoftBetaMxLogix.svg?style=flat-square)](https://www.nuget.org/packages/SoftBetaMxLogix/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-6.0-512BD4?style=flat-square)](https://dotnet.microsoft.com/)

*Simplify industrial automation with easy-to-use PLC communication in .NET*

[Installation](#-installation) • [Features](#-features) • [Quick Start](#-quick-start) • [Examples](#-usage-examples) • [Documentation](#-api-reference)

</div>

---

## 🎯 Why SoftBetaMX Logix Core?

### The Problem
Industrial automation often requires:
- ❌ Complex PLC communication setup
- ❌ Deep knowledge of industrial protocols
- ❌ Manual tag management and data conversion
- ❌ Handling connection timeouts and errors
- ❌ Different implementations for each PLC model

### The Solution
```csharp
var plc = new logix("192.168.0.100", 5000);
string value = plc.read("N7:0");
plc.write("B3:0", "1");
```

✅ **Simple API. Reliable communication. Production-ready.**

---

## 📦 Installation

### Using .NET CLI
```bash
dotnet add package SoftBetaMxLogix
```

### Using Package Manager Console
```powershell
Install-Package SoftBetaMxLogix
```

### Using PackageReference (manual)
```xml
<PackageReference Include="SoftBetaMxLogix" Version="1.1.2" />
```

### Requirements
- ✅ .NET 6.0 or higher
- ✅ Allen-Bradley MicroLogix PLC
- ✅ Network connectivity to PLC

---

## ✨ Features

### 🔌 Easy PLC Connection
- **Simple IP-based connection** - Just provide IP address and timeout
- **Automatic protocol handling** - Built on reliable libplctag library
- **Connection management** - Handles timeouts and reconnections

### 📖 Read/Write Operations
- **Read PLC tags** - Integer, Boolean, Timer, Counter values
- **Write PLC tags** - Update outputs, bits, and data registers
- **Multiple data types** - Supports N, B, T, C, O, I file types
- **Type-safe operations** - Automatic data type handling

### ⏱️ Timer Support
- **Read timer values** - Access PRE (preset) and ACC (accumulated) values
- **Timer management** - Easy timer monitoring and control

### 🛡️ Robust & Reliable
- **Error handling** - Built-in timeout and exception management
- **Production-tested** - Used in real industrial environments
- **XML documentation** - Full IntelliSense support

---

## 🚀 Quick Start

### 1️⃣ Create PLC Connection

```csharp
using SoftBetaMxLogix;

// Connect to PLC at 192.168.0.100 with 5 second timeout
var plc = new logix("192.168.0.100", 5000);
```

### 2️⃣ Read Data from PLC

```csharp
// Read integer from N7:0
string intValue = plc.read("N7:0");
Console.WriteLine($"N7:0 value: {intValue}");

// Read bit from B3:0
string bitValue = plc.read("B3:0");
Console.WriteLine($"B3:0 value: {bitValue}");
```

### 3️⃣ Write Data to PLC

```csharp
// Write to output O:0/0
plc.write("O:0/0", "1");  // Turn ON

// Write to bit B3:0
plc.write("B3:0", "0");  // Turn OFF

// Write to integer N7:0
plc.write("N7:0", "100");
```

### 4️⃣ Work with Timers

```csharp
// Read timer preset value
string timerPre = plc.readTimer("T4:0", logix.dataTimer.PRE);
Console.WriteLine($"Timer T4:0 PRE: {timerPre}");

// Read timer accumulated value
string timerAcc = plc.readTimer("T4:0", logix.dataTimer.ACC);
Console.WriteLine($"Timer T4:0 ACC: {timerAcc}");
```

---

## 💡 Usage Examples

### Example 1: Monitor Multiple Tags

```csharp
using SoftBetaMxLogix;

var plc = new logix("192.168.0.100", 5000);

while (true)
{
    string temp = plc.read("N7:10");  // Temperature sensor
    string level = plc.read("N7:11"); // Level sensor
    string motor = plc.read("B3:0");  // Motor status
    
    Console.WriteLine($"Temp: {temp}°C, Level: {level}%, Motor: {(motor == "1" ? "ON" : "OFF")}");
    
    Thread.Sleep(1000);
}
```

### Example 2: Control System

```csharp
using SoftBetaMxLogix;

var plc = new logix("192.168.0.100", 5000);

// Start production line
plc.write("O:0/0", "1");  // Conveyor motor
plc.write("O:0/1", "1");  // Indicator light
plc.write("N7:20", "150"); // Set target speed

Console.WriteLine("Production line started");

// Monitor until complete
while (plc.read("B3:5") != "1")  // Wait for complete signal
{
    string progress = plc.read("N7:21");
    Console.WriteLine($"Progress: {progress}%");
    Thread.Sleep(500);
}

// Stop production line
plc.write("O:0/0", "0");
plc.write("O:0/1", "0");
Console.WriteLine("Production line stopped");
```

### Example 3: Timer Monitoring

```csharp
using SoftBetaMxLogix;

var plc = new logix("192.168.0.100", 5000);

// Monitor cycle timer
string preset = plc.readTimer("T4:0", logix.dataTimer.PRE);
Console.WriteLine($"Cycle time set to: {preset} seconds");

while (true)
{
    string elapsed = plc.readTimer("T4:0", logix.dataTimer.ACC);
    int percentage = (int)((double.Parse(elapsed) / double.Parse(preset)) * 100);
    
    Console.WriteLine($"Cycle progress: {percentage}% ({elapsed}/{preset}s)");
    
    Thread.Sleep(100);
}
```

---

## 📚 API Reference

### Constructor

```csharp
public logix(string ip, int timeout)
```

**Parameters:**
- ip - IP address of the PLC (e.g., "192.168.0.100")
- 	imeout - Connection timeout in milliseconds (default: 5000)

### Methods

#### Read Tag

```csharp
public string read(string tagName)
```

Reads a value from the specified PLC tag.

**Supported tag types:**
- N7:x - Integer files
- B3:x - Binary/Bit files
- T4:x - Timer files
- C5:x - Counter files
- O:x/x - Output files
- I:x/x - Input files

#### Write Tag

```csharp
public void write(string tagName, string value)
```

Writes a value to the specified PLC tag.

**Parameters:**
- 	agName - The PLC tag address
- alue - The value to write (as string)

#### Read Timer

```csharp
public string readTimer(string tagName, dataTimer dataType)
```

Reads timer preset (PRE) or accumulated (ACC) values.

**Parameters:**
- 	agName - Timer tag address (e.g., "T4:0")
- dataType - logix.dataTimer.PRE or logix.dataTimer.ACC

---

## 🔧 Configuration

### Timeout Settings

```csharp
var plc = new logix("192.168.0.100", 5000);  // 5 second timeout

// Or modify timeout after creation
plc.Timeout = 10000;  // 10 second timeout
```

### Error Handling

```csharp
try
{
    var plc = new logix("192.168.0.100", 5000);
    string value = plc.read("N7:0");
}
catch (Exception ex)
{
    Console.WriteLine($"PLC communication error: {ex.Message}");
}
```

---

## 🤝 Contributing

### Report Issues
[Create Issue](https://github.com/SoftBetaMX/softbetamx-logix-core/issues/new)

### Submit Changes
1. Fork the repository
2. Create a branch: `git checkout -b feature/improvement`
3. Make your changes
4. Commit: `git commit -m "Add improvement"`
5. Push: `git push origin feature/improvement`
6. Open a Pull Request

---

## 📋 Changelog

### v1.1.2 (Current)
- ✅ Added automated CI/CD with GitHub Actions
- ✅ Included DLL dependencies in repository
- 📝 Comprehensive README documentation

### v1.1.1
- 🔧 Updated to .NET 6.0
- 📝 Added XML documentation

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

---

## 🔗 Useful Links

- [GitHub Repository](https://github.com/SoftBetaMX/softbetamx-logix-core)
- [NuGet Package](https://www.nuget.org/packages/SoftBetaMxLogix/)
- [libplctag Library](https://github.com/libplctag/libplctag)
- [Allen-Bradley PLC Documentation](https://www.rockwellautomation.com/)

---

## 💬 Support

Need help? Contact us:
- 📧 Email: soporte@softbetamx.com
- 🌐 Web: [softbetamx.com](https://softbetamx.com)
- 🐛 Issues: [GitHub Issues](https://github.com/SoftBetaMX/softbetamx-logix-core/issues)

---

<div align="center">

**Made with ❤️ by [SoftBetaMX](https://github.com/SoftBetaMX)**

⭐ If you find this library useful, consider giving it a star on GitHub

© 2026 SoftBetaMX. All rights reserved.

</div>
