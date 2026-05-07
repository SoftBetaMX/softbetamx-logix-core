# SoftBetaMX Logix Core — Wiki

Welcome to the official wiki for **SoftBetaMX Logix Core**, a .NET library for communicating with Allen-Bradley MicroLogix PLCs using the [libplctag](https://github.com/libplctag/libplctag) protocol.

---

## Table of Contents

1. [Overview](#overview)
2. [Requirements](#requirements)
3. [Installation](#installation)
4. [Getting Started](#getting-started)
5. [API Reference](#api-reference)
   - [Constructor](#constructor)
   - [Property: Timeout](#property-timeout)
   - [readBinary](#readbinary)
   - [WriteBinary](#writebinary)
   - [WriteSingleBinary](#writesinglebinary)
   - [WriteOneShotBinary](#writeoneshotbinary)
   - [readSingleInteger (single)](#readSingleInteger-single)
   - [readSingleInteger (array)](#readSingleInteger-array)
   - [writeSingleInteger](#writesingleinteger)
   - [readFloat](#readfloat)
   - [readTimer](#readtimer)
   - [dataTimer Enum](#datatimer-enum)
6. [Supported Tag Types](#supported-tag-types)
7. [Usage Examples](#usage-examples)
8. [Error Handling](#error-handling)
9. [FAQ](#faq)
10. [Links & Support](#links--support)

---

## Overview

**SoftBetaMX Logix Core** (`SoftBetaMxLogix`) provides a simple, strongly-typed interface for reading and writing data on Allen-Bradley MicroLogix PLCs over Ethernet. It targets multiple .NET runtimes and is available as a NuGet package.

| Attribute      | Value                              |
|---------------|------------------------------------|
| Package ID    | `SoftBetaMxLogix`                  |
| Current Version | 1.1.8                            |
| Target Frameworks | .NET 6.0, .NET 8.0, .NET 10.0 |
| License       | MIT                                |
| PLC Protocol  | SLC/MicroLogix (via libplctag)     |

---

## Requirements

- **.NET 6.0, 8.0, or 10.0**
- Allen-Bradley **MicroLogix** PLC (SLC protocol)
- Network (TCP/IP) connectivity to the PLC

---

## Installation

### .NET CLI

```bash
dotnet add package SoftBetaMxLogix
```

### Package Manager Console (Visual Studio)

```powershell
Install-Package SoftBetaMxLogix
```

### PackageReference (csproj)

```xml
<PackageReference Include="SoftBetaMxLogix" Version="1.1.8" />
```

---

## Getting Started

```csharp
using SoftBetaMxLogix;

// Create a connection to the PLC at 192.168.0.100 with a 5-second timeout
var plc = new logix("192.168.0.100", 5000);

// Read a binary word from B3:0
bool[] bits = plc.readBinary("B3:0");
Console.WriteLine($"Bit 0: {bits[0]}");

// Read an integer from N7:0
int value = plc.readSingleInteger("N7:0");
Console.WriteLine($"N7:0 = {value}");

// Write an integer to N7:0
plc.writeSingleInteger("N7:0", 100);

// Read a timer accumulated value
int acc = plc.readTimer("T4:0", logix.dataTimer.ACC);
Console.WriteLine($"T4:0 ACC = {acc}");
```

---

## API Reference

### Constructor

```csharp
public logix(string ip, int timeout)
```

Creates a new PLC connection instance.

| Parameter | Type   | Description                                       |
|-----------|--------|---------------------------------------------------|
| `ip`      | string | IP address of the PLC (e.g., `"192.168.0.100"`)  |
| `timeout` | int    | Communication timeout in **milliseconds**         |

**Example:**

```csharp
var plc = new logix("192.168.0.100", 5000);
```

---

### Property: Timeout

```csharp
public int Timeout { get; set; }
```

Gets or sets the communication timeout in milliseconds after the instance has been created.

```csharp
plc.Timeout = 10000; // Change to 10-second timeout
```

---

### readBinary

```csharp
public bool[] readBinary(string tagName)
```

Reads a 16-bit binary word from a **B** (binary/bit) file and returns an array of 16 booleans.

| Parameter | Type   | Description                       |
|-----------|--------|-----------------------------------|
| `tagName` | string | Binary tag address (e.g., `"B3:0"`) |

**Returns:** `bool[16]` — index `0` is bit 0, index `15` is bit 15. Returns `null` on communication error.

```csharp
bool[] bits = plc.readBinary("B3:0");
// bits[0]  → bit 0
// bits[15] → bit 15
```

---

### WriteBinary

```csharp
public bool WriteBinary(string tagName, bool[] values)
```

Writes a full 16-bit binary word to a **B** file by providing an array of 16 booleans.

| Parameter | Type    | Description                         |
|-----------|---------|-------------------------------------|
| `tagName` | string  | Binary tag address (e.g., `"B3:0"`) |
| `values`  | bool[16]| Array of 16 bit values to write     |

**Returns:** `true` on success, `false` on error.

```csharp
bool[] newBits = new bool[16];
newBits[0] = true;   // Set bit 0 ON
newBits[1] = false;  // Set bit 1 OFF
bool ok = plc.WriteBinary("B3:0", newBits);
```

---

### WriteSingleBinary

```csharp
public bool WriteSingleBinary(string tagName, bool value)
```

Writes a single bit within a **B** file word without affecting the other bits.

| Parameter | Type   | Description                                  |
|-----------|--------|----------------------------------------------|
| `tagName` | string | Tag address with bit index (e.g., `"B3:0/0"`) |
| `value`   | bool   | `true` = ON, `false` = OFF                   |

**Returns:** `true` on success, `false` on error.

```csharp
plc.WriteSingleBinary("B3:0/0", true);   // Turn bit 0 ON
plc.WriteSingleBinary("B3:0/3", false);  // Turn bit 3 OFF
```

---

### WriteOneShotBinary

```csharp
public bool WriteOneShotBinary(string tagName)
```

Sends a momentary `true` pulse to a single bit, then immediately resets it to `false`. Useful for triggering one-shot events in the PLC ladder logic.

| Parameter | Type   | Description                                  |
|-----------|--------|----------------------------------------------|
| `tagName` | string | Tag address with bit index (e.g., `"B3:0/0"`) |

**Returns:** `true` on success, `false` on error.

```csharp
plc.WriteOneShotBinary("B3:0/0"); // Pulse bit 0 for one cycle
```

---

### readSingleInteger (single)

```csharp
public int readSingleInteger(string tagName)
```

Reads a single 16-bit signed integer from an **N** (integer) file.

| Parameter | Type   | Description                          |
|-----------|--------|--------------------------------------|
| `tagName` | string | Integer tag address (e.g., `"N7:0"`) |

**Returns:** `int` value (`-32768` to `32767`). Returns `0` on error.

```csharp
int speed = plc.readSingleInteger("N7:10");
```

---

### readSingleInteger (array)

```csharp
public int[] readSingleInteger(string tagName, int qty)
```

Reads multiple consecutive 16-bit integers starting at the given address. Maximum quantity is **20**.

| Parameter | Type   | Description                                           |
|-----------|--------|-------------------------------------------------------|
| `tagName` | string | Starting integer tag address (e.g., `"N7:0"`)        |
| `qty`     | int    | Number of consecutive registers to read (max 20)     |

**Returns:** `int[]` array of the requested length. Returns `null` on error.

```csharp
int[] data = plc.readSingleInteger("N7:0", 5);
// data[0] = N7:0, data[1] = N7:1, ..., data[4] = N7:4
```

---

### writeSingleInteger

```csharp
public bool writeSingleInteger(string tagName, int value)
```

Writes a single 16-bit signed integer to an **N** file.

| Parameter | Type   | Description                                        |
|-----------|--------|----------------------------------------------------|
| `tagName` | string | Integer tag address (e.g., `"N7:0"`)              |
| `value`   | int    | Value to write (`-32768` to `32767`)               |

**Returns:** `true` on success, `false` on error.

```csharp
plc.writeSingleInteger("N7:20", 150); // Set N7:20 = 150
```

---

### readFloat

```csharp
public float readFloat(string tagName)
```

Reads a 32-bit floating-point value from an **F** (float) file.

| Parameter | Type   | Description                         |
|-----------|--------|-------------------------------------|
| `tagName` | string | Float tag address (e.g., `"F8:0"`) |

**Returns:** `float` value. Returns `0` on error.

```csharp
float temperature = plc.readFloat("F8:0");
Console.WriteLine($"Temperature: {temperature:F2}°C");
```

---

### readTimer

```csharp
public int readTimer(string tagName, dataTimer data)
```

Reads the preset (PRE) or accumulated (ACC) value of a **T** (timer) file.

| Parameter | Type       | Description                                                  |
|-----------|------------|--------------------------------------------------------------|
| `tagName` | string     | Timer tag address (e.g., `"T4:0"`)                          |
| `data`    | dataTimer  | `logix.dataTimer.PRE` or `logix.dataTimer.ACC`              |

**Returns:** `int` timer value in milliseconds/tenths-of-a-second (depending on timer base). Returns the error code on failure.

```csharp
int preset     = plc.readTimer("T4:0", logix.dataTimer.PRE);
int accumulated = plc.readTimer("T4:0", logix.dataTimer.ACC);
Console.WriteLine($"T4:0 — PRE: {preset}, ACC: {accumulated}");
```

---

### dataTimer Enum

```csharp
public enum dataTimer : int
{
    PRE,  // Preset value
    ACC,  // Accumulated value
}
```

Used as the second argument to `readTimer` to select which timer word to read.

---

## Supported Tag Types

| File Letter | File Type        | Read Method            | Write Method           |
|-------------|------------------|------------------------|------------------------|
| `B`         | Binary (Bit)     | `readBinary`           | `WriteBinary`, `WriteSingleBinary`, `WriteOneShotBinary` |
| `N`         | Integer          | `readSingleInteger`    | `writeSingleInteger`   |
| `F`         | Float            | `readFloat`            | *(not yet available)*  |
| `T`         | Timer            | `readTimer`            | *(not yet available)*  |
| `C`         | Counter          | *(not yet available)*  | *(not yet available)*  |
| `O`         | Output           | *(not yet available)*  | *(not yet available)*  |
| `I`         | Input            | *(not yet available)*  | *(not yet available)*  |

---

## Usage Examples

### Monitor Multiple Tags in a Loop

```csharp
using SoftBetaMxLogix;

var plc = new logix("192.168.0.100", 5000);

while (true)
{
    int  temp  = plc.readSingleInteger("N7:10"); // Temperature sensor
    int  level = plc.readSingleInteger("N7:11"); // Level sensor
    bool[]  bits  = plc.readBinary("B3:0");
    bool motorOn  = bits[0];                     // Motor status bit 0

    Console.WriteLine($"Temp: {temp}°C | Level: {level}% | Motor: {(motorOn ? "ON" : "OFF")}");
    Thread.Sleep(1000);
}
```

### Control a Production Line

```csharp
using SoftBetaMxLogix;

var plc = new logix("192.168.0.100", 5000);

// Start production line
plc.WriteSingleBinary("B3:0/0", true);  // Conveyor motor ON
plc.WriteSingleBinary("B3:0/1", true);  // Indicator light ON
plc.writeSingleInteger("N7:20", 150);   // Set target speed

Console.WriteLine("Production line started");

// Poll completion flag (B3:5/0)
bool[] status;
do
{
    status = plc.readBinary("B3:5");
    int progress = plc.readSingleInteger("N7:21");
    Console.WriteLine($"Progress: {progress}%");
    Thread.Sleep(500);
} while (!status[0]);

// Stop production line
plc.WriteSingleBinary("B3:0/0", false);
plc.WriteSingleBinary("B3:0/1", false);
Console.WriteLine("Production line stopped");
```

### Timer Monitoring

```csharp
using SoftBetaMxLogix;

var plc = new logix("192.168.0.100", 5000);

int preset = plc.readTimer("T4:0", logix.dataTimer.PRE);
Console.WriteLine($"Cycle time preset: {preset} ms");

while (true)
{
    int elapsed    = plc.readTimer("T4:0", logix.dataTimer.ACC);
    int percentage = preset > 0 ? (int)((elapsed / (double)preset) * 100) : 0;

    Console.WriteLine($"Cycle progress: {percentage}% ({elapsed}/{preset} ms)");
    Thread.Sleep(100);
}
```

### Read Multiple Integer Registers

```csharp
using SoftBetaMxLogix;

var plc = new logix("192.168.0.100", 5000);

// Read N7:0 through N7:4 in a single call
int[] registers = plc.readSingleInteger("N7:0", 5);
for (int i = 0; i < registers.Length; i++)
{
    Console.WriteLine($"N7:{i} = {registers[i]}");
}
```

### Read Floating-Point Sensor

```csharp
using SoftBetaMxLogix;

var plc = new logix("192.168.0.100", 5000);

float pressure = plc.readFloat("F8:0");
Console.WriteLine($"Pressure: {pressure:F3} bar");
```

### One-Shot Trigger

```csharp
using SoftBetaMxLogix;

var plc = new logix("192.168.0.100", 5000);

// Send a momentary pulse to trigger a PLC rung
bool success = plc.WriteOneShotBinary("B3:1/5");
Console.WriteLine(success ? "Trigger sent" : "Trigger failed");
```

---

## Error Handling

All read methods return `null` or `0` on failure. All write methods return `false` on failure. Wrap calls in a try-catch for unexpected exceptions:

```csharp
try
{
    var plc = new logix("192.168.0.100", 5000);
    int value = plc.readSingleInteger("N7:0");

    if (value == 0)
        Console.WriteLine("Read returned 0 — may indicate a communication error");
}
catch (Exception ex)
{
    Console.WriteLine($"PLC communication error: {ex.Message}");
}
```

> **Tip:** Increase the `Timeout` value (e.g., `10000` ms) on slow or high-latency networks to reduce spurious errors.

---

## FAQ

**Q: Which PLC families are supported?**  
A: Currently only Allen-Bradley **MicroLogix** and **SLC 500** series (SLC protocol via libplctag). ControlLogix / CompactLogix (Ethernet/IP) is not supported at this time.

**Q: Can I read/write output (`O`) and input (`I`) files?**  
A: Direct read/write for `O` and `I` file types is not yet implemented in the public API. This is planned for a future release.

**Q: What is the maximum number of integers I can read at once?**  
A: `readSingleInteger(tagName, qty)` supports a maximum of **20** elements per call.

**Q: Why does `readBinary` return a `bool[16]` instead of a single value?**  
A: B-file words are 16-bit registers where each bit is individually addressable. The array gives direct access to every bit by index.

**Q: What .NET versions are supported?**  
A: .NET 6.0, .NET 8.0, and .NET 10.0. .NET Framework is not supported.

---

## Links & Support

| Resource          | URL                                                                 |
|-------------------|---------------------------------------------------------------------|
| GitHub Repository | https://github.com/SoftBetaMX/softbetamx-logix-core               |
| NuGet Package     | https://www.nuget.org/packages/SoftBetaMxLogix/                    |
| libplctag         | https://github.com/libplctag/libplctag                             |
| Issues / Bugs     | https://github.com/SoftBetaMX/softbetamx-logix-core/issues        |
| Email Support     | soporte@softbetamx.com                                             |

---

*Made with ❤️ by [SoftBetaMX](https://github.com/SoftBetaMX) — Copyright © 2026 SoftBetaMX. All rights reserved.*
