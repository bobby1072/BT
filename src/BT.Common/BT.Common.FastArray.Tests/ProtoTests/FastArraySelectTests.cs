using BT.Common.FastArray.Proto;

namespace BT.Common.FastArray.Tests.ProtoTests
{
    public class FastArraySelectTests : FastArrayTestBase
    {
        private class FastArraySelect__Functionality_Class_Data : TheoryData<IReadOnlyCollection<object>, Func<IEnumerable<object>, IEnumerable<object>>, Func<IEnumerable<object>, IEnumerable<object>>>
        {
            public FastArraySelect__Functionality_Class_Data()
            {
                Add(new List<object> { 1, 2, 3 }, x => x.Select(y => y), x => x.FastArraySelect(y => y));

                foreach (var arrayData in _basicArraysToTestFunctionality)
                {
                    Add(arrayData, x => x.Select(y => y), x => x.FastArraySelect(y => y));
                }
            }
        }
        [Theory]
        [ClassData(typeof(FastArraySelect__Functionality_Class_Data))]
        public void FastArraySelect__Functionality(IReadOnlyCollection<object> arrayData, Func<IEnumerable<object>, IEnumerable<object>> actualFunc, Func<IEnumerable<object>, IEnumerable<object>> yourFunc)
        {
            FunctionalityTestRunner(arrayData, actualFunc, yourFunc);
        }
    }
}