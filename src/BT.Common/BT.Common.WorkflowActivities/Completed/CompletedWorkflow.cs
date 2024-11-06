using BT.Common.WorkflowActivities.Abstract;
using BT.Common.WorkflowActivities.Contexts;
using System.Text.Json.Serialization;

namespace BT.Common.WorkflowActivities.Completed
{
    public sealed record CompletedWorkflow<TContext, TInputContext, TOutputContext, TReturn>
        where TContext : IWorkflowContext<
                TInputContext,
                TOutputContext,
                TReturn
            >
        where TInputContext : IWorkflowInputContext
        where TOutputContext : IWorkflowOutputContext<TReturn>
    {
        [JsonIgnore]
        private IWorkflow<TContext, TInputContext, TOutputContext, TReturn> _actualWorkflow { get; init; }
        public Guid WorkflowId => _actualWorkflow.WorkflowRunId;
        public string WorkflowName => _actualWorkflow.Name;
        [JsonIgnore]
        public TReturn? WorkflowOutput => _actualWorkflow.Context.Output.ReturnObject;
        public DateTime StartedAt { get; init; }
        public DateTime CompletedAt { get; init; }
        [JsonIgnore]
        public TimeSpan TotalTimeTaken { get; init; }
        public double TotalTimeTakenMilliSeconds => TotalTimeTaken.TotalMilliseconds;
        public IReadOnlyCollection<CompletedActivityBlockToRun<object?, object?>> CompletedActivities { get; init; }
        public CompletedWorkflow(
            IWorkflow<TContext, TInputContext, TOutputContext, TReturn> actualWorkflow,
            DateTime startedAt,
            DateTime completedAt,
            TimeSpan timeTaken,
            IReadOnlyCollection<CompletedActivityBlockToRun<object?, object?>> completedActivities
        )
        {
            _actualWorkflow = actualWorkflow;
            CompletedAt = completedAt;
            StartedAt = startedAt;
            TotalTimeTaken = timeTaken;
            CompletedActivities = completedActivities;
        }
    }
}