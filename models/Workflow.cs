using System;
using System.Collections.Generic;
using System.Linq;

public class Workflow
{
    public int id { get; set; }
    public string name { get; set; }
    public List<State> states { get; set; } = new List<State>();
    public List<Action> actions { get; set; } = new List<Action>();
    public int initialStateId { get; set; }
    public List<HistoryEntry> history { get; set; } = new List<HistoryEntry>();

    public void SetStates(List<State> newStates)
    {
        states = newStates ?? new List<State>();
    }
}
