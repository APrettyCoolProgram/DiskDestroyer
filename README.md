<div align="center">

  <img src=".github/logo/RepositoryTemplate-672x440.png" alt="Repository Template">

  <br/>

  ![RELEASE](https://img.shields.io/badge/release-September_2026-teal)&nbsp;&nbsp;
  ![LICENSE](https://img.shields.io/badge/license-Apache%202.0-blue)

# Repository-Template

</div>

| CONTENTS                                                        |
|:----------------------------------------------------------------|
| [About this repository](#about-this-repository)                 |
| [What this repository includes](#what-this-repository-includes) |
| [Using the template](#using-the-template)                       |
| [Folder structure](#folder-structure)                           |
| [Supporting documentation](#supporting-documentation)           |

***

# About this repository

This is a GitHub repository template, which provides a consistent starting structure for new repositories.

## What this repository includes

* A [.github/](.github) folder for repository resources.
* A [docs/](docs) folder for repository documentation.
* A [project/](project) folder for [*project repositories*](#project-repositories).
* A [src/](src) folder for [*source code repositories*](#source-code-repositories).
* A `.gitignore` file.
* An `AGENTS.md` file.
* A `LICENSE` file.
* The repository's main `README.md`, which will be replaced by:
  * The [Collection-README.md](Collection-README.md) for collection-style repositories.
  * The [Project-README.md](Project-README.md) for project-style repositories.
  * The [SourceCode-README.md](SourceCode-README.md) for software or source-code repositories.

# Using the template

## Collection repositories

A ***collection repository*** is a repository that primarily collects links, notes, or documentation and **does not contain** source code.

[The Documentation Project](https://github.com/APrettyCoolProgram/TheDocumentationProject) is an example of a collection repository.

To create a collection repository:

1. Create a new repository using this template.
2. Remove the following files/folders:
  * `src/`
  * `projects/`
  * `README.md` (this file!)
  * `SourceCode-README.md`
  * `Projects-README.md`
3. Rename `Collection-README.md` to `README.md`.
4. Modify `README.md` to reflect the content and purpose of the repository.
5. Review the files in [docs/](docs) and remove any folders/documents the new repository does not need.
6. Replace %RepositoryName% with the name of the new repository in all relevant files.
7. Add the `.do-not-commit/` folder to `.github/` (optional, but helpful)

## Project repositories

A ***project repository*** is a repository that contains multiple *projects*, which may include source code, documentation, and other assets.

The difference between a **project repository** and a **source code repository** is that a **project repository** contains projects that are loosely related - or not related at all - whereas a **source code repository** primarily focuses on a single codebase.

The [Experiments](https://github.com/APrettyCoolProgram/Experiments) repository is an example of a project repository.

To create a project repository:

1. Create a new repository using this template.
2. Remove the following files/folders:
  * `src/`
  * `README.md` (this file!)
  * `Collection-README.md`
  * `SourceCode-README.md`
3. Rename `Projects-README.md` to `README.md`.
4. Modify `README.md` to reflect the content and purpose of the repository.
5. Review the files in [docs/](docs) and remove any folders/documents the new repository does not need.
6. Replace %RepositoryName% with the name of the new repository in all relevant files.
7. Add the `.do-not-commit/` folder to `.github/` (optional, but helpful)

Each project within the repository should have its own dedicated folder under [projects/](projects).

## Source code repositories

A ***source code repository*** is a repository that primarily contains buildable software or other source code.

To create a source code repository:

1. Create a new repository using this template.
2. Remove the following files/folders:
  * `projects/`
  * `README.md` (this file!)
  * `Collection-README.md`
  * `Projects-README.md`
3. Rename `SourceCode-README.md` to `README.md`.
4. Modify `README.md` to reflect the content and purpose of the repository.
5. Review the files in [docs/](docs) and remove any folders/documents the new repository does not need.
6. Replace %RepositoryName% with the name of the new repository in all relevant files.
7. Add the `.do-not-commit/` folder to `.github/` (optional, but helpful)

# Folder structure

Each of these folders contains a `README.md` file that provides information about the contents and purpose of the folder.

## The `./` (root) folder

All repositories have the following root folder structure:

```text
.
|-- .github/      Repository resources
|-- docs/         Repository documentation
|-- .gitignore    The repository .gitignore
|-- LICENSE       The repository license
|-- README.md     The repository README
```

*Project collections* also include a `projects/` folder to organize individual projects.

```text
.
|-- projects/       Projects
|   |-- ProjectA/   Project A
|   |-- ProjectB/   Project B
```

*Source code repositories* also include a `src/` folder to organize the source code.

```text
.
|-- src/          Source code
```

## The `./.github/` folder

All repositories include a `./.github/` folder for repository-specific resources.

```text
.
|-- .github/
|   |-- .do-not-commit/   Data/resources that should not be committed
|   |-- agents/           AGENT files
|   |-- archive/          Archived data
|   |-- development/      Development resources
|   |-- logo/             Logos
|   |-- readme/           Repository README.md resources
|   |-- third-party/      Third-party resources
|   |-- workspace/        Workspace resources
```

### The `./.github/development/` folder

The `./.github/development/` folder contains resources related to the development process, including old source files, scratch files, templates, working files, and various development documents.

```text
.
|-- .github/
|   |-- development/
|   |   |-- old-src/                      Old source files
|   |   |-- scratch/                      Scratch files
|   |   |-- template/                     Template files
|   |   |-- working/                      Working files 
|   |   |-- DesignDocument.md             Design document
|   |   |-- Development-KnownIssues.md    Known issues in development
|   |   |-- Development-Notes.md          Development notes
|   |   |-- Development-Roadmap.md        Development roadmap
|   |   |-- ScratchPad.md                 Scratch pad for development notes
```

## The `./docs/` folder

All repositories include a `./docs/` folder for documentation and supporting materials.

```text
.
|-- docs/
|   |-- api/                  API documentation
|   |-- man/                  Manual
|   |-- release-notes/        Release notes (detailed version history)
|   |-- development/          Development documentation
|   |-- CHANGELOG.md          Changelog (basic version history)
|   |-- CODEOWNERS            Code ownership assignments
|   |-- CONTRIBUTORS.md       Repository contributors
|   |-- FAQ.md                Frequently asked questions
|   |-- KNOWN-ISSUES.md       Known issues and workarounds
|   |-- ROADMAP.md            Planned work and future direction
|   |-- SECURITY.md           Security policy and vulnerability reporting guidance
|   |-- SUPPORT.md            How to get help
|   |-- TROUBLESHOOTING.md    Common problems and resolution steps
```

### The `./docs/api/` folder

The `./docs/api/` folder contains API documentation for the repository.

```text
|-- docs/
|   |-- api/
```

### The `./docs/man/` folder

The `./docs/man/` folder contains the manual for the repository.

```text
|-- docs/
|   |-- man/
```

### The `./docs/release-notes/` folder

The `./docs/release-notes/` folder contains detailed version history for the repository.

```text
|-- docs/
|   |-- release-notes/
```

### The `./docs/development/` folder

The `./docs/development/` folder contains development documentation for the repository.

```text
|-- docs/
|   |-- development/
```












