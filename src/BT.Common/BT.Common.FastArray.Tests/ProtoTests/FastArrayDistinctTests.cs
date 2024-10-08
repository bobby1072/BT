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
            private class TestIsStringEqualityComparer : IEqualityComparer<object>
            {
                public new bool Equals(object? x, object? y)
                {
                    return x is string && y is string && x == y;
                }
                public int GetHashCode(object obj)
                {
                    return obj.GetHashCode();
                }
            }
            private class TestIsNullEqualityComparer : IEqualityComparer<object>
            {
                public new bool Equals(object? x, object? y)
                {
                    return x == null && y == null;
                }
                public int GetHashCode(object obj)
                {
                    return obj.GetHashCode();
                }
            }
            private class TestIsIntAndXGreaterThanYEqualityComparer : IEqualityComparer<object>
            {
                public new bool Equals(object? x, object? y)
                {
                    return x is int foundX && y is int foundY && foundX > foundY;
                }
                public int GetHashCode(object obj)
                {
                    return obj.GetHashCode();
                }
            }
            public FastArrayDistinctByTests_Functionality_Class_Data()
            {
                Add(new List<object> { 1, 2, 3 }, x => x.DistinctBy(x => x), x => x.FastArrayDistinctBy(x => x));

                foreach (var arrayData in _basicArraysToTestFunctionality)
                {
                    Add(arrayData, x => x.DistinctBy(x => x), x => x.FastArrayDistinctBy(x => x));
                    Add(arrayData, x => x.DistinctBy(x => x, new TestIsStringEqualityComparer()), x => x.FastArrayDistinctBy(x => x, (x, y) => x is string && y is string && x == y));
                    Add(arrayData, x => x.DistinctBy(x => x, new TestIsNullEqualityComparer()), x => x.FastArrayDistinctBy(x => x, (x, y) => x == null && y == null));
                    Add(arrayData, x => x.DistinctBy(x => x, new TestIsIntAndXGreaterThanYEqualityComparer()), x => x.FastArrayDistinctBy(x => x, (x, y) => x is int foundX && y is int foundY && foundX > foundY));
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