using System;
using System.Collections.Generic;

public class Instance
{
    private static int nextId = 1;

    public int id { get; set; }

    public Workflow workflow { get; set; }

    public int currentStateId { get; set; }

    public List<HistoryEntry> history { get; set; } = new List<HistoryEntry>();

    public Instance(Workflow workflow)
    {
        if (workflow == null)
            throw new ArgumentNullException(nameof(workflow));

        this.id = nextId++; 
        this.workflow = workflow;
        this.currentStateId = workflow.initialStateId;
    }
}
