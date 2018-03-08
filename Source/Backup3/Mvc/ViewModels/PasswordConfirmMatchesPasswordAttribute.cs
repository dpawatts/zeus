using System.ComponentModel.DataAnnotations;

namespace Zeus.Templates.Mvc.ViewModels
{
	public class PasswordConfirmMatchesPasswordAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			IConfirmPassword typedValue = (IConfirmPassword) value;
			if (typedValue.Password != typedValue.ConfirmPassword)
				return false;
			return true;
		}

		public override string FormatErrorMessage(string name)
		{
			return "Passwords must match";
		}
	}
}