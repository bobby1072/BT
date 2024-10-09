using BT.Common.FastArray.Tests.TestModels;
using BT.Common.OperationTimer.Proto;
using FluentAssertions;

namespace BT.Common.FastArray.Tests
{
    public abstract class FastArrayTestBase
    {

        protected static readonly IReadOnlyCollection<IReadOnlyCollection<object>> _basicArraysToTestFunctionality =
        [
            [
                new TestPlane {Color = "Red", Make = "Boeing", Model = "747", Year = 1990, NumberOfEngines = 4, NumberOfWings = 2},
                new TestCar {Color = "Blue", Make = "Toyota", Model = "Camry", Year = 2020, NumberOfDoors = 4, DriveSystem = DriveSystems.AWD},
                new TestCar {Color = "Green", Make = "Toyota", Model = "Gy86", Year = 2010, NumberOfDoors = 3, DriveSystem = DriveSystems.FOURWD},
                new TestPlane {Color = "White", Make = "Airbus", Model = "A320", Year = 2018, NumberOfEngines = 2, NumberOfWings = 2},
                new TestCar {Color = "Black", Make = "Tesla", Model = "Model S", Year = 2022, NumberOfDoors = 4, DriveSystem = DriveSystems.AWD},
                new TestPlane {Color = "Yellow", Make = "Cessna", Model = "172", Year = 2005, NumberOfEngines = 1, NumberOfWings = 2},
                new TestCar {Color = "Red", Make = "Ford", Model = "Mustang", Year = 2015, NumberOfDoors = 2, DriveSystem = DriveSystems.FOURWD}
            ],

            [
                "Pete", "Boi", "Ant", "Kate", "Marie", "Sam", "Ben", "Joseph", "Chris",
                "Alice", "Bob", "Clara", "David", "Eva", "Frank", "Grace", "Hank", "Ivy"
            ],

            [
                1, 32, 435, 435345, 435, 43, 34, 435, 43545, 45, 45, 65, 54, 5, 65, 65,
                6545, 554, 654, 64, 5645, 3654, 67687897, 5435, 4, 654, 65, 3, 556, 68,
                768, 97, 958, 4, 5, 12, 4431, 2556, 988, 543, 342, 2345, 6789, 789
            ],

            [],

            [
                3.14, 1.618, 2.718, 42.42, 0.001, 1000.123, 9.81, -273.15,
                6.022e23, 299792458, 9.109e-31, 1.602e-19, 0.007, 98.76, -40.0, 23.45
            ],

            [
                123, "hello", true, 456.78, "world", false, 789, 0.99,
                321, "bye", false, 654.32, "universe", true, 987, 1.01
            ],

            [
                true, false, false, true, true, false, true,
                false, true, true, false, false, true, false
            ],

            [
                "repeat", "repeat", 42, 42, true, true, false, false,
                "again", "again", 84, 84, false, false, true, true
            ],

            [
                new List<object> { 1, 2, 3 },
                new List<object> { "a", "b", "c" },
                new List<object> { true, false, true },
                new List<object> { 4, 5, 6 },
                new List<object> { "x", "y", "z" },
                new List<object> { false, true, false }
            ],

            [
                null, null, null,
                null, null, null
            ],

            [
                "@", "#", "$", "%", "^", "&", "*", "(", ")",
                "_", "+", "=", "!", "?", "/", "|", "~", "`"
            ],

            [
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
                'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p'
            ],

            [
                Enumerable.Range(1, 100).Cast<object>().ToList(),
                Enumerable.Range(101, 100).Cast<object>().ToList()
            ],

            [
                0x1A, 0xFF, 0xBEEF, 0x10, 0xABCD, 0x1010,
                0xDEAD, 0xCAFE, 0xBABE, 0x7FFF, 0x1001
            ],

            [
                0b0010, 0b1100, 0b1010, 0b1111, 0b0001,
                0b0111, 0b0000, 0b1110, 0b1001, 0b1011
            ],

            [
                new DateTime(2023, 1, 1),
                new DateTime(1990, 12, 25),
                DateTime.Now,
                DateTime.UtcNow,
                new DateTime(1985, 7, 13),
                new DateTime(2000, 10, 1)
            ],

            [
                "", "", "", "",
                "", "", "", ""
            ]
        ];

