using BT.Common.Helpers.TypeFor;
using BT.Common.Workflow.Activities.Abstract;
using BT.Common.Workflow.Activities.Concrete;

namespace BT.Common.Workflow.Concrete
{
    internal sealed record CompletedWorkflowActualActivityResult<
        TActivityContextItem,
        TActivityReturnItem
    >
    {
        public ActualActivityToRun<
            TActivityContextItem,
            TActivityReturnItem
        > ActualActivity { get; init; }
        public DateTime CompletedAt { get; init; }
        public int NumberOfRetries { get; init; }
        public TimeSpan TimeTaken { get; init; }

        public CompletedWorkflowActualActivityResult(
            ActualActivityToRun<TActivityContextItem, TActivityReturnItem> actualActivity,
            DateTime completedAt,
            int numberOfRetries,
            TimeSpan timeTaken
        )
        {
            ActualActivity = actualActivity;
            CompletedAt = completedAt;
            NumberOfRetries = numberOfRetries;
            TimeTaken = timeTaken;
        }
    }
}
