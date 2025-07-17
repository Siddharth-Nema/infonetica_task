using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("instances")]
public class InstancesController : ControllerBase
{
    private readonly JsonDbService _jsonDbService;

    public InstancesController(JsonDbService jsonDbService)
    {
        _jsonDbService = jsonDbService;
    }

    [HttpPost]
    public ActionResult<Instance> CreateInstance([FromBody] CreateInstanceRequest request)
    {
        if (request == null || request.workflowId <= 0)
            return BadRequest("Valid workflowId must be provided.");

        var workflows = _jsonDbService.LoadWorkflows();
        var workflow = workflows.FirstOrDefault(w => w.id == request.workflowId);
        if (workflow == null)
            return NotFound($"Workflow with id {request.workflowId} not found.");

        var instances = _jsonDbService.LoadInstances();
        int nextInstanceId = instances.Any() ? instances.Max(i => i.id) + 1 : 1;

        var instance = new Instance(workflow)
        {
            id = nextInstanceId
        };

        instances.Add(instance);
        _jsonDbService.SaveInstances(instances);

        return CreatedAtAction(nameof(GetInstance), new { id = instance.id }, instance);
    }

    [HttpGet("{id}")]
    public ActionResult GetInstance(int id)
    {
        var instances = _jsonDbService.LoadInstances();
        var instance = instances.FirstOrDefault(i => i.id == id);
        if (instance == null) return NotFound();

        var workflows = _jsonDbService.LoadWorkflows();
        var workflow = workflows.FirstOrDefault(w => w.id == instance.workflowId);

        return Ok(new
        {
            instance.id,
            instance.workflowId,
            workflowName = workflow?.name ?? "Unknown",
            instance.currentStateId,
            instance.history
        });
    }


    [HttpPost("{id}/changestate")]
    public ActionResult ChangeState(int id, [FromBody] ChangeStateRequest request)
    {
        if (request == null || request.actionId <= 0)
            return BadRequest("Valid actionId must be provided.");

        var instances = _jsonDbService.LoadInstances();
        var instance = instances.FirstOrDefault(i => i.id == id);
        if (instance == null)
            return NotFound($"Instance with id {id} not found.");

        var workflows = _jsonDbService.LoadWorkflows();
        var workflow = workflows.FirstOrDefault(w => w.id == instance.workflowId);
        if (workflow == null)
            return NotFound($"Workflow for instance id {id} not found.");

        var action = workflow.actions.FirstOrDefault(a =>
            a.id == request.actionId &&
            a.enabled &&
            a.fromStates.Contains(instance.currentStateId));

        if (action == null)
            return BadRequest("Invalid action for the current state.");

        var targetState = workflow.states.FirstOrDefault(s => s.id == action.toState);
        if (targetState == null || !targetState.enabled)
            return BadRequest("Target state is not enabled.");

        // Update instance state and log history
        instance.currentStateId = action.toState;
        instance.history.Add(new HistoryEntry
        {
            actionName = action.name,
            timestamp = DateTime.UtcNow
        });

        _jsonDbService.SaveInstances(instances);

        return Ok(new
        {
            instance.id,
            instance.workflowId,
            workflowName = workflow.name,
            instance.currentStateId,
            instance.history
        });
    }


    public class CreateInstanceRequest
    {
        public int workflowId { get; set; }
    }

    public class ChangeStateRequest
    {
        public int actionId { get; set; }
    }
}
