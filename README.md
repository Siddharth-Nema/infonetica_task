## Features

- Create and retrieve workflows with custom states and actions
- Create workflow instances and change their states via defined actions
- Track history of actions performed on each instance
- Data is persisted in JSON files (`db/workflows.json`, `db/instances.json`)

## Project Structure

- `Program.cs` – Application entry point and configuration
- `Middleware/JsonDbService.cs` – Service for reading/writing JSON data
- `models` – Data models: `Workflow`, `State`, `Action`, `Instance`, `HistoryEntry`
- `routes` – API controllers: `WorkflowsController`, `InstancesController`
- `db` – JSON files for data storage: workflows.json, instances.json

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Build and Run

```sh
dotnet build
dotnet run
```

## Data Model

See the models in `models` for details on properties.

## Notes

- Data is stored in JSON files in the `db` directory.
- No authentication or authorization is implemented.
