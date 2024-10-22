using System.Text.Json.Serialization;
using BT.Common.Workflow.Abstract;
using BT.Common.Workflow.Contexts;

namespace BT.Common.Workflow.Completed
{
    public record CompletedWorkflow<TContext, TReturn>
        where TContext : IWorkflowContext<
                IWorkflowInputContext,
                IWorkflowOutputContext<TReturn>,
                TReturn
            >
    {
        [JsonIgnore]
        private IWorkflow<TContext, TReturn> _actualWorkflow { get; init; }
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
            IWorkflow<TContext, TReturn> actualWorkflow,
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