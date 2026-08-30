

namespace BLL.Validation
{ 
    public class clsRequiredValidator : IFieldValidator
    {
        public clsValidationResult Validate(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? clsValidationResult.Empty : clsValidationResult.Valid;
        }
    }
}
