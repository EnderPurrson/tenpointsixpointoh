using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.Cwrapper
{
	internal static class SnapshotMetadataChangeBuilder
	{
		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr SnapshotMetadataChange_Builder_Construct();

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern IntPtr SnapshotMetadataChange_Builder_Create(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void SnapshotMetadataChange_Builder_Dispose(HandleRef self);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void SnapshotMetadataChange_Builder_SetCoverImageFromPngData(HandleRef self, byte[] png_data, UIntPtr png_data_size);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void SnapshotMetadataChange_Builder_SetDescription(HandleRef self, string description);

		[DllImport("gpg", CharSet=CharSet.None, ExactSpelling=false)]
		internal static extern void SnapshotMetadataChange_Builder_SetPlayedTime(HandleRef self, ulong played_time);
	}
}