using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace WpfApp1
{
    public static class UserValidation
    {
        public static void ValidateAge(int age)
        {
            if (age < 0 || age > 135)
            {
                throw new InvalidAgeException("Invalid age. Age cannot be less than 0 or greater than or equal to 135.");
            }
        }

        public static void ValidateEmailAddress(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidEmailException("Email address cannot be empty.");
            }

            try
            {
                var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                if (!emailRegex.IsMatch(email))
                {
                    throw new InvalidEmailException("Invalid email format.");
                }
            }
            catch (RegexMatchTimeoutException)
            {
                throw new InvalidEmailException("Regex match timeout.");
            }
        }

        public static void EndsWithRu(string email)
        {
            if (email.EndsWith(".ru", StringComparison.OrdinalIgnoreCase))
            {
                throw new EmailEndsWithRuException("Email address should not end with '.ru'.");
            }
        }
    }

    public class InvalidAgeException : Exception
    {
        public InvalidAgeException() { }
        public InvalidAgeException(string message) : base(message) { }
        public InvalidAgeException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InvalidEmailException : Exception
    {
        public InvalidEmailException() { }
        public InvalidEmailException(string message) : base(message) { }
        public InvalidEmailException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class EmailEndsWithRuException : Exception
    {
        public EmailEndsWithRuException() { }
        public EmailEndsWithRuException(string message) : base(message) { }
        public EmailEndsWithRuException(string message, Exception innerException) : base(message, innerException) { }
    }
}
