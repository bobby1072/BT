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
        > ActualActivity
        { get; init; }
        public Guid RanActivityId => ActualActivity.ActualActivity.ActivityRunId;
        public string RanActivityName => ActualActivity.ActualActivity.Name;
        public int NumberOfRetries { get; init; }
        public DateTime CompletedAt { get; init; }
        [JsonIgnore]
        public TimeSpan TotalTimeTaken { get; init; }
        public double TotalTimeTakenMilliSeconds => TotalTimeTaken.TotalMilliseconds;

        public CompletedWorkflowActualActivity(
            ActualActivityToRun<TActivityContextItem, TActivityReturnItem> actualActivity,
            DateTime completedAt,
            int numberOfRetries,
            TimeSpan timeTaken
        )
        {
            ActualActivity = actualActivity;
            CompletedAt = completedAt;
            NumberOfRetries = numberOfRetries;
            TotalTimeTaken = timeTaken;
        }
    }
}
