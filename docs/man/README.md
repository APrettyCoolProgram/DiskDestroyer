<!--
  README.md template for manual.
-->

<!-- [WARNING] =========================================================
* Warning
---------------------------------------------------------------------------- -->

> [!WARNING]  
> This is a warning everyone should see, or remove this section entirely.

<!--
This divider separates the this section from the rest of the README. If you are
not using the this section, comment this divider out.
--->
---

<!-- ============================================================ [ WARNING] -->

<!-- [INTRO] ===========================================================
* Project logo
  There are references for both a "light" and "dark" images. The dark image
  should have a background of HEX #0d1117, to match the dark mode of GitHub.
  The light image is the fallback.
* Project title
* Project badges
---------------------------------------------------------------------------- -->

<div align="center">

  <picture>
    <source media="(prefers-color-scheme: dark)" srcset=".github/logo/dark/repository-logo.jpg">
    <source media="(prefers-color-scheme: light)" srcset=".github/logo/light/repository-logo.jpg">
    <img alt="Fallback image description" src=".github/logo/light/repository-logo.jpg">
  </picture>

  <h1>%ProjectName Manual</h1>

  ![RELEASE](https://img.shields.io/badge/Release\/Version-25.0.0.0-teal)&nbsp;

</div>

---

<!-- ============================================================= [ INTRO ] -->

<!-- [TABLE OF CONTENTS] =======================================================
* The Table of Contents
  The Table of Contents.
---------------------------------------------------------------------------- -->

### CONTENTS

* [About %ProjectName%](#about)<br>
    * [Features](#features)<br>
    * [What's New](#whats-new)<br>
    * [Built With](#built-with)<br>
* [How It Works](#how-it-works)<br>
* [Getting Started](#getting-started)<br>
    * [Before you begin](#before-you-begin)<br>
    * [Requirements](#requirements)<br>
* [Installing](#installing)<br>
    * [Windows](#windows)<br>
    * [MacOS](#macos)<br>
    * [Linux](#linux)<br>
    * [Other Operating Systems](#other-operating-systems)<br>
* [Setup](#setup)<br>
* [Configuration](#configuration)<br>
    * [Required settings](#required-settings)<br>
    * [Recommended settings](#recommended-settings)<br>
    * [Optional settings](#optional-settings)<br>
* [Additional Setup](#additional-setup)<br>
* [Using](#using)<br>

---

<!-- =================================================== [TABLE OF CONTENTS] -->

<!-- [REPOSITORY README CONTENTS] ==============================================
This section contains the following components of the repository README:
* About
    * Features
    * What's New
    * Built With
* How It Works
=============================================== [REPOSITORY README CONTENTS] -->

<!-- [GETTING STARTED] =========================================================
* Before you begin
  Any prerequisites, assumptions, or other information a user should know before
  getting started.
* Prerequisites
  List of software, hardware, or other requirements.

  If this section is only comprised of prerequisites, it can be merged with the
  About section.
============================================================================ -->

## Getting Started

A quick overview of how to get started with the project.

### Before you begin

Any assumptions, or other information a user should know before

### Requirements

| Requirement | Minimum version | Notes |
|-------------|-----------------|-------|
| [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) | 10.0 | Required to build and run. |
| Requirement | | |
| Requirement | | |

<!-- ===================================================== [GETTING STARTED] -->

<!-- [INSTALLING] ==============================================================
* Windows
* MacOS
* Linux
* Other Operating Systems
============================================================================ -->

## Installing

A quick overview of how to install the project.

### Windows

Instructions for installing on Windows.

### MacOS

Instructions for installing on MacOS.

### Linux

Instructions for installing on Linux.

### Other Operating Systems

Instructions for installing on other operating systems.

<!-- ========================================================= [INSTALLING] -->

<!-- [SETUP] ==============================================================
* Setup
  Instructions for setting up the project after installation, generally
  related to the initial execution of the project.
============================================================================ -->

## Setup

Brief description of the setup process.

<!-- ========================================================= [SETUP] -->

<!-- [CONFIGURATION] ========================================================
* Configuration
  Instructions for configuring the project after installation, generally
  related to the initial execution of the project.
============================================================================ -->

## Configuration

Configuration file location: `path/to/config.file`

### Required settings

| Setting | Description | Default |
|---------|-------------|---------|
| `SettingName` | What this setting controls. | `default` |

### Recommended settings

| Setting | Description | Default |
|---------|-------------|---------|
| `SettingName` | What this setting controls. | `default` |

### Optional settings

| Setting | Description | Default |
|---------|-------------|---------|
| `SettingName` | What this setting controls. | `default` |

<!-- ========================================================= [CONFIGURATION] -->

<!-- [ADDITIONAL SETUP] ========================================================
* Additional Setup
  Instructions for additional setup steps after installation, generally
  related to the initial execution of the project.
============================================================================ -->

## Additional Setup

<!-- ========================================================= [ADDITIONAL SETUP] -->

<!-- [USAGE] ========================================================
* Usage
  Instructions for using the project after installation and setup.
============================================================================ -->

## Using

<!-- ========================================================= [USAGE] -->

<!-- [UPDATING] ================================================================
  Optional. Instructions for upgrading to a newer version.
============================================================================ -->

## Updating

Instructions for updating from a previous version.

1. Step one.
2. Step two.
3. Step three.

<!-- ============================================================= [UPDATING] -->

<!-- [UNINSTALLING] ============================================================
  Optional. Instructions for removing the software cleanly.
============================================================================ -->

# Uninstalling

Instructions for uninstalling.

1. Step one.
2. Step two.

<!-- ========================================================= [UNINSTALLING] -->

<!-- [BUILDING FROM SOURCE] ====================================================
  Required for source code repositories. Cover restore, build, and deploy.
============================================================================ -->

# Building from source

## Restore dependencies

```shell
dotnet restore
```

## Build

```shell
dotnet build --configuration Release
```

## Run

```shell
dotnet run --project src/%ProjectName%.csproj
```

## Publish / deploy

```shell
dotnet publish --configuration Release --output ./publish
```

<!-- ================================================= [BUILDING FROM SOURCE] -->

<!-- [TESTING] =================================================================
  Optional. Describe how to run the test suite.
============================================================================ -->

# Testing

## Running tests

```shell
dotnet test
```

## Test coverage

Brief description of what is covered and what is not.

***

<!-- ============================================================== [TESTING] -->

<!-- [API] =====================================================================
  Optional. Document the public API or link to generated API documentation.
  Remove this section if the project has no public API.
============================================================================ -->

# API

Brief description of the API.

Full API reference: [docs/api/README.md](docs/api/README.md)

***