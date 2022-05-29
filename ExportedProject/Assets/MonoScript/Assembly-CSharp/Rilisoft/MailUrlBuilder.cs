using System;
using System.Text;

namespace Rilisoft
{
	public class MailUrlBuilder
	{
		public string to;

		public string subject;

		public string body;

		public MailUrlBuilder()
		{
		}

		public string GetUrl()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("mailto:{0}", this.to);
			string str = Uri.EscapeUriString(this.subject);
			stringBuilder.AppendFormat("?subject={0}", str);
			string str1 = Uri.EscapeUriString(this.body);
			stringBuilder.AppendFormat("&body={0}", str1);
			return stringBuilder.ToString();
		}
	}
}