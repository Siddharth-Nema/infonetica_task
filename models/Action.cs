using System.Collections.Generic;

public class Action
{
    public int id { get; set; }
    public string name { get; set; }
    public bool enabled { get; set; }
    public List<int> fromStates { get; set; } = new List<int>();
    public int toState { get; set; }
}
