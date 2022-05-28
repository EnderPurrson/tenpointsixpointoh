using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DevToDev.Core.Utils.Validators
{
	internal class EmailValidator : IValidator
	{
		private bool invalid;

		public bool IsValid(string data)
		{
			invalid = false;
			return IsValidEmail(data);
		}

		private bool IsValidEmail(string strIn)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			if (string.IsNullOrEmpty(strIn))
			{
				return false;
			}
			try
			{
				strIn = Regex.Replace(strIn, "(@)(.+)$", new MatchEvaluator(DomainMapper), (RegexOptions)0);
			}
			catch (global::System.Exception)
			{
				return false;
			}
			if (invalid)
			{
				return false;
			}
			try
			{
				return Regex.IsMatch(strIn, "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$", (RegexOptions)1);
			}
			catch (global::System.Exception)
			{
				return false;
			}
		}

		private string DomainMapper(Match match)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			IdnMapping val = new IdnMapping();
			string text = ((Capture)match.get_Groups().get_Item(2)).get_Value();
			try
			{
				text = val.GetAscii(text);
			}
			catch (ArgumentException)
			{
				invalid = true;
			}
			return ((Capture)match.get_Groups().get_Item(1)).get_Value() + text;
		}
	}
}
