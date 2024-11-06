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
        private IActivity<TActivityContextItem?, TActivityReturnItem?> _activity
        { get; init; }
        public Guid ActivityId => _activity.ActivityRunId;
        public string ActivityName => _activity.Name;
        [JsonIgnore]
        public ActivityResultEnum ActivityResult { get; init; }
        [JsonPropertyName("FinalActivityState")]
        public string FinalActivityStateString => ActivityResult.ToString();
        public int NumberOfRetriesTaken { get; init; }
        [JsonIgnore]
        public TimeSpan TotalTimeTaken { get; init; }
        public double TotalTimeTakenMilliSeconds => TotalTimeTaken.TotalMilliseconds;

        public CompletedWorkflowActivity(
           IActivity<TActivityContextItem?, TActivityReturnItem?> actualActivity,
            int numberOfRetries,
            TimeSpan timeTaken,
            ActivityResultEnum activityResult
        )
        {
            _activity = actualActivity;
            NumberOfRetriesTaken = numberOfRetries;
            TotalTimeTaken = timeTaken;
            ActivityResult = activityResult;
        }
    }
}
