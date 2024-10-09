using BT.Common.FastArray.Proto;

namespace BT.Common.FastArray.Tests.ProtoTests
{
    public class FastArrayDistinctTests : FastArrayTestBase
    {
        private class FastArrayDistinctTests_Functionality_Class_Data : TheoryData<IReadOnlyCollection<object>, Func<IEnumerable<object>, IEnumerable<object>>, Func<IEnumerable<object>, IEnumerable<object>>>
        {
            public FastArrayDistinctTests_Functionality_Class_Data()
            {
                Add(new List<object> { 1, 2, 3 }, x => x.Distinct(), x => x.FastArrayDistinct());

                foreach (var arrayData in _basicArraysToTestFunctionality)
                {
                    Add(arrayData, x => x.Distinct(), x => x.FastArrayDistinct());
                }
            }
        }
        [Theory]
        [ClassData(typeof(FastArrayDistinctTests_Functionality_Class_Data))]
        public void FastArrayDistinctTests_Functionality(IReadOnlyCollection<object> arrayData, Func<IEnumerable<object>, IEnumerable<object>> actualFunc, Func<IEnumerable<object>, IEnumerable<object>> yourFunc)
        {
            FunctionalityTestRunner(arrayData, actualFunc, yourFunc);
        }
        private class FastArrayDistinctByTests_Functionality_Class_Data : TheoryData<IReadOnlyCollection<object>, Func<IEnumerable<object>, IEnumerable<object>>, Func<IEnumerable<object>, IEnumerable<object>>>
        {
            public FastArrayDistinctByTests_Functionality_Class_Data()
            {
                Add(new List<object> { 1, 2, 3 }, x => x.DistinctBy(x => x), x => x.FastArrayDistinctBy(x => x));

                foreach (var arrayData in _basicArraysToTestFunctionality)
                {
                    Add(arrayData, x => x.DistinctBy(x => x), x => x.FastArrayDistinctBy(x => x));
                }
            }
        }
        [Theory]
        [ClassData(typeof(FastArrayDistinctByTests_Functionality_Class_Data))]
        public void FastArrayDistinctByTests_Functionality(IReadOnlyCollection<object> arrayData, Func<IEnumerable<object>, IEnumerable<object>> actualFunc, Func<IEnumerable<object>, IEnumerable<object>> yourFunc)
        {
            FunctionalityTestRunner(arrayData, actualFunc, yourFunc);
        }
    }
}