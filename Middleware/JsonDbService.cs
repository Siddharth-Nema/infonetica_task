using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class JsonDbService
{
    private readonly string workflowsPath = Path.Combine("db", "workflows.json");
    private readonly string instancesPath = Path.Combine("db", "instances.json");
    private readonly object lockObj = new();

    public List<Workflow> LoadWorkflows()
    {
        lock (lockObj)
        {
            if (!File.Exists(workflowsPath)) return new List<Workflow>();

            var json = File.ReadAllText(workflowsPath);
            return JsonSerializer.Deserialize<List<Workflow>>(json) ?? new List<Workflow>();
        }
    }

    public void SaveWorkflows(List<Workflow> workflows)
    {
        lock (lockObj)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(workflows, options);
            File.WriteAllText(workflowsPath, json);
        }
    }

    public List<Instance> LoadInstances()
    {
        lock (lockObj)
        {
            if (!File.Exists(instancesPath)) return new List<Instance>();

            var json = File.ReadAllText(instancesPath);
            return JsonSerializer.Deserialize<List<Instance>>(json) ?? new List<Instance>();
        }
    }

    public void SaveInstances(List<Instance> instances)
    {
        lock (lockObj)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(instances, options);
            File.WriteAllText(instancesPath, json);
        }
    }

    public async Task<List<Workflow>> LoadWorkflowsAsync()
    {
        using var stream = File.OpenRead(workflowsPath);
        return await JsonSerializer.DeserializeAsync<List<Workflow>>(stream) ?? new List<Workflow>();
    }

    public async Task SaveWorkflowsAsync(List<Workflow> workflows)
    {
        using var stream = File.Create(workflowsPath);
        await JsonSerializer.SerializeAsync(stream, workflows, new JsonSerializerOptions { WriteIndented = true });
    }

    public async Task<List<Instance>> LoadInstancesAsync()
    {
        using var stream = File.OpenRead(instancesPath);
        return await JsonSerializer.DeserializeAsync<List<Instance>>(stream) ?? new List<Instance>();
    }

    public async Task SaveInstancesAsync(List<Instance> instances)
    {
        using var stream = File.Create(instancesPath);
        await JsonSerializer.SerializeAsync(stream, instances, new JsonSerializerOptions { WriteIndented = true });
    }
}
