namespace BT.Common.OperationTimer.Tests
{
    public class OperationTimerTestBase
    {
        protected static class TestFuncsWithData
        {
            public static IDictionary<IReadOnlyCollection<object>, Func<object, object>> BuildTestDataAndFunctions()
            {
                var dict = new Dictionary<IReadOnlyCollection<object>, Func<object, object>>();
                return dict;
            }
            private static string ReverseString(string input)
            {
                if (string.IsNullOrEmpty(input)) return input;
                return new string(input.Reverse().ToArray());
            }

            private static int FindMax(IEnumerable<int> numbers)
            {
                if (!numbers.Any()) throw new ArgumentException("List cannot be empty");
                return numbers.Max();
            }

            private static string ConvertToCommaSeparated(IEnumerable<string> strings)
            {
                return string.Join(",", strings);
            }

            private static int CountCharacterOccurrences(string input, char character)
            {
                return input.Count(c => c == character);
            }

            private static int SumIntegers(IEnumerable<int> numbers)
            {
                return numbers.Sum();
            }

            private static bool AreAllEven(IEnumerable<int> numbers)
            {
                return numbers.All(n => n % 2 == 0);
            }

            private static string GetFirstNCharacters(string input, int n)
            {
                if (n > input.Length) return input;
                return input.Substring(0, n);
            }

            private static IEnumerable<string> FilterByLength(IEnumerable<string> strings, int minLength)
            {
                return strings.Where(s => s.Length >= minLength);
            }

            private static string CapitalizeFirstLetter(string input)
            {
                if (string.IsNullOrEmpty(input)) return input;
                return char.ToUpper(input[0]) + input.Substring(1);
            }

            private static IEnumerable<T> GetDistinctElements<T>(IEnumerable<T> elements)
            {
                return elements.Distinct();
            }
        }
    }
}