
namespace BLL.Validation
{
    public class clsNameValidator : IFieldValidator
    {
        public clsValidationResult Validate(string input)
        {
            if (string.IsNullOrEmpty(input))
                return clsValidationResult.Empty;
            
            return clsValidation.IsValidName(input)? clsValidationResult.Valid : clsValidationResult.InvalidName;
        }
    }
}
