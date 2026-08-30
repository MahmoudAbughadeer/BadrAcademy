
using System.Threading.Tasks;

namespace BLL.Validation
{
    public class clsUsernameValidator : IFieldValidator
    {
        clsUser _currentUser;
        public clsUsernameValidator(clsUser currentUser = null)
        {
            _currentUser = currentUser;
        }



        public clsValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return clsValidationResult.Empty;

            if (_currentUser != null && input == _currentUser.Username)
                return clsValidationResult.Valid;


            //This line to call the asyna method withn sync method without block the UI
            bool exists = Task.Run(() => clsUser.IsExistsAsync(input)).GetAwaiter().GetResult();

            return exists ? clsValidationResult.AlreadyExists : clsValidationResult.Valid;
        }
    }
}
