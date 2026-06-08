using BT.Common.FastArray.Proto;

namespace BT.Common.FastArray.Tests.ProtoTests
{
    public class FastArrayDistinctTests : FastArrayTestBase
    {
        private class FastArrayDistinctTestsFunctionalityClassData : TheoryData<IReadOnlyCollection<object>, Func<IEnumerable<object>, IEnumerable<object>>, Func<IEnumerable<object>, IEnumerable<object>>>
        {
            public FastArrayDistinctTestsFunctionalityClassData()
            {
                Add(new List<object> { 1, 2, 3 }, x => x.Distinct(), x => x.FastArrayDistinct());

                foreach (var arrayData in BasicArraysToTestFunctionality)
                {
                    Add(arrayData, x => x.Distinct(), x => x.FastArrayDistinct());
                }
            }
        }
        [Theory]
        [ClassData(typeof(FastArrayDistinctTestsFunctionalityClassData))]
        public void FastArrayDistinctTests_Functionality(IReadOnlyCollection<object> arrayData, Func<IEnumerable<object>, IEnumerable<object>> actualFunc, Func<IEnumerable<object>, IEnumerable<object>> yourFunc)
        {
            FunctionalityTestRunner(arrayData, actualFunc, yourFunc);
        }
        private class FastArrayDistinctByTestsFunctionalityClassData : TheoryData<IReadOnlyCollection<object>, Func<IEnumerable<object>, IEnumerable<object>>, Func<IEnumerable<object>, IEnumerable<object>>>
        {
            public FastArrayDistinctByTestsFunctionalityClassData()
            {
                Add(new List<object> { 1, 2, 3 }, x => x.DistinctBy(x => x), x => x.FastArrayDistinctBy(x => x));

                foreach (var arrayData in BasicArraysToTestFunctionality)
                {
                    Add(arrayData, x => x.DistinctBy(x => x), x => x.FastArrayDistinctBy(x => x));
                }
            }
        }
        [Theory]
        [ClassData(typeof(FastArrayDistinctByTestsFunctionalityClassData))]
        public void FastArrayDistinctByTests_Functionality(IReadOnlyCollection<object> arrayData, Func<IEnumerable<object>, IEnumerable<object>> actualFunc, Func<IEnumerable<object>, IEnumerable<object>> yourFunc)
        {
            FunctionalityTestRunner(arrayData, actualFunc, yourFunc);
        }
    }
}