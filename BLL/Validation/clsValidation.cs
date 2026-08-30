
using System.Text.RegularExpressions;

namespace BLL.Validation
{
    public class clsValidation
    {
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[\w_.]+(\-?[\w_.])*@(gmail.com|yahoo.com|hotmail.com)$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(email);
        }

        public static bool IsValidPhone(string phone)
        {
            string pattern = @"^\d{3,11}$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(phone);
        }

        public static bool IsValidName(string name)
        {
            string pattern = @"^[\p{L} ]+$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(name);
        }
    }
}
