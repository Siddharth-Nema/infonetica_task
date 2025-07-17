using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("workflows")]
public class WorkflowsController : ControllerBase
{
    private static readonly Dictionary<int, Workflow> workflows = new Dictionary<int, Workflow>();
    private static int nextId = 1;

    [HttpPost]
    public ActionResult<Workflow> CreateWorkflow([FromBody] Workflow workflow)
    {
        if (workflow == null)
        {
            return BadRequest("Workflow cannot be null");
        }

        workflow.id = nextId++;
        workflows.Add(workflow.id, workflow);

        return CreatedAtAction(nameof(GetWorkflow), new { id = workflow.id }, workflow);
    }

    [HttpGet("{id}")]
    public ActionResult<Workflow> GetWorkflow(int id)
    {
        if (!workflows.TryGetValue(id, out var workflow))
        {
            return NotFound($"Workflow with id {id} not found");
        }

        return Ok(workflow);
    }
}
