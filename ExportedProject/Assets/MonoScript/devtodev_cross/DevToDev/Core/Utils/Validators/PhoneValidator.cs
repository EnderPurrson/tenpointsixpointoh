using System;
using System.Text.RegularExpressions;

namespace DevToDev.Core.Utils.Validators
{
	internal class PhoneValidator : IValidator
	{
		public bool IsValid(string data)
		{
			try
			{
				return ((Group)Regex.Match(data, "^(\\+[0-9]+?)$")).get_Success();
			}
			catch (global::System.Exception)
			{
			}
			return false;
		}
	}
}
