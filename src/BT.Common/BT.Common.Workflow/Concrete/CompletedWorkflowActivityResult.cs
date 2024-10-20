using BT.Common.Helpers.TypeFor;
using BT.Common.Workflow.Activities.Abstract;

namespace BT.Common.Workflow.Concrete
{
    internal sealed record CompletedWorkflowActivityResult<TActivityContextItem, TActivityReturnItem>
    {
        public TypeFor<IActivity<TActivityContextItem, TActivityReturnItem>> ActivityType { get; init; }
        public DateTime CompletedAt { get; init; }
        public int NumberOfRetries { get; init; }
        public TimeSpan TimeTaken { get; init; }

        public CompletedWorkflowActivityResult(
            TypeFor<IActivity<TActivityContextItem, TActivityReturnItem>> actvityType,
            DateTime completedAt,
            int numberOfRetries,
            TimeSpan timeTaken
        )
        {
            ActivityType = actvityType;
            CompletedAt = completedAt;
            NumberOfRetries = numberOfRetries;
            TimeTaken = timeTaken;
        }
    }
}
