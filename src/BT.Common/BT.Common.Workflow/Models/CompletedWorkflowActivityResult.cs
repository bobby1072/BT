using AiTrainer.Web.Workflow.Activities;
using BT.Common.Helpers.TypeFor;

namespace AiTrainer.Web.Workflow.Models
{
    public sealed record CompletedWorkflowActivityResult<T>
    {
        public TimeSpan TimeTaken { get; init; }
        public DateTime CompletedAt { get; init; }
        public TypeFor<IActivity<T>> ActivityType { get; init; }
        public int NumberOfRetries { get; init; }

        public CompletedWorkflowActivityResult(
            TimeSpan timeTaken,
            DateTime completedAt,
            TypeFor<IActivity<T>> actvityType,
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
