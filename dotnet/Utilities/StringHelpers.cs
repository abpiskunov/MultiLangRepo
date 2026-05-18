using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities
{
    public static class StringHelpers
    {
        public static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }

        public static string ToSlug(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            string slug = value.ToLowerInvariant();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-").Trim('-');
            slug = Regex.Replace(slug, @"-+", "-");
            return slug;
        }

        public static string ToCamelCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var words = value.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            for (int i = 0; i < words.Length; i++)
            {
                if (i == 0)
                {
                    sb.Append(words[i].ToLowerInvariant());
                }
                else
                {
                    sb.Append(char.ToUpperInvariant(words[i][0]));
                    sb.Append(words[i].Substring(1).ToLowerInvariant());
                }
            }
            return sb.ToString();
        }

        public static string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                return email;
            }

            var parts = email.Split('@');
            var localPart = parts[0];
            if (localPart.Length <= 2)
            {
                return localPart[0] + "***@" + parts[1];
            }

            return localPart[0] + new string('*', localPart.Length - 2) + localPart[localPart.Length - 1] + "@" + parts[1];
        }

        public static int WordCount(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }

            return text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
