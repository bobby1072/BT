using System.Text.Json.Serialization;
using BT.Common.Workflow.Activities.Abstract;
using BT.Common.Workflow.Activities.Concrete;

namespace BT.Common.Workflow.Completed
{
    public sealed record CompletedWorkflowActualActivity<
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
        private ActivityResultEnum ActivityResult { get; init; }
        [JsonPropertyName("FinalActivityState")]
        public string FinalActivityStateString => ActivityResult.ToString();
        public int NumberOfRetriesTaken { get; init; }
        public DateTime CompletedAt { get; init; }
        [JsonIgnore]
        public TimeSpan TotalTimeTaken { get; init; }
        public double TotalTimeTakenMilliSeconds => TotalTimeTaken.TotalMilliseconds;

        public CompletedWorkflowActualActivity(
           IActivity<TActivityContextItem?, TActivityReturnItem?> actualActivity,
            DateTime completedAt,
            int numberOfRetries,
            TimeSpan timeTaken,
            ActivityResultEnum activityResult
        )
        {
            _activity = actualActivity;
            CompletedAt = completedAt;
            NumberOfRetriesTaken = numberOfRetries;
            TotalTimeTaken = timeTaken;
            ActivityResult = activityResult;
        }
    }
}
