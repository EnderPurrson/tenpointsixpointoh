using System.Runtime.CompilerServices;

namespace FyberPlugin
{
	public class BannerSize
	{
		public const int FULL_WIDTH = -1;

		public const int AUTO_HEIGHT = -2;

		public int height
		{
			[CompilerGenerated]
			get
			{
				return _003Cheight_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003Cheight_003Ek__BackingField = value;
			}
		}

		public int width
		{
			[CompilerGenerated]
			get
			{
				return _003Cwidth_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003Cwidth_003Ek__BackingField = value;
			}
		}

		public BannerSizeOrientation orientation
		{
			[CompilerGenerated]
			get
			{
				return _003Corientation_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003Corientation_003Ek__BackingField = value;
			}
		}

		public BannerSize(int width, int height)
		{
			orientation = BannerSizeOrientation.None;
			this.height = height;
			this.width = width;
		}

		public int GetWidth()
		{
			return width;
		}

		public int GetHeight()
		{
			return height;
		}

		public BannerSizeOrientation GetOrientation()
		{
			return orientation;
		}

		public BannerSize WithOrientation(BannerSizeOrientation orientation)
		{
			this.orientation = orientation;
			return this;
		}

		public override string ToString()
		{
			string text = ((width == -1) ? "full_width " : ((width != -2) ? width.ToString() : "smart_width "));
			string text2 = ((height == -1) ? " full_height" : ((height != -2) ? height.ToString() : " smart_height"));
			return string.Concat(new string[6]
			{
				"(",
				text,
				"x",
				text2,
				")",
				(orientation == BannerSizeOrientation.None) ? string.Empty : string.Concat((object)" - Orientation:", (object)orientation)
			});
		}
	}
}
