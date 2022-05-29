using System;

namespace Rilisoft
{
	internal interface IMarketProduct
	{
		string Description
		{
			get;
		}

		string Id
		{
			get;
		}

		object PlatformProduct
		{
			get;
		}

		string Price
		{
			get;
		}

		string Title
		{
			get;
		}
	}
}