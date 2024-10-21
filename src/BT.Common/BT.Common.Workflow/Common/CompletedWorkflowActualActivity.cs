using System.Text.Json.Serialization;
using BT.Common.Workflow.Activities.Concrete;

namespace BT.Common.Workflow.Concrete
{
    public sealed record CompletedWorkflowActualActivity<
        TActivityContextItem,
        TActivityReturnItem
    >
    {
        [JsonIgnore]
        private ActualActivityToRun<
            TActivityContextItem,
            TActivityReturnItem
        > _actualActivity
        { get; init; }
        public Guid ActivityId => _actualActivity.ActualActivity.ActivityRunId;
        public string ActivityName => _actualActivity.ActualActivity.Name;
        [JsonIgnore]
        private ActivityResultEnum ActivityResult { get; init; }
        [JsonPropertyName("FinalActivityState")]
        public string FinalActivityStateString => ActivityResult.ToString();
        public int NumberOfRetries { get; init; }
        public DateTime CompletedAt { get; init; }
        [JsonIgnore]
        public TimeSpan TotalTimeTaken { get; init; }
        public double TotalTimeTakenMilliSeconds => TotalTimeTaken.TotalMilliseconds;

        public CompletedWorkflowActualActivity(
            ActualActivityToRun<TActivityContextItem, TActivityReturnItem> actualActivity,
            DateTime completedAt,
            int numberOfRetries,
            TimeSpan timeTaken,
            ActivityResultEnum activityResult
        )
        {
            _actualActivity = actualActivity;
            CompletedAt = completedAt;
            NumberOfRetries = numberOfRetries;
            TotalTimeTaken = timeTaken;
            ActivityResult = activityResult;
        }
    }
}
