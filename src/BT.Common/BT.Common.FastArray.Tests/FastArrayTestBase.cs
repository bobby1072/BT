
using BT.Common.FastArray.Tests.Models;
using FluentAssertions;

namespace BT.Common.FastArray.Tests
{
    public abstract class FastArrayTestBase
    {

        protected static readonly IReadOnlyCollection<IReadOnlyCollection<object>> _basicArraysToTestFunctionality =
        [
            [
                new TestPlane {Color = "Red", Make = "Boeing", Model = "747", Year = 1990, NumberOfEngines = 4, NumberOfWings = 2}, new TestCar {Color = "Blue", Make = "Toyota", Model = "Camry", Year = 2020, NumberOfDoors = 4, DriveSystem=  DriveSystems.AWD},
                new TestCar {Color = "green", Make = "Toyota", Model = "gy86", Year = 2010, NumberOfDoors = 3, DriveSystem=  DriveSystems.FOURWD}
            ],
        [
            "pete", "boi", "ant", "kate", "marie", "sam", "ben", "joseph", "chris"
        ],

        [
            1, 32, 435, 435345, 435, 43, 34, 435, 43545, 45, 45, 65, 54, 5, 65, 65,
            6545, 554, 654, 64, 5645, 3654, 67687897, 5435, 4, 654, 65, 3, 556, 68,
            768, 97, 958, 4, 5, 12, 4431, 2556
        ],

        [],

        [
            3.14, 1.618, 2.718, 42.42, 0.001, 1000.123, 9.81, -273.15
        ],

        [
            123, "hello", true, 456.78, "world", false, 789, 0.99
        ],

        [
            true, false, false, true, true, false, true
        ],

        [
            "repeat", "repeat", 42, 42, true, true, false, false
        ],

        [
            new List<object> { 1, 2, 3 },
            new List<object> { "a", "b", "c" },
            new List<object> { true, false, true }
        ],

        [
            null, null, null
        ],

        [
            "@", "#", "$", "%", "^", "&", "*", "(", ")"
        ],

        [
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'
        ],

        [
            Enumerable.Range(1, 100).Cast<object>().ToList()
        ],

        [
            0x1A, 0xFF, 0xBEEF, 0x10, 0xABCD, 0x1010
        ],

        [
            0b0010, 0b1100, 0b1010, 0b1111, 0b0001
        ],

        [
            new DateTime(2023, 1, 1),
            new DateTime(1990, 12, 25),
            DateTime.Now,
            DateTime.UtcNow
        ],

        [
            "", "", "", ""
        ],

        ];
        protected static IReadOnlyCollection<T> CreateLargeArrayForPerformanceTest<T>(int size)
        {
            var tType = typeof(T);

            if (tType == typeof(string))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)x.ToString()).ToList();
            }
            else if (tType == typeof(int))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)x).ToList();
            }
            else if (tType == typeof(double))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)x).ToList();
            }
            else if (tType == typeof(bool))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)(x % 2 == 0)).ToList();
            }
            else if (tType == typeof(char))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)(char)(x % 26 + 65)).ToList();
            }
            else if (tType == typeof(DateTime))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)new DateTime(2023, 1, 1)).ToList();
            }
            else if (tType == typeof(byte))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)(byte)(x % 256)).ToList();
            }
            else if (tType == typeof(short))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)(short)(x % 1000)).ToList();
            }
            else if (tType == typeof(long))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)(long)(x % 1000)).ToList();
            }
            else if (tType == typeof(float))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)(float)(x % 1000)).ToList();
            }
            else if (tType == typeof(decimal))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)(decimal)(x % 1000)).ToList();
            }
            else if (tType == typeof(uint))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)(uint)(x % 1000)).ToList();
            }
            else if (tType == typeof(ushort))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)(ushort)(x % 1000)).ToList();
            }
            else if (tType == typeof(ulong))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)(ulong)(x % 1000)).ToList();
            }
            else if (tType == typeof(sbyte))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)(sbyte)(x % 1000)).ToList();
            }
            else if (tType == typeof(Guid))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)Guid.NewGuid()).ToList();
            }
            else if (tType == typeof(TimeSpan))
            {
                return Enumerable.Range(0, size).Select(x => (T)(object)TimeSpan.FromMilliseconds(x)).ToList();
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

            expected.Count().Should().Be(yourResult.Count());
            for (int i = 0; i < expected.Count(); i++)
            {
                if(expected.ElementAt(i)?.GetType() != yourResult.ElementAt(i)?.GetType())
                {
                    Console.WriteLine("lololo");
                }
                yourResult.ElementAt(i)?.Should().BeOfType(expected.ElementAt(i)?.GetType());
                yourResult.ElementAt(i)?.Should().Be(expected.ElementAt(i));
            }
        }
    }
}
