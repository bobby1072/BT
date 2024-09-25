using BT.Common.FastArray.Proto;
using BT.Common.FastArray.Tests.TestModels;

namespace BT.Common.FastArray.Tests.ProtoTests
{
    public class FastArrayDistinctTests : FastArrayTestBase
    {
        private class FastArrayDistinctTests_Functionality_Class_Data : TheoryData<IReadOnlyCollection<object>, Func<IEnumerable<object>, IEnumerable<object>>, Func<IEnumerable<object>, IEnumerable<object>>>
        {
            public FastArrayDistinctTests_Functionality_Class_Data()
            {
                Add(new List<object> { 1, 2, 3 }, x => x.Distinct(), x => x.Distinct());

                foreach (var arrayData in _basicArraysToTestFunctionality)
                {
                    Add(arrayData, x => x.Distinct(), x => x.Distinct());
                }
            }
        }
        [Theory]
        [ClassData(typeof(FastArrayDistinctTests_Functionality_Class_Data))]
        public void FastArrayDistinctTests_Functionality(IReadOnlyCollection<object> arrayData, Func<IEnumerable<object>, IEnumerable<object>> actualFunc, Func<IEnumerable<object>, IEnumerable<object>> yourFunc)
        {
            FunctionalityTestRunner(arrayData, actualFunc, yourFunc);
        }
    }
}