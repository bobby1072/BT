using BT.Common.WorkflowActivities.Activities.Concrete;
using BT.Common.WorkflowActivities.Contexts;
using System.Text.Json.Serialization;

namespace BT.Common.WorkflowActivities.Completed
{
    public sealed record CompletedActivityBlockToRun<
        TActivityContextItem,
        TActivityReturnItem
    >
        where TActivityContextItem : ActivityContextItem
        where TActivityReturnItem : ActivityReturnItem
    {
        public required IReadOnlyCollection<CompletedWorkflowActivity<TActivityContextItem, TActivityReturnItem>> CompletedWorkflowActivities { get; init; }
        [JsonIgnore]
        public required ActivityBlockExecutionTypeEnum ExecutionType { get; init; }
        [JsonPropertyName("ExecutionType")]
        public string ExecutionTypeString => ExecutionTypeString.ToString();

    }
}
