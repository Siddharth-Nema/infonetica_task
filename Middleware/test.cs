using System;
using System.Collections.Generic;

class Test
{
    static void Main()
    {
        var jsonDb = new JsonDbService();

        // Test workflows saving and loading
        var workflows = new List<Workflow>
        {
            new Workflow
            {
                id = 1,
                name = "Sample Workflow",
                states = new List<State>
                {
                    new State { id = 1, name = "Start", isInitial = true, isFinal = false, enabled = true },
                    new State { id = 2, name = "End", isInitial = false, isFinal = true, enabled = true }
                },
                actions = new List<Action>(),
                currentStateId = 1,
                history = new List<HistoryEntry>()
            }
        };

        jsonDb.SaveWorkflows(workflows);
        var loadedWorkflows = jsonDb.LoadWorkflows();

        Console.WriteLine($"Workflows loaded: {loadedWorkflows.Count}");
        Console.WriteLine($"First Workflow Name: {loadedWorkflows[0].name}");

        // Test instances saving and loading
        var instances = new List<Instance>
        {
            new Instance(loadedWorkflows[0])
            {
                id = 1,
                currentStateId = loadedWorkflows[0].initialStateId,
                history = new List<HistoryEntry>()
            }
        };

        jsonDb.SaveInstances(instances);
        var loadedInstances = jsonDb.LoadInstances();

        Console.WriteLine($"Instances loaded: {loadedInstances.Count}");
        Console.WriteLine($"First Instance current state Id: {loadedInstances[0].currentStateId}");
    }
}
