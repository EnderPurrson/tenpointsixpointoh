using System.Security.Cryptography;
using System.Text;

namespace DevToDev.Core.Utils
{
	internal class MD5Helper
	{
		internal byte[] ComputeHash(byte[] bs)
		{
			return ((HashAlgorithm)new MD5Managed()).ComputeHash(bs);
		}

		public static string GetMd5(byte[] bs)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			if (bs != null)
			{
				MD5Helper mD5Helper = new MD5Helper();
				byte[] array = mD5Helper.ComputeHash(bs);
				StringBuilder val = new StringBuilder();
				byte[] array2 = array;
				foreach (byte b in array2)
				{
					val.Append(b.ToString("x2").ToLower());
				}
				return ((object)val).ToString();
			}
			return "";
		}
	}
}
