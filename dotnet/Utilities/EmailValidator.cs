using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Utilities
{
    public static class EmailValidator
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled);

        public static bool IsValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            if (!EmailRegex.IsMatch(email))
            {
                return false;
            }

            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string GetDomain(string email)
        {
            if (!IsValid(email))
            {
                throw new ArgumentException("Invalid email address", nameof(email));
            }

            return email.Split('@')[1].ToLowerInvariant();
        }

        public static bool IsBusinessEmail(string email)
        {
            if (!IsValid(email))
            {
                return false;
            }

            string domain = GetDomain(email);
            string[] freeProviders = { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "aol.com", "live.com" };

            foreach (var provider in freeProviders)
            {
                if (domain.Equals(provider, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
