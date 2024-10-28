using BT.Common.WorkflowActivities.Activities.Concrete;
using System.Text.Json.Serialization;

namespace BT.Common.WorkflowActivities.Completed
{
    public sealed record CompletedActivityBlockToRun<
        TActivityContextItem,
        TActivityReturnItem
    >
    {
        public IReadOnlyCollection<CompletedWorkflowActivity<TActivityContextItem, TActivityReturnItem>> CompletedWorkflowActivities { get; init; }
        [JsonIgnore]
        public ActivityBlockExecutionTypeEnum ExecutionType { get; init; }
        [JsonPropertyName("ExecutionType")]
        public string ExecutionTypeString => ExecutionTypeString.ToString();

        public CompletedActivityBlockToRun(
            IReadOnlyCollection<CompletedWorkflowActivity<TActivityContextItem, TActivityReturnItem>> completedWorkflowActivities,
            ActivityBlockExecutionTypeEnum executionType
            )
        {
            CompletedWorkflowActivities = completedWorkflowActivities;
            ExecutionType = executionType;
        }

    }
}
