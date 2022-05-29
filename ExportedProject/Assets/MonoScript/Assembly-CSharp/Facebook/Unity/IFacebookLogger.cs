using System;

namespace Facebook.Unity
{
	internal interface IFacebookLogger
	{
		void Error(string msg);

		void Info(string msg);

		void Log(string msg);

		void Warn(string msg);
	}
}