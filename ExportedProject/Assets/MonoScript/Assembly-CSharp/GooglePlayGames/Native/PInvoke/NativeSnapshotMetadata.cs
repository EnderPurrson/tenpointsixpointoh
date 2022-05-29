using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeSnapshotMetadata : BaseReferenceHolder, ISavedGameMetadata
	{
		public string CoverImageURL
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => SnapshotMetadata.SnapshotMetadata_CoverImageURL(base.SelfPtr(), out_string, out_size));
			}
		}

		public string Description
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => SnapshotMetadata.SnapshotMetadata_Description(base.SelfPtr(), out_string, out_size));
			}
		}

		public string Filename
		{
			get
			{
				return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => SnapshotMetadata.SnapshotMetadata_FileName(base.SelfPtr(), out_string, out_size));
			}
		}

		public bool IsOpen
		{
			get
			{
				return SnapshotMetadata.SnapshotMetadata_IsOpen(base.SelfPtr());
			}
		}

		public DateTime LastModifiedTimestamp
		{
			get
			{
				return PInvokeUtilities.FromMillisSinceUnixEpoch(SnapshotMetadata.SnapshotMetadata_LastModifiedTime(base.SelfPtr()));
			}
		}

		public TimeSpan TotalTimePlayed
		{
			get
			{
				long num = SnapshotMetadata.SnapshotMetadata_PlayedTime(base.SelfPtr());
				if (num < (long)0)
				{
					return TimeSpan.FromMilliseconds(0);
				}
				return TimeSpan.FromMilliseconds((double)num);
			}
		}

		internal NativeSnapshotMetadata(IntPtr selfPointer) : base(selfPointer)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			SnapshotMetadata.SnapshotMetadata_Dispose(base.SelfPtr());
		}

		public override string ToString()
		{
			if (base.IsDisposed())
			{
				return "[NativeSnapshotMetadata: DELETED]";
			}
			return string.Format("[NativeSnapshotMetadata: IsOpen={0}, Filename={1}, Description={2}, CoverImageUrl={3}, TotalTimePlayed={4}, LastModifiedTimestamp={5}]", new object[] { this.IsOpen, this.Filename, this.Description, this.CoverImageURL, this.TotalTimePlayed, this.LastModifiedTimestamp });
		}
	}
}