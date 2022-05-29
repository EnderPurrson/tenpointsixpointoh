using System;
using System.Collections.Generic;
using UnityEngine;

public class EveryplayLegacy : MonoBehaviour
{
	public EveryplayLegacy()
	{
	}

	public string AccessToken()
	{
		return Everyplay.AccessToken();
	}

	public float FaceCamAudioPeakLevel()
	{
		return Everyplay.FaceCamAudioPeakLevel();
	}

	public float FaceCamAudioPowerLevel()
	{
		return Everyplay.FaceCamAudioPowerLevel();
	}

	public bool FaceCamIsAudioRecordingSupported()
	{
		return Everyplay.FaceCamIsAudioRecordingSupported();
	}

	public bool FaceCamIsHeadphonesPluggedIn()
	{
		return Everyplay.FaceCamIsHeadphonesPluggedIn();
	}

	public bool FaceCamIsRecordingPermissionGranted()
	{
		return Everyplay.FaceCamIsRecordingPermissionGranted();
	}

	public bool FaceCamIsSessionRunning()
	{
		return Everyplay.FaceCamIsSessionRunning();
	}

	public bool FaceCamIsVideoRecordingSupported()
	{
		return Everyplay.FaceCamIsVideoRecordingSupported();
	}

	public void FaceCamRequestRecordingPermission()
	{
		Everyplay.FaceCamRequestRecordingPermission();
	}

	public void FaceCamSetAudioOnly(bool audioOnly)
	{
		Everyplay.FaceCamSetAudioOnly(audioOnly);
	}

	public void FaceCamSetMonitorAudioLevels(bool enabled)
	{
		Everyplay.FaceCamSetMonitorAudioLevels(enabled);
	}

	public void FaceCamSetPreviewBorderColor(float r, float g, float b, float a)
	{
		Everyplay.FaceCamSetPreviewBorderColor(r, g, b, a);
	}

	public void FaceCamSetPreviewBorderWidth(int width)
	{
		Everyplay.FaceCamSetPreviewBorderWidth(width);
	}

	public void FaceCamSetPreviewOrigin(Everyplay.FaceCamPreviewOrigin origin)
	{
		Everyplay.FaceCamSetPreviewOrigin(origin);
	}

	public void FaceCamSetPreviewPositionX(int x)
	{
		Everyplay.FaceCamSetPreviewPositionX(x);
	}

	public void FaceCamSetPreviewPositionY(int y)
	{
		Everyplay.FaceCamSetPreviewPositionY(y);
	}

	public void FaceCamSetPreviewScaleRetina(bool autoScale)
	{
		Everyplay.FaceCamSetPreviewScaleRetina(autoScale);
	}

	public void FaceCamSetPreviewSideWidth(int width)
	{
		Everyplay.FaceCamSetPreviewSideWidth(width);
	}

	public void FaceCamSetPreviewVisible(bool visible)
	{
		Everyplay.FaceCamSetPreviewVisible(visible);
	}

	public void FaceCamSetTargetTextureHeight(int textureHeight)
	{
		Everyplay.FaceCamSetTargetTextureHeight(textureHeight);
	}

	public void FaceCamSetTargetTextureId(int textureId)
	{
		Everyplay.FaceCamSetTargetTextureId(textureId);
	}

	public void FaceCamSetTargetTextureWidth(int textureWidth)
	{
		Everyplay.FaceCamSetTargetTextureWidth(textureWidth);
	}

	public void FaceCamStartSession()
	{
		Everyplay.FaceCamStartSession();
	}

	public void FaceCamStopSession()
	{
		Everyplay.FaceCamStopSession();
	}

	public int GetUserInterfaceIdiom()
	{
		return Everyplay.GetUserInterfaceIdiom();
	}

	public bool IsPaused()
	{
		return Everyplay.IsPaused();
	}

	public bool IsRecording()
	{
		return Everyplay.IsRecording();
	}

	public bool IsRecordingSupported()
	{
		return Everyplay.IsRecordingSupported();
	}

	public bool IsSingleCoreDevice()
	{
		return Everyplay.IsSingleCoreDevice();
	}

	public bool IsSupported()
	{
		return Everyplay.IsSupported();
	}

	public void MakeRequest(string method, string url, Dictionary<string, object> data, Everyplay.RequestReadyDelegate readyDelegate, Everyplay.RequestFailedDelegate failedDelegate)
	{
		Everyplay.MakeRequest(method, url, data, readyDelegate, failedDelegate);
	}

	public void PauseRecording()
	{
		Everyplay.PauseRecording();
	}

	public void PlayLastRecording()
	{
		Everyplay.PlayLastRecording();
	}

	public void PlayVideoWithDictionary(Dictionary<string, object> dict)
	{
		Everyplay.PlayVideoWithDictionary(dict);
	}

