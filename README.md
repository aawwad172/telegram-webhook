# API Template (.NET 8)

## Overview

This template provides a clean architecture for building robust .NET 8 Web APIs. It includes:

* Layered solution structure (Domain, Application, Infrastructure, WebAPI)
* Dependency injection setup
* Exception handling middleware
* Health checks for SQL Server
* Automated code formatting and quality checks using [Husky.Net](https://github.com/alirezanet/Husky.Net)

## Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Git](https://git-scm.com/)
* Bash (for Unix) or CMD (for Windows) for Husky.Net hooks

### Setup Instructions

1. **Clone the repository**

   ```bash
   git clone <your-repo-url> \
   cd <repo-folder>
   ```

2. **Restore NuGet packages**

   ```bash
   dotnet restore
   ```

3. **Update the connection string**

   * Open `API.Template.WebAPI/appsettings.json` and set your SQL Server connection string under `ConnectionStrings:DbConnectionString`.

4. **(Optional) Rename the template placeholders**
   If you want to replace all `Template` identifiers with your own project name, run the renaming script:

   ```bash
   # Ensure the script has UNIX line endings and is executable
   sed -i 's/\r$//' scripts/rename_template.sh
   chmod +x scripts/rename_template.sh

   # Run the script, passing your new project name in quotes
   ./scripts/rename_template.sh "Your.Project.Name"
   ```

5. **Build the solution**

   ```bash
   dotnet build
   ```

6. **Run the application**

   ```bash
   dotnet run --project API.Template.WebAPI
   ```

7. **Access Swagger UI**

   * Open your browser to `https://localhost:<port>/swagger`.

## Husky.Net Setup & Usage

Husky.Net automates code quality checks (formatting, build, etc.) on Git hooks.

### Install Husky.Net as a dotnet tool

```bash
# 1) create a local tool manifest (once per repo)
dotnet new tool-manifest

# 2) install the Husky tool (note: package ID is Husky)
dotnet tool install Husky

# 3) initialize hooks
dotnet husky install
```

### Add or modify tasks

Tasks live in `.husky/task-runner.json`. Example groups:

* `pre-commit`: `dotnet-format`
* `pre-push`: `dotnet-clean`, `dotnet build --warnaserror`

### Configure Git hooks

**Example: `.husky/pre-commit`**

```bash
#!/bin/sh
. "$(dirname "$0")/_/husky.sh"

dotnet husky run --group pre-commit
```

### Troubleshooting

* Make sure hooks are executable:

  ```bash
  chmod +x .husky/*
  ```

## Customization

* Edit `.husky/task-runner.json` to add/remove tasks.
* Add new hooks:

  ```bash
  dotnet husky add <hook-name> -c "dotnet husky run --group <group-name>"
  ```

## License

MIT
