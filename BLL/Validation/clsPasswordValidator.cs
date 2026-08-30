
using BLL.Utilities;
using System.Threading.Tasks;

namespace BLL.Validation
{ 
    public class clsPasswordValidator : IFieldValidator
    {
        clsUser _currentUser;
        public clsPasswordValidator(clsUser currentUser = null)
        {
            _currentUser = currentUser;
        }



        public clsValidationResult Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return clsValidationResult.Empty;


            //This line to call the asyna method withn sync method without block the UI
            string hashedPassword = Task.Run(() => clsUser.GetHashPasswordAsync(_currentUser.UserID.Value)).GetAwaiter().GetResult();
            string newHashedPassword = clsHasher.Hash(input);

            if (newHashedPassword != hashedPassword)
                return clsValidationResult.InvalidPassword;

            return clsValidationResult.Valid;
        }
    }
}
