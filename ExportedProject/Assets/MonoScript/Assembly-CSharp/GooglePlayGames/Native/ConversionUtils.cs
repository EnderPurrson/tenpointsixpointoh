using GooglePlayGames.BasicApi;
using GooglePlayGames.Native.Cwrapper;
using System;
using UnityEngine;

namespace GooglePlayGames.Native
{
	internal static class ConversionUtils
	{
		internal static GooglePlayGames.Native.Cwrapper.Types.DataSource AsDataSource(DataSource source)
		{
			DataSource dataSource = source;
			if (dataSource == DataSource.ReadCacheOrNetwork)
			{
				return GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK;
			}
			if (dataSource != DataSource.ReadNetworkOnly)
			{
				throw new InvalidOperationException(string.Concat("Found unhandled DataSource: ", source));
			}
			return GooglePlayGames.Native.Cwrapper.Types.DataSource.NETWORK_ONLY;
		}

		internal static ResponseStatus ConvertResponseStatus(CommonErrorStatus.ResponseStatus status)
		{
			switch (status)
			{
				case CommonErrorStatus.ResponseStatus.ERROR_TIMEOUT:
				{
					return ResponseStatus.Timeout;
				}
				case CommonErrorStatus.ResponseStatus.ERROR_VERSION_UPDATE_REQUIRED:
				{
					return ResponseStatus.VersionUpdateRequired;
				}
				case CommonErrorStatus.ResponseStatus.ERROR_NOT_AUTHORIZED:
				{
					return ResponseStatus.NotAuthorized;
				}
				case CommonErrorStatus.ResponseStatus.ERROR_INTERNAL:
				{
					return ResponseStatus.InternalError;
				}
				case CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED:
				{
					return ResponseStatus.LicenseCheckFailed;
				}
				case 0:
				{
					throw new InvalidOperationException(string.Concat("Unknown status: ", status));
				}
				case CommonErrorStatus.ResponseStatus.VALID:
				{
					return ResponseStatus.Success;
				}
				case CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
				{
					return ResponseStatus.SuccessWithStale;
				}
				default:
				{
					throw new InvalidOperationException(string.Concat("Unknown status: ", status));
				}
			}
		}

		internal static CommonStatusCodes ConvertResponseStatusToCommonStatus(CommonErrorStatus.ResponseStatus status)
		{
			switch (status)
			{
				case CommonErrorStatus.ResponseStatus.ERROR_TIMEOUT:
				{
					return CommonStatusCodes.Timeout;
				}
				case CommonErrorStatus.ResponseStatus.ERROR_VERSION_UPDATE_REQUIRED:
				{
					return CommonStatusCodes.ServiceVersionUpdateRequired;
				}
				case CommonErrorStatus.ResponseStatus.ERROR_NOT_AUTHORIZED:
				{
					return CommonStatusCodes.AuthApiAccessForbidden;
				}
				case CommonErrorStatus.ResponseStatus.ERROR_INTERNAL:
				{
					return CommonStatusCodes.InternalError;
				}
				case CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED:
				{
					return CommonStatusCodes.LicenseCheckFailed;
				}
				case 0:
				{
					Debug.LogWarning(string.Concat("Unknown ResponseStatus: ", status, ", defaulting to CommonStatusCodes.Error"));
					return CommonStatusCodes.Error;
				}
				case CommonErrorStatus.ResponseStatus.VALID:
				{
					return CommonStatusCodes.Success;
				}
				case CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
				{
					return CommonStatusCodes.SuccessCached;
				}
				default:
				{
					Debug.LogWarning(string.Concat("Unknown ResponseStatus: ", status, ", defaulting to CommonStatusCodes.Error"));
					return CommonStatusCodes.Error;
				}
			}
		}

		internal static UIStatus ConvertUIStatus(CommonErrorStatus.UIStatus status)
		{
			switch (status)
			{
				case CommonErrorStatus.UIStatus.ERROR_UI_BUSY:
				{
					return UIStatus.UiBusy;
				}
				case CommonErrorStatus.UIStatus.VALID | CommonErrorStatus.UIStatus.ERROR_UI_BUSY:
				case -10:
				case CommonErrorStatus.UIStatus.VALID | CommonErrorStatus.UIStatus.ERROR_UI_BUSY:
				case -8:
				case -7:
				case CommonErrorStatus.UIStatus.VALID | CommonErrorStatus.UIStatus.ERROR_INTERNAL | CommonErrorStatus.UIStatus.ERROR_NOT_AUTHORIZED | CommonErrorStatus.UIStatus.ERROR_VERSION_UPDATE_REQUIRED | CommonErrorStatus.UIStatus.ERROR_TIMEOUT | CommonErrorStatus.UIStatus.ERROR_CANCELED | CommonErrorStatus.UIStatus.ERROR_UI_BUSY | CommonErrorStatus.UIStatus.ERROR_LEFT_ROOM:
				case 0:
				{
					throw new InvalidOperationException(string.Concat("Unknown status: ", status));
				}
				case CommonErrorStatus.UIStatus.ERROR_CANCELED:
				{
					return UIStatus.UserClosedUI;
				}
				case CommonErrorStatus.UIStatus.ERROR_TIMEOUT:
				{
					return UIStatus.Timeout;
				}
				case CommonErrorStatus.UIStatus.ERROR_VERSION_UPDATE_REQUIRED:
				{
					return UIStatus.VersionUpdateRequired;
				}
				case CommonErrorStatus.UIStatus.ERROR_NOT_AUTHORIZED:
				{
					return UIStatus.NotAuthorized;
				}
				case CommonErrorStatus.UIStatus.ERROR_INTERNAL:
				{
					return UIStatus.InternalError;
				}
				case CommonErrorStatus.UIStatus.VALID:
				{
					return UIStatus.Valid;
				}
				default:
				{
					throw new InvalidOperationException(string.Concat("Unknown status: ", status));
				}
			}
		}
	}
}