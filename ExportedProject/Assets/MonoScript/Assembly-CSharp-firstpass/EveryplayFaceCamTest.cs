using System;
using UnityEngine;

public class EveryplayFaceCamTest : MonoBehaviour
{
	private bool recordingPermissionGranted;

	private GameObject debugMessage;

	public EveryplayFaceCamTest()
	{
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void CheckFaceCamRecordingPermission(bool granted)
	{
		this.recordingPermissionGranted = granted;
		if (!granted && !this.debugMessage)
		{
			this.debugMessage = new GameObject("FaceCamDebugMessage", new Type[] { typeof(GUIText) });
			this.debugMessage.transform.position = new Vector3(0.5f, 0.5f, 0f);
			if (this.debugMessage != null)
			{
				GUIText component = this.debugMessage.GetComponent<GUIText>();
				if (component)
				{
					component.text = "Microphone access denied. FaceCam requires access to the microphone.\nPlease enable Microphone access from Settings / Privacy / Microphone.";
					component.alignment = TextAlignment.Center;
					component.anchor = TextAnchor.MiddleCenter;
				}
			}
		}
	}

	private void Destroy()
	{
		Everyplay.FaceCamRecordingPermission -= new Everyplay.FaceCamRecordingPermissionDelegate(this.CheckFaceCamRecordingPermission);
	}

	private void OnGUI()
	{
		if (this.recordingPermissionGranted)
		{
			if (GUI.Button(new Rect((float)(Screen.width - 10 - 158), 10f, 158f, 48f), (!Everyplay.FaceCamIsSessionRunning() ? "Start FaceCam session" : "Stop FaceCam session")))
			{
				if (!Everyplay.FaceCamIsSessionRunning())
				{
					Everyplay.FaceCamStartSession();
				}
				else
				{
					Everyplay.FaceCamStopSession();
				}
			}
		}
		else if (GUI.Button(new Rect((float)(Screen.width - 10 - 158), 10f, 158f, 48f), "Request REC permission"))
		{
			Everyplay.FaceCamRequestRecordingPermission();
		}
	}

	private void Start()
	{
		Everyplay.FaceCamRecordingPermission += new Everyplay.FaceCamRecordingPermissionDelegate(this.CheckFaceCamRecordingPermission);
	}
}