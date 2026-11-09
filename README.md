<div align="center">

  <img src=".github/logo/DiskDestroyer-256x256.png" alt="DiskDestroyer">

  <br/>
  <br/>

  ![RELEASE](https://img.shields.io/badge/Version-1.0-teal)&nbsp;
  ![LICENSE](https://img.shields.io/badge/License-apache-blue)&nbsp;
  ![Platform](https://img.shields.io/badge/Platform-windows-lightgrey)&nbsp;
  ![.NET](https://img.shields.io/badge/.NET-10.0+-blueviolet)&nbsp;

</div>

***

<h6 align="center">

  [MANUAL](docs/man/README.md)&nbsp;&bull;&nbsp;[CHANGELOG](docs/CHANGELOG.md)&nbsp;&bull;&nbsp;[ROADMAP](docs/ROADMAP.md)&nbsp;&bull;&nbsp;[KNOWN ISSUES](docs/KNOWN-ISSUES.md)

</h6>

***

| CONTENTS                                      |
|-----------------------------------------------|
| [About Disk Destroyer](#about-disk-destroyer) |
| [How it works](#how-it-works)                 |
| [Getting started](#getting-started)           |
| [Installing](#installing)                     |
| [Usage](#usage)                               |
| [Acknowledgements](#acknowledgements)         |
| [Related projects](#related-projects)         |
| [License](#license)                           |

***

> [!WARNING]  
> **Disk Destroyer**<sup>1</sup> will *permanently* delete any data it overwrites!

## About Disk Destroyer

**Disk Destroyer**<sup>1</sup> is a utility that writes random data to a drive's free space.

## How it works

**Disk Destroyer**<sup>1</sup> creates (or appends) random data to a file named `disk_destroyer.dat` until the drive is full.

### I'll do you one better: Why it works?

When you "delete" a file, only the *reference* to that file is erased, not the *file* itself.

The file still exists and is marked as *free space*, and will remain as such until it's location is overwritten with new data. And until it's overwritten, data recovery software can recover the file.

To ensure the file is permanently deleted, you need to write random patterns of data over the file's location.

### Requirements

* [.NET 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

**Disk Destroyer**<sup>1</sup> has only been tested on Windows.

## Installing

**Disk Destroyer**<sup>1</sup> is a portable application, and doesn't need to be installed.

All you need to do is:

1. Download the latest [release](https://github.com/APrettyCoolProgram/DiskDestroyer/releases)
2. Extract the downloaded file to the root of the drive you want to destroy<sup>1</sup>

## Usage

**Disk Destroyer**<sup>1</sup> is run from a command prompt.

### Syntax

**Disk Destroyer**<sup>1</sup> has the following syntax:

`DiskDestroyer [--chunk-size]`

### Default execution

If you don't specifiy a chunk-size, the default is 1MB.

To run **Disk Destroyer**<sup>1</sup>:

1. Open a command prompt
2. Navigate to the root of the drive you want to destroy<sup>1</sup>
3. Type `DiskDestroyer`

The program will display a warning message and require confirmation before proceeding.

**Disk Destroyer**<sup>1</sup> progress will be shown in the console.

> [!NOTE]
> Depending on the size and type of disk (e.g., HDD, SSD), **Disk Destroyer**<sup>1</sup> may take a long time to complete.

### Custom Chunk Size

The default chunk size is 1MB.

To specify a custom chunk size in megabytes, simply add that to the `DiskDestroyer` command, like this:

| Command             | Chunk-size  |
|---------------------|:-----------:|
| `DiskDestroyer`     | 1MB         |
| `DiskDestroyer -8`  | 8MB         |
| `DiskDestroyer -16` | 16MB        |


## Documentation

* [Manual](docs/man/README.md) — Comprehensive user manual
* [API Reference](docs/api/README.md) — Complete API documentation
* [Development Guide](docs/DEVELOPMENT.md) — For contributors and developers
* [FAQ](docs/FAQ.md) — Frequently asked questions
* [Support](docs/SUPPORT.md) — Getting help and support options

## Acknowledgements

None.

## Built with

* [.NET 10+](https://dotnet.microsoft.com/) - Modern C# runtime and libraries
* [`System.IO`](https://learn.microsoft.com/en-us/dotnet/api/system.io) - File I/O operations
* [`DriveInfo`](https://learn.microsoft.com/en-us/dotnet/api/system.io.drivedriveinfo) - Disk space information

## Related projects

None.

## License

Copyright © 2026 [A Pretty Cool Program](https://github.com/aprettycoolprogram)  
Distributed under the [Apache 2.0 License](LICENSE)

***

<h6 align="center">

  [FAQ](docs/FAQ.md)&nbsp;&bull;&nbsp;[DEVELOPMENT](docs/DEVELOPMENT.md)&nbsp;&bull;&nbsp;[API](docs/api/README.md)&nbsp;&bull;&nbsp;[SUPPORT](docs/SUPPORT.md)&nbsp;&bull;&nbsp;[NOTICES](docs/NOTICES.md)

</h6>

<sup>**1**</sup> Disk Destroy *probably* won't *actually* destroy disks.
