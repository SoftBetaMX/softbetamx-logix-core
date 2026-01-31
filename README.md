# SoftBetaMX Logix Core

**A powerful .NET library for communicating with Allen-Bradley MicroLogix PLCs**

[![NuGet Version](https://img.shields.io/nuget/v/SoftBetaMxLogix.svg)](https://www.nuget.org/packages/SoftBetaMxLogix/)
[![.NET](https://img.shields.io/badge/.NET-6.0-512BD4)](https://dotnet.microsoft.com/)

## Installation

```bash
dotnet add package SoftBetaMxLogix
```

## Quick Start

```csharp
using SoftBetaMxLogix;

// Connect to PLC
var plc = new logix("192.168.0.100", 5000);

// Read data
string value = plc.read("N7:0");

// Write data
plc.write("B3:0", "1");
```

## Features

- Easy PLC connection with IP address and timeout
- Read and write PLC tags (N, B, T, C, O, I file types)
- Timer support (PRE and ACC values)
- Built on reliable libplctag library
- Full XML documentation

## License

MIT License - see LICENSE.txt for details
