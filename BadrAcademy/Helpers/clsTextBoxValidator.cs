using BLL.Validation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace BadrAcademy.Helpers
{

    public class clsTextBoxValidator
    {
        private readonly ErrorProvider _errorProvider;
        private readonly Dictionary<TextBox, IFieldValidator> _validators;


        //Private Methods

        private void TextBox_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (!_validators.TryGetValue(txt, out IFieldValidator validator))
                return;

            clsValidationResult result = validator.Validate(txt.Text.Trim());
            _errorProvider.SetError(txt, GetErrorMessage(result));
        }

        private string GetErrorMessage(clsValidationResult result)
        {
            switch (result)
            {
                case clsValidationResult.Valid:
                    return "";
                case clsValidationResult.Empty:
                    return "هذا الحقل مطلوب";
                case clsValidationResult.AlreadyExists:
                    return "اسم المستخدم موجود بالفعل";
                case clsValidationResult.InvalidEmail:
                    return "بريد إلكترونى غير صالح";
                case clsValidationResult.InvalidName:
                    return "الاسم يجب أن يحتوي على أحرف ومسافات فقط";
                case clsValidationResult.InvalidPhone:
                    return "رقم الهاتف غير صالح";
                case clsValidationResult.InvalidPassword:
                    return "رقم غير غير صالح";
                default:
                    return "بيانات غير صحيحة";
            }
        }


        //Public Methods

        public clsTextBoxValidator(ErrorProvider errorProvider)
        {
            _errorProvider = errorProvider;
            _validators = new Dictionary<TextBox, IFieldValidator>();
        }

        public void Clear()
        {
            foreach (var kvp in _validators)
            {
                kvp.Key.Validating -= TextBox_Validating;
            }
            _validators.Clear();
        }
        public void Add(TextBox textBox, IFieldValidator validator)
        {
            _validators[textBox] = validator;
            textBox.Validating += TextBox_Validating;
        }


        public bool AreAllFieldsValid()
        {
            bool areAllValid = true;

            foreach(var kvp in _validators)
            {
                TextBox txt = kvp.Key;
                IFieldValidator validator = kvp.Value;
                clsValidationResult result = validator.Validate(txt.Text.Trim());
                _errorProvider.SetError(txt, GetErrorMessage(result));

                if (result != clsValidationResult.Valid)
                    areAllValid = false;
            }

            return areAllValid;
        }
    }
}
