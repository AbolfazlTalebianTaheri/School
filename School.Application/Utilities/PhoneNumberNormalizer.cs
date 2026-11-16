using System.Text;
using System.Text.RegularExpressions;

namespace School.Application.Utilities
{
    public static class PhoneNumberNormalizer
    {
        private static readonly Regex IranMobile = new(@"^09\d{9}$", RegexOptions.Compiled);

        /// <summary>ارقام فارسی/عربی را به لاتین تبدیل و فاصله و خط‌تیره را حذف می‌کند.</summary>
        public static string NormalizeDigits(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            var sb = new StringBuilder(input.Length);
            foreach (var c in input.Trim())
            {
                if (c is >= '0' and <= '9' or '+') sb.Append(c);
                else if (c is >= '\u06F0' and <= '\u06F9') sb.Append((char)('0' + (c - '\u06F0')));
                else if (c is >= '\u0660' and <= '\u0669') sb.Append((char)('0' + (c - '\u0660')));
                else if (c is ' ' or '-' or '\u200c' or '\u200f') continue;
                else sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>انواع +98912…، 0098912…، 98912…، 912… را به 0912… تبدیل می‌کند؛ نامعتبر = null.</summary>
        public static string? NormalizeIranMobile(string? input)
        {
            var s = NormalizeDigits(input);
            if (s.StartsWith("+98")) s = "0" + s[3..];
            else if (s.StartsWith("0098")) s = "0" + s[4..];
            else if (s.StartsWith("98") && s.Length == 12) s = "0" + s[2..];
            else if (s.StartsWith('9') && s.Length == 10) s = "0" + s;
            return IranMobile.IsMatch(s) ? s : null;
        }
    }
}
