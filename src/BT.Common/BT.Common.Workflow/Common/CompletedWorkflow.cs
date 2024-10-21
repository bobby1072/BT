using System.Text.Json.Serialization;
using BT.Common.Workflow.Abstract;
using BT.Common.Workflow.Concrete;
using BT.Common.Workflow.Contexts;

namespace BT.Common.Workflow.Common
{
    public sealed record CompletedWorkflow<TContext, TReturn>
        where TContext : IWorkflowContext<
                IWorkflowInputContext,
                IWorkflowOutputContext<TReturn>,
                TReturn
            >
    {
        [JsonIgnore]
        private IWorkflow<TContext, TReturn> ActualWorkflow { get; init; }
        public Guid WorkflowId => ActualWorkflow.WorkflowRunId;
        public string WorkflowName => ActualWorkflow.Name;
        [JsonIgnore]
        public TReturn? WorkflowOutput => ActualWorkflow.Context.Output.ReturnObject;
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
            ActualWorkflow = actualWorkflow;
            CompletedAt = completedAt;
            TotalTimeTaken = timeTaken;
            CompletedActivities = completedActivities;
        }
    }
}