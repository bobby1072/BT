using BT.Common.Workflow.Activities.Abstract;

namespace BT.Common.Workflow.Activities.Concrete
{
    internal record ActualActivityToRun<TActivityContextItem, TActivityReturnItem>
    {
        public IActivity<TActivityContextItem?, TActivityReturnItem?> ActualActivity { get; init; }
        public Func<
            Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>
        > ActualExecuteAsync { get; init; }

        public ActualActivityToRun(
            IActivity<TActivityContextItem?, TActivityReturnItem?> actualActivity,
            Func<
                Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>
            > actualExecuteAsync
        )
        {
            ActualExecuteAsync = actualExecuteAsync;
            ActualActivity = actualActivity;
        }
    }
}
