
namespace BLL.Validation
{
    public class clsEmialValidator : IFieldValidator
    {
        public clsValidationResult Validate(string input)
        {
            if (string.IsNullOrEmpty(input))
                return clsValidationResult.Valid;

            return clsValidation.IsValidEmail(input) ? clsValidationResult.Valid : clsValidationResult.InvalidEmail;
        }
    }
}
