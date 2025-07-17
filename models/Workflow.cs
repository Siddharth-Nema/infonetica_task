using System;
using System.Collections.Generic;
using System.Linq;

public class Workflow
{
    public required int id { get; set; }
    public required string name { get; set; }
    public List<State> states { get; set; } = new List<State>();
    public List<Action> actions { get; set; } = new List<Action>();
    public int currentStateId { get; set; }
    public int initialStateId { get; private set; }
    public List<HistoryEntry> history { get; set; } = new List<HistoryEntry>();

    // Constructor
    public Workflow()
    {
        ValidateInitialState();
    }

    // You might want to call this explicitly when states are set/updated
    private void ValidateInitialState()
    {
        var initialStates = states.Where(s => s.isInitial && s.enabled).ToList();

        if (initialStates.Count != 1)
        {
            throw new InvalidOperationException("There must be exactly one enabled initial state in the workflow.");
        }

        initialStateId = initialStates[0].id;
    }

    // Optional: Method to set states and re-validate
    public void SetStates(List<State> newStates)
    {
        states = newStates ?? new List<State>();
        ValidateInitialState();
    }
}
