using BT.Common.Helpers.TypeFor;
using BT.Common.Workflow.Activities;

namespace BT.Common.Workflow.Models
{
    internal sealed record CompletedWorkflowActivityResult<TActivityContextItem>
    {
        public TypeFor<IActivity<TActivityContextItem>> ActivityType { get; init; }
        public DateTime CompletedAt { get; init; }
        public int NumberOfRetries { get; init; }
        public TimeSpan TimeTaken { get; init; }

        public CompletedWorkflowActivityResult(
            TypeFor<IActivity<TActivityContextItem>> actvityType,
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
