using BT.Common.FastArray.Proto;
using BT.Common.FastArray.Tests.TestModels;

namespace BT.Common.FastArray.Tests.ProtoTests
{
    public class FastArrayFirstTests : FastArrayTestBase
    {
        private class FastArrayFirstOrDefaultTestsFunctionalityClassData : TheoryData<IReadOnlyCollection<object>, Func<IEnumerable<object>, IEnumerable<object>>, Func<IEnumerable<object>, IEnumerable<object>>>
        {
            public FastArrayFirstOrDefaultTestsFunctionalityClassData()
            {
                Add(new List<object> { 1, 2, 3 }, x => [x.FirstOrDefault(y => y is int)!], x => [x.FastArrayFirstOrDefault(y => y is int)!]);

                foreach (var arrayData in BasicArraysToTestFunctionality)
                {
                    Add(arrayData, x => [x.FirstOrDefault(y => y is not null)!], x => [x.FastArrayFirstOrDefault(y => y is not null)!]);
                    Add(arrayData, x => [x.FirstOrDefault(y => y is true)!], x => [x.FastArrayFirstOrDefault(y => y is true)!]);
                    Add(arrayData, x => [x.FirstOrDefault(y => y is false)!], x => [x.FastArrayFirstOrDefault(y => y is false)!]);
                    Add(arrayData, x => [x.FirstOrDefault(y => y is int)!], x => [x.FastArrayFirstOrDefault(y => y is int)!]);
                    Add(arrayData, x => [x.FirstOrDefault(y => y is int && (int)y > 100)!], x => [x.FastArrayFirstOrDefault(y => y is int && (int)y > 100)!]);
                    Add(arrayData, x => [x.FirstOrDefault(y => y is int && (int)y > 1000)!], x => [x.FastArrayFirstOrDefault(y => y is int && (int)y > 1000)!]);
                    Add(arrayData, x => [x.FirstOrDefault(y => y is string)!], x => [x.FastArrayFirstOrDefault(y => y is string)!]);
                    Add(arrayData, x => [x.FirstOrDefault(y => y is TestVehicle)!], x => [x.FastArrayFirstOrDefault(y => y is TestVehicle)!]);
                    Add(arrayData, x => [x.FirstOrDefault(y => y is TestCar)!], x => [x.FastArrayFirstOrDefault(y => y is TestCar)!]);
                    Add(arrayData, x => [x.FirstOrDefault(y => y is TestPlane)!], x => [x.FastArrayFirstOrDefault(y => y is TestPlane)!]);
                }
            }
        }
        [Theory]
        [ClassData(typeof(FastArrayFirstOrDefaultTestsFunctionalityClassData))]
        public void FastArrayFirstOrDefaultTests_Functionality(IReadOnlyCollection<object> arrayData, Func<IEnumerable<object>, IEnumerable<object>> actualFunc, Func<IEnumerable<object>, IEnumerable<object>> yourFunc)
        {
            FunctionalityTestRunner(arrayData, actualFunc, yourFunc);
        }
        private class FastArrayFirstTestsFunctionalityClassData : TheoryData<IReadOnlyCollection<object>, Func<IEnumerable<object>, IEnumerable<object>>, Func<IEnumerable<object>, IEnumerable<object>>>
        {
            public FastArrayFirstTestsFunctionalityClassData()
            {
                Add(new List<object> { 1, 2, 3 }, x => [x.FirstOrDefault(y => y is int)!], x => [x.FastArrayFirst(y => y is int)!]);

                foreach (var arrayData in BasicArraysToTestFunctionality)
                {
                    Add(arrayData, x => [x.First(y => y is not null)!], x => [x.FastArrayFirst(y => y is not null)!]);
                    Add(arrayData, x => [x.First(y => y is true)!], x => [x.FastArrayFirst(y => y is true)!]);
                    Add(arrayData, x => [x.First(y => y is false)!], x => [x.FastArrayFirst(y => y is false)!]);
                    Add(arrayData, x => [x.First(y => y is int)!], x => [x.FastArrayFirst(y => y is int)!]);
                    Add(arrayData, x => [x.First(y => y is int && (int)y > 100)!], x => [x.FastArrayFirst(y => y is int && (int)y > 100)!]);
                    Add(arrayData, x => [x.First(y => y is int && (int)y > 1000)!], x => [x.FastArrayFirst(y => y is int && (int)y > 1000)!]);
                    Add(arrayData, x => [x.First(y => y is string)!], x => [x.FastArrayFirst(y => y is string)!]);
                    Add(arrayData, x => [x.First(y => y is TestVehicle)!], x => [x.FastArrayFirst(y => y is TestVehicle)!]);
                    Add(arrayData, x => [x.First(y => y is TestCar)!], x => [x.FastArrayFirst(y => y is TestCar)!]);
                    Add(arrayData, x => [x.First(y => y is TestPlane)!], x => [x.FastArrayFirst(y => y is TestPlane)!]);
                }
            }
        }
        
        [Theory]
        [ClassData(typeof(FastArrayFirstTestsFunctionalityClassData))]
        public void FastArrayFirstTests_Functionality(IReadOnlyCollection<object> arrayData, Func<IEnumerable<object>, IEnumerable<object>> actualFunc, Func<IEnumerable<object>, IEnumerable<object>> yourFunc)
        {
            FunctionalityTestRunner(arrayData, actualFunc, yourFunc);
        }

    }
}