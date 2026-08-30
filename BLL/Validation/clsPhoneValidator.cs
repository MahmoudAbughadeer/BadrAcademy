

namespace BLL.Validation
{
    public class clsPhoneValidator : IFieldValidator
    {
        public clsValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return clsValidationResult.Empty;

            return clsValidation.IsValidPhone(input) ? clsValidationResult.Valid : clsValidationResult.InvalidPhone;

        }
    }
}
