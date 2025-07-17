using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("workflows")]
public class WorkflowsController : ControllerBase
{
    private readonly JsonDbService _jsonDbService;

    public WorkflowsController(JsonDbService jsonDbService)
    {
        _jsonDbService = jsonDbService;
    }

    [HttpPost]
    public ActionResult<Workflow> CreateWorkflow([FromBody] Workflow workflow)
    {
        if (workflow == null)
        {
            return BadRequest("Workflow cannot be null");
        }

        var workflows = _jsonDbService.LoadWorkflows();

        int nextId = workflows.Any() ? workflows.Max(w => w.id) + 1 : 1;
        workflow.id = nextId;
        var initialStates = workflow.states.Where(s => s.isInitial && s.enabled).ToList();

        if (initialStates.Count != 1)
        {
            return BadRequest("There must be exactly one enabled state marked as initial.");
        }

        workflow.initialStateId = initialStates[0].id;



        workflows.Add(workflow);
        _jsonDbService.SaveWorkflows(workflows);
        

        return CreatedAtAction(nameof(GetWorkflow), new { id = workflow.id }, workflow);
    }

    [HttpGet("{id}")]
    public ActionResult<Workflow> GetWorkflow(int id)
    {
        var workflows = _jsonDbService.LoadWorkflows();
        var workflow = workflows.FirstOrDefault(w => w.id == id);

        if (workflow == null)
        {
            return NotFound($"Workflow with id {id} not found");
        }

        return Ok(workflow);
    }
}
