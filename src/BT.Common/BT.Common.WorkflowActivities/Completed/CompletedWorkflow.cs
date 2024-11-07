using BT.Common.WorkflowActivities.Abstract;
using BT.Common.WorkflowActivities.Contexts;
using System.Text.Json.Serialization;

namespace BT.Common.WorkflowActivities.Completed
{
    public sealed record CompletedWorkflow<TContext, TInputContext, TOutputContext, TReturn>
        where TContext : WorkflowContext<
                TInputContext,
                TOutputContext,
                TReturn
            >
        where TInputContext : WorkflowInputContext
        where TOutputContext : WorkflowOutputContext<TReturn>
    {
        [JsonIgnore]
        public required IWorkflow<TContext, TInputContext, TOutputContext, TReturn> ActualWorkflow { get; init; }
        public Guid WorkflowId => ActualWorkflow.WorkflowRunId;
        public string WorkflowName => ActualWorkflow.Name;
        [JsonIgnore]
        public TReturn? WorkflowOutput => ActualWorkflow.Context.Output.ReturnObject;
        public required DateTime StartedAt { get; init; }
        public required DateTime CompletedAt { get; init; }
        [JsonIgnore]
        public required TimeSpan TotalTimeTaken { get; init; }
        public double TotalTimeTakenMilliSeconds => TotalTimeTaken.TotalMilliseconds;
        public required IReadOnlyCollection<CompletedActivityBlockToRun<object?, object?>> CompletedActivities { get; init; }
    }
}