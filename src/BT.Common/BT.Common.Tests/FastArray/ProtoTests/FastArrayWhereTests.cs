using BT.Common.FastArray.Proto;
using BT.Common.FastArray.Tests.TestModels;

namespace BT.Common.FastArray.Tests.ProtoTests
{
    public class FastArrayWhereTests : FastArrayTestBase
    {
        private class FastArrayWhereTestsFunctionalityClassData : TheoryData<IReadOnlyCollection<object>, Func<IEnumerable<object>, IEnumerable<object>>, Func<IEnumerable<object>, IEnumerable<object>>>
        {
            public FastArrayWhereTestsFunctionalityClassData()
            {
                Add(new List<object> { 1, 2, 3 }, x => x.Where(y => y is int), x => x.FastArrayWhere(y => y is int));

                foreach (var arrayData in BasicArraysToTestFunctionality)
                {
                    Add(arrayData, x => x.Where(y => y is not null), x => x.FastArrayWhere(y => y is not null));
                    Add(arrayData, x => x.Where(y => y is true), x => x.FastArrayWhere(y => y is true));
                    Add(arrayData, x => x.Where(y => y is false), x => x.FastArrayWhere(y => y is false));
                    Add(arrayData, x => x.Where(y => y is int), x => x.FastArrayWhere(y => y is int));
                    Add(arrayData, x => x.Where(y => y is int && (int)y > 100), x => x.FastArrayWhere(y => y is int && (int)y > 100));
                    Add(arrayData, x => x.Where(y => y is int && (int)y > 1000), x => x.FastArrayWhere(y => y is int && (int)y > 1000));
                    Add(arrayData, x => x.Where(y => y is string), x => x.FastArrayWhere(y => y is string));
                    Add(arrayData, x => x.Where(y => y is TestVehicle), x => x.FastArrayWhere(y => y is TestVehicle));
                    Add(arrayData, x => x.Where(y => y is TestCar), x => x.FastArrayWhere(y => y is TestCar));
                    Add(arrayData, x => x.Where(y => y is TestPlane), x => x.FastArrayWhere(y => y is TestPlane));
                }
            }
        }
        [Theory]
        [ClassData(typeof(FastArrayWhereTestsFunctionalityClassData))]
        public void FastArrayWhereTests_Functionality(IReadOnlyCollection<object> arrayData, Func<IEnumerable<object>, IEnumerable<object>> actualFunc, Func<IEnumerable<object>, IEnumerable<object>> yourFunc)
        {
            FunctionalityTestRunner(arrayData, actualFunc, yourFunc);
        }
        private class FastArrayWhereTestsPerformanceClassData : TheoryData<IReadOnlyCollection<object>, Func<IEnumerable<object>, IEnumerable<object>>, Func<IEnumerable<object>, IEnumerable<object>>>
        {
            public FastArrayWhereTestsPerformanceClassData()
            {
                var allArrays = CreateLargeArraysForPerformanceTestAllTypes(4000, 35);
                foreach (var arrayData in allArrays)
                {
                    Add(arrayData, x => x.Where(y => y is not null), x => x.FastArrayWhere(y => y is not null));
                    Add(arrayData, x => x.Where(y => y is true), x => x.FastArrayWhere(y => y is true));
                    Add(arrayData, x => x.Where(y => y is false), x => x.FastArrayWhere(y => y is false));
                    Add(arrayData, x => x.Where(y => y is int), x => x.FastArrayWhere(y => y is int));
                    Add(arrayData, x => x.Where(y => y is int && (int)y > 100), x => x.FastArrayWhere(y => y is int && (int)y > 100));
                    Add(arrayData, x => x.Where(y => y is int && (int)y > 1000), x => x.FastArrayWhere(y => y is int && (int)y > 1000));
                    Add(arrayData, x => x.Where(y => y is string), x => x.FastArrayWhere(y => y is string));
                    Add(arrayData, x => x.Where(y => y is TestVehicle), x => x.FastArrayWhere(y => y is TestVehicle));
                    Add(arrayData, x => x.Where(y => y is TestCar), x => x.FastArrayWhere(y => y is TestCar));
                    Add(arrayData, x => x.Where(y => y is TestPlane), x => x.FastArrayWhere(y => y is TestPlane));
                }
            }
        }
    }
}
