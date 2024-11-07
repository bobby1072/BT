using BT.Common.WorkflowActivities.Activities.Abstract;
using BT.Common.WorkflowActivities.Activities.Concrete;
using System.Text.Json.Serialization;

namespace BT.Common.WorkflowActivities.Completed
{
    public sealed record CompletedWorkflowActivity<
        TActivityContextItem,
        TActivityReturnItem
    >
    {
        [JsonIgnore]
        public required IActivity<TActivityContextItem?, TActivityReturnItem?> Activity
        { get; init; }
        public Guid ActivityId => Activity.ActivityRunId;
        public string ActivityName => Activity.Name;
        public string ActivityDescription => Activity.Description;
        [JsonIgnore]
        public required ActivityResultEnum ActivityResult { get; init; }
        [JsonPropertyName("FinalActivityState")]
        public string FinalActivityStateString => ActivityResult.ToString();
        public required int NumberOfRetriesTaken { get; init; }
        [JsonIgnore]
        public required TimeSpan TotalTimeTaken { get; init; }
        public double TotalTimeTakenMilliSeconds => TotalTimeTaken.TotalMilliseconds;
    }
}