	public void PlayVideoWithURL(string url)
	{
		Everyplay.PlayVideoWithURL(url);
	}

	public void ResumeRecording()
	{
		Everyplay.ResumeRecording();
	}

	public void SetDisableSingleCoreDevices(bool state)
	{
		Everyplay.SetDisableSingleCoreDevices(state);
	}

	public void SetLowMemoryDevice(bool state)
	{
		Everyplay.SetLowMemoryDevice(state);
	}

	public void SetMaxRecordingMinutesLength(int minutes)
	{
		Everyplay.SetMaxRecordingMinutesLength(minutes);
	}

	public void SetMetadata(string key, object val)
	{
		Everyplay.SetMetadata(key, val);
	}

	public void SetMetadata(Dictionary<string, object> dict)
	{
		Everyplay.SetMetadata(dict);
	}

	public void SetMotionFactor(int factor)
	{
		Everyplay.SetMotionFactor(factor);
	}

	public void SetTargetFPS(int fps)
	{
		Everyplay.SetTargetFPS(fps);
	}

	public void SetThumbnailTargetTextureHeight(int textureHeight)
	{
		Everyplay.SetThumbnailTargetTextureHeight(textureHeight);
	}

	public void SetThumbnailTargetTextureId(int textureId)
	{
		Everyplay.SetThumbnailTargetTextureId(textureId);
	}

	public void SetThumbnailTargetTextureWidth(int textureWidth)
	{
		Everyplay.SetThumbnailTargetTextureWidth(textureWidth);
	}

	public void Show()
	{
		Everyplay.Show();
	}

	public void ShowSharingModal()
	{
		Everyplay.ShowSharingModal();
	}

	public void ShowWithPath(string path)
	{
		Everyplay.ShowWithPath(path);
	}

	public bool SnapshotRenderbuffer()
	{
		return Everyplay.SnapshotRenderbuffer();
	}

	public void StartRecording()
	{
		Everyplay.StartRecording();
	}

	public void StopRecording()
	{
		Everyplay.StopRecording();
	}

	public void TakeThumbnail()
	{
		Everyplay.TakeThumbnail();
	}

	public event Everyplay.FaceCamRecordingPermissionDelegate FaceCamRecordingPermission
	{
		add
		{
			Everyplay.FaceCamRecordingPermission += value;
		}
		remove
		{
			Everyplay.FaceCamRecordingPermission -= value;
		}
	}

	public event Everyplay.FaceCamSessionStartedDelegate FaceCamSessionStarted
	{
		add
		{
			Everyplay.FaceCamSessionStarted += value;
		}
		remove
		{
			Everyplay.FaceCamSessionStarted -= value;
		}
	}

	public event Everyplay.FaceCamSessionStoppedDelegate FaceCamSessionStopped
	{
		add
		{
			Everyplay.FaceCamSessionStopped += value;
		}
		remove
		{
			Everyplay.FaceCamSessionStopped -= value;
		}
	}

	public event Everyplay.ReadyForRecordingDelegate ReadyForRecording
	{
		add
		{
			Everyplay.ReadyForRecording += value;
		}
		remove
		{
			Everyplay.ReadyForRecording -= value;
		}
	}

	public event Everyplay.RecordingStartedDelegate RecordingStarted
	{
		add
		{
			Everyplay.RecordingStarted += value;
		}
		remove
		{
			Everyplay.RecordingStarted -= value;
		}
	}

	public event Everyplay.RecordingStoppedDelegate RecordingStopped
	{
		add
		{
			Everyplay.RecordingStopped += value;
		}
		remove
		{
			Everyplay.RecordingStopped -= value;
		}
	}

	public event Everyplay.ThumbnailReadyAtTextureIdDelegate ThumbnailReadyAtTextureId
	{
		add
		{
			Everyplay.ThumbnailReadyAtTextureId += value;
		}
		remove
		{
			Everyplay.ThumbnailReadyAtTextureId -= value;
		}
	}

	public event Everyplay.UploadDidCompleteDelegate UploadDidComplete
	{
		add
		{
			Everyplay.UploadDidComplete += value;
		}
		remove
		{
			Everyplay.UploadDidComplete -= value;
		}
	}

	public event Everyplay.UploadDidProgressDelegate UploadDidProgress
	{
		add
		{
			Everyplay.UploadDidProgress += value;
		}
		remove
		{
			Everyplay.UploadDidProgress -= value;
		}
	}

	public event Everyplay.UploadDidStartDelegate UploadDidStart
	{
		add
		{
			Everyplay.UploadDidStart += value;
		}
		remove
		{
			Everyplay.UploadDidStart -= value;
		}
	}

	public event Everyplay.WasClosedDelegate WasClosed
	{
		add
		{
			Everyplay.WasClosed += value;
		}
		remove
		{
			Everyplay.WasClosed -= value;
		}
	}
}