using System.Text.Json.Serialization;
using BT.Common.Workflow.Abstract;
using BT.Common.Workflow.Contexts;

namespace BT.Common.Workflow.Completed
{
    public sealed record CompletedWorkflow<TContext, TReturn>
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
        public DateTime CompletedAt { get; init; }
        [JsonIgnore]
        public TimeSpan TotalTimeTaken { get; init; }
        public double TotalTimeTakenMilliSeconds => TotalTimeTaken.TotalMilliseconds;
        public IReadOnlyCollection<CompletedWorkflowActualActivity<object?, object?>> CompletedActivities { get; init; }
        public CompletedWorkflow(
            IWorkflow<TContext, TReturn> actualWorkflow,
            DateTime completedAt,
            TimeSpan timeTaken,
            IReadOnlyCollection<CompletedWorkflowActualActivity<object?, object?>> completedActivities
        )
        {
            _actualWorkflow = actualWorkflow;
            CompletedAt = completedAt;
            TotalTimeTaken = timeTaken;
            CompletedActivities = completedActivities;
        }
    }
}