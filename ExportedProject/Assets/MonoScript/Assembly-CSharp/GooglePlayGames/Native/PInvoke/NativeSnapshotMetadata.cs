using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeSnapshotMetadata : BaseReferenceHolder, ISavedGameMetadata
	{
		public bool IsOpen
		{
			get
			{
				return SnapshotMetadata.SnapshotMetadata_IsOpen(SelfPtr());
			}
		}

		public string Filename
		{
			get
			{
				return PInvokeUtilities.OutParamsToString(_003Cget_Filename_003Em__153);
			}
		}

		public string Description
		{
			get
			{
				return PInvokeUtilities.OutParamsToString(_003Cget_Description_003Em__154);
			}
		}

		public string CoverImageURL
		{
			get
			{
				return PInvokeUtilities.OutParamsToString(_003Cget_CoverImageURL_003Em__155);
			}
		}

		public TimeSpan TotalTimePlayed
		{
			get
			{
				long num = SnapshotMetadata.SnapshotMetadata_PlayedTime(SelfPtr());
				if (num < 0)
				{
					return TimeSpan.FromMilliseconds(0.0);
				}
				return TimeSpan.FromMilliseconds(num);
			}
		}

		public DateTime LastModifiedTimestamp
		{
			get
			{
				return PInvokeUtilities.FromMillisSinceUnixEpoch(SnapshotMetadata.SnapshotMetadata_LastModifiedTime(SelfPtr()));
			}
		}

		internal NativeSnapshotMetadata(IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		public override string ToString()
		{
			if (IsDisposed())
			{
				return "[NativeSnapshotMetadata: DELETED]";
			}
			return string.Format("[NativeSnapshotMetadata: IsOpen={0}, Filename={1}, Description={2}, CoverImageUrl={3}, TotalTimePlayed={4}, LastModifiedTimestamp={5}]", IsOpen, Filename, Description, CoverImageURL, TotalTimePlayed, LastModifiedTimestamp);
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			SnapshotMetadata.SnapshotMetadata_Dispose(SelfPtr());
		}

		[CompilerGenerated]
		private UIntPtr _003Cget_Filename_003Em__153(StringBuilder out_string, UIntPtr out_size)
		{
			return SnapshotMetadata.SnapshotMetadata_FileName(SelfPtr(), out_string, out_size);
		}

		[CompilerGenerated]
		private UIntPtr _003Cget_Description_003Em__154(StringBuilder out_string, UIntPtr out_size)
		{
			return SnapshotMetadata.SnapshotMetadata_Description(SelfPtr(), out_string, out_size);
		}

		[CompilerGenerated]
		private UIntPtr _003Cget_CoverImageURL_003Em__155(StringBuilder out_string, UIntPtr out_size)
		{
			return SnapshotMetadata.SnapshotMetadata_CoverImageURL(SelfPtr(), out_string, out_size);
		}
	}
}
