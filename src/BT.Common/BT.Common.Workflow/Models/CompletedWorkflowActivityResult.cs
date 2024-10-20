using BT.Common.Helpers.TypeFor;
using BT.Common.Workflow.Activities;

namespace BT.Common.Workflow.Models
{
    public sealed record CompletedWorkflowActivityResult<TActivityContextItem>
    {
        public TimeSpan TimeTaken { get; init; }
        public DateTime CompletedAt { get; init; }
        public TypeFor<IActivity<TActivityContextItem>> ActivityType { get; init; }
        public int NumberOfRetries { get; init; }

        public CompletedWorkflowActivityResult(
            TimeSpan timeTaken,
            DateTime completedAt,
            TypeFor<IActivity<TActivityContextItem>> actvityType,
            int numberOfRetries
        )
        {
            TimeTaken = timeTaken;
            CompletedAt = completedAt;
            ActivityType = actvityType;
            NumberOfRetries = numberOfRetries;
        }
    }
}
