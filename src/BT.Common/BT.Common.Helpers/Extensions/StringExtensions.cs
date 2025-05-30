using System.Net.Mail;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BT.Common.Helpers.Extensions
{
    public static partial class StringExtensions
    {
        public static bool IsValidJson(this string json)
        {
            try
            {
                JsonDocument.Parse(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public static string Join(this IEnumerable<string> values, string separator)
        {
            return string.Join(separator, values);
        }
        public static string TrimBase64String(this string input)
        {
            return Base64TrimRegex().Replace(input, string.Empty);
        }
        public static bool IsValidEmail(this string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsJustNumbers(this string input)
        {
            return input.All(char.IsDigit);
        }

        public static bool IsJustLetters(this string input)
        {
            return input.All(char.IsLetter);
        }

        public static bool IsJustSpaces(this string input)
        {
            return input.All(char.IsWhiteSpace);
        }

        [GeneratedRegex(@"^data:image\/[^;]+;base64,")]
        private static partial Regex Base64TrimRegex();
    }
}