        protected static IReadOnlyCollection<IReadOnlyCollection<object>> CreateLargeArraysForPerformanceTestAllTypes(int individualArraySize = 5000, int? totalArraysSize = null)
        {
            var result = new object[][]
            {
                [
                    CreateLargeArrayForPerformanceTest<string>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<int>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<double>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<bool>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<char>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<DateTime>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<byte>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<short>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<long>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<float>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<decimal>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<uint>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<ushort>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<ulong>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<sbyte>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<Guid>(individualArraySize),
                    CreateLargeArrayForPerformanceTest<TimeSpan>(individualArraySize)
                ]
            };


            if (totalArraysSize is int foundSize)
            {
                return result.Take(foundSize).ToArray();
            }
            else
            {
                return result;
            }
        }
        protected static IReadOnlyCollection<object> CreateLargeArrayForPerformanceTest<T>(int size)
        {
            var tType = typeof(T);

            if (tType == typeof(string))
            {
                return Enumerable.Range(0, size).Select(x => (object)x.ToString()).ToList();
            }
            else if (tType == typeof(int))
            {
                return Enumerable.Range(0, size).Select(x => (object)x).ToList();
            }
            else if (tType == typeof(double))
            {
                return Enumerable.Range(0, size).Select(x => (object)x).ToList();
            }
            else if (tType == typeof(bool))
            {
                return Enumerable.Range(0, size).Select(x => (object)(x % 2 == 0)).ToList();
            }
            else if (tType == typeof(char))
            {
                return Enumerable.Range(0, size).Select(x => (object)(char)(x % 26 + 65)).ToList();
            }
            else if (tType == typeof(DateTime))
            {
                return Enumerable.Range(0, size).Select(x => (object)new DateTime(2023, 1, 1)).ToList();
            }
            else if (tType == typeof(byte))
            {
                return Enumerable.Range(0, size).Select(x => (object)(byte)(x % 256)).ToList();
            }
            else if (tType == typeof(short))
            {
                return Enumerable.Range(0, size).Select(x => (object)(short)(x % 1000)).ToList();
            }
            else if (tType == typeof(long))
            {
                return Enumerable.Range(0, size).Select(x => (object)(long)(x % 1000)).ToList();
            }
            else if (tType == typeof(float))
            {
                return Enumerable.Range(0, size).Select(x => (object)(float)(x % 1000)).ToList();
            }
            else if (tType == typeof(decimal))
            {
                return Enumerable.Range(0, size).Select(x => (object)(decimal)(x % 1000)).ToList();
            }
            else if (tType == typeof(uint))
            {
                return Enumerable.Range(0, size).Select(x => (object)(uint)(x % 1000)).ToList();
            }
            else if (tType == typeof(ushort))
            {
                return Enumerable.Range(0, size).Select(x => (object)(ushort)(x % 1000)).ToList();
            }
            else if (tType == typeof(ulong))
            {
                return Enumerable.Range(0, size).Select(x => (object)(ulong)(x % 1000)).ToList();
            }
            else if (tType == typeof(sbyte))
            {
                return Enumerable.Range(0, size).Select(x => (object)(sbyte)(x % 1000)).ToList();
            }
            else if (tType == typeof(Guid))
            {
                return Enumerable.Range(0, size).Select(x => (object)Guid.NewGuid()).ToList();
            }
            else if (tType == typeof(TimeSpan))
            {
                return Enumerable.Range(0, size).Select(x => (object)TimeSpan.FromMilliseconds(x)).ToList();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        protected static void FunctionalityTestRunner<T>(IEnumerable<T> arrayData, Func<IEnumerable<T>, IEnumerable<T>> actualFunc, Func<IEnumerable<T>, IEnumerable<T>> yourFunc)
        {
            var expected = actualFunc.Invoke(arrayData);
            var yourResult = yourFunc.Invoke(arrayData);

            if(expected.Count() != yourResult.Count())
            {
                Console.WriteLine(expected.Count());
                expected = actualFunc.Invoke(arrayData);
                yourResult = yourFunc.Invoke(arrayData);
            }

            expected.Count().Should().Be(yourResult.Count());
            for (int i = 0; i < expected.Count(); i++)
            {
                yourResult.ElementAt(i)?.Should().BeOfType(expected.ElementAt(i)?.GetType());
                yourResult.ElementAt(i)?.Should().Be(expected.ElementAt(i));
            }
        }
        protected static void PerformanceTestRunner<T>(IEnumerable<T> arrayData, Func<IEnumerable<T>, IEnumerable<T>> actualFunc, Func<IEnumerable<T>, IEnumerable<T>> yourFunc)
        {
            var actualTime = OperationTimerUtils.Time(actualFunc, arrayData);
            var yourTime = OperationTimerUtils.Time(yourFunc, arrayData);

            yourTime.Should().BeLessThanOrEqualTo(actualTime);
        }
    }
}
