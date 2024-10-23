using BT.Common.FastArray.Proto;
using BT.Common.FastArray.Tests.TestModels;

namespace BT.Common.FastArray.Tests.ProtoTests
{
    public class FastArraySelectTests : FastArrayTestBase
    {
        private class FastArraySelectTests_Functionality_Class_Data : TheoryData<IReadOnlyCollection<object>, Func<IEnumerable<object>, IEnumerable<object>>, Func<IEnumerable<object>, IEnumerable<object>>>
        {
            public FastArraySelectTests_Functionality_Class_Data()
            {
                Add(new List<object> { 1, 2, 3 }, x => x.Select(y => y), x => x.FastArraySelect(y => y));

                foreach (var arrayData in _basicArraysToTestFunctionality)
                {
                    Add(arrayData, x => x.Select(y => y), x => x.FastArraySelect(y => y));
                }
            }
        }
        private class FastArraySelectAsyncTests_Functionality_Class_Data : TheoryData<IReadOnlyCollection<object>, Func<IEnumerable<object>, IEnumerable<Task<object>>>, Func<IEnumerable<object>, IEnumerable<Task<object>>>>
        {
            public FastArraySelectAsyncTests_Functionality_Class_Data()
            {
                Add(new List<object> { 1, 2, 3 }, x => x.Select(y => Task.FromResult(y)), x => x.FastArraySelect(y => Task.FromResult(y)));

                foreach (var arrayData in _basicArraysToTestFunctionality)
                {
                    Add(arrayData, x => x.Select(y => Task.FromResult(y)), x => x.FastArraySelect(y => Task.FromResult(y)));

                    Add(arrayData, x => x.Select(async y => y), x => x.FastArraySelect(async y => y));

                    Add(arrayData, x => x.Select(async y => {
                        await Task.Delay(1);
                        return y;
                    }), x => x.FastArraySelect(async y => {
                        await Task.Delay(1);
                        return y;
                    }));
                }
            }
        }
        [Theory]
        [ClassData(typeof(FastArraySelectTests_Functionality_Class_Data))]
        public void FastArraySelectTests_Functionality(IReadOnlyCollection<object> arrayData, Func<IEnumerable<object>, IEnumerable<object>> actualFunc, Func<IEnumerable<object>, IEnumerable<object>> yourFunc)
        {

            FunctionalityTestRunner(arrayData, actualFunc, yourFunc);
        }
        [Theory]
        [ClassData(typeof(FastArraySelectAsyncTests_Functionality_Class_Data))]
        public async Task FastArraySelectAsyncTests_Functionality(IReadOnlyCollection<object> arrayData, Func<IEnumerable<object>, IEnumerable<Task<object>>> actualFunc, Func<IEnumerable<object>, IEnumerable<Task<object>>> yourFunc)
        {

            await FunctionalityTestRunner(arrayData, actualFunc, yourFunc);
        }
        private class FastArraySelectWhereTests_Functionality_Class_Data : TheoryData<IReadOnlyCollection<object>, Func<IEnumerable<object>, IEnumerable<object>>, Func<IEnumerable<object>, IEnumerable<object>>>
        {
            public FastArraySelectWhereTests_Functionality_Class_Data()
            {
                Add(new List<object> { 1, 2, 3 }, x => x.Select(y => y).Where(x => x is int), x => x.FastArraySelectWhere(x => x is int, x => x));

                foreach (var arrayData in _basicArraysToTestFunctionality)
                {
                    Add(arrayData, x => x.Select(x => x).Where(y => y is not null), x => x.FastArraySelectWhere(y => y is not null, y => y));
                    Add(arrayData, x => x.Select(x => x).Where(y => y is true), x => x.FastArraySelectWhere(y => y is true, y => y));
                    Add(arrayData, x => x.Select(x => x).Where(y => y is false), x => x.FastArraySelectWhere(y => y is false, y => y));
                    Add(arrayData, x => x.Select(x => x).Where(y => y is int), x => x.FastArraySelectWhere(y => y is int, y => y));
                    Add(arrayData, x => x.Select(x => x).Where(y => y is int && (int)y > 100), x => x.FastArraySelectWhere(y => y is int && (int)y > 100, y => y));
                    Add(arrayData, x => x.Select(x => x).Where(y => y is int && (int)y > 1000), x => x.FastArraySelectWhere(y => y is int && (int)y > 1000, y => y));
                    Add(arrayData, x => x.Select(x => x).Where(y => y is string), x => x.FastArraySelectWhere(y => y is string, y => y));
                    Add(arrayData, x => x.Select(x => x).Where(y => y is TestVehicle), x => x.FastArraySelectWhere(y => y is TestVehicle, y => y));
                    Add(arrayData, x => x.Select(x => x).Where(y => y is TestCar), x => x.FastArraySelectWhere(y => y is TestCar, y => y));
                    Add(arrayData, x => x.Select(x => x).Where(y => y is TestPlane), x => x.FastArraySelectWhere(y => y is TestPlane, y => y));

                }
            }
        }
        [Theory]
        [ClassData(typeof(FastArraySelectWhereTests_Functionality_Class_Data))]
        public void FastArraySelectWhereTests_Functionality(IReadOnlyCollection<object> arrayData, Func<IEnumerable<object>, IEnumerable<object>> actualFunc, Func<IEnumerable<object>, IEnumerable<object>> yourFunc)
        {
            FunctionalityTestRunner(arrayData, actualFunc, yourFunc);
        }
    }
}