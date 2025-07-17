public class Instance
{
    public int id { get; set; }
    public int workflowId { get; set; }
    public int currentStateId { get; set; }
    public List<HistoryEntry> history { get; set; } = new List<HistoryEntry>();
    
    public Instance() {}

    public Instance(Workflow workflow)
    {
        if (workflow == null)
            throw new ArgumentNullException(nameof(workflow));

        workflowId = workflow.id;
        currentStateId = workflow.initialStateId;
    }
}
