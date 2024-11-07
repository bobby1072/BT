using BT.Common.WorkflowActivities.Activities.Concrete;
using System.Text.Json.Serialization;

namespace BT.Common.WorkflowActivities.Completed
{
    public sealed record CompletedActivityBlockToRun<
        TActivityContextItem,
        TActivityReturnItem
    >
    {
        public required IReadOnlyCollection<CompletedWorkflowActivity<TActivityContextItem, TActivityReturnItem>> CompletedWorkflowActivities { get; init; }
        [JsonIgnore]
        public required ActivityBlockExecutionTypeEnum ExecutionType { get; init; }
        [JsonPropertyName("ExecutionType")]
        public string ExecutionTypeString => ExecutionTypeString.ToString();

    }
}
