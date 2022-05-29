using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EveryplayTest : MonoBehaviour
{
	public bool showUploadStatus = true;

	private bool isRecording;

	private bool isPaused;

	private bool isRecordingFinished;

	private GUIText uploadStatusLabel;

	public EveryplayTest()
	{
	}

	private void Awake()
	{
		if (base.enabled && this.showUploadStatus)
		{
			this.CreateUploadStatusLabel();
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void CreateUploadStatusLabel()
	{
		GameObject gameObject = new GameObject("UploadStatus", new Type[] { typeof(GUIText) });
		if (gameObject)
		{
			gameObject.transform.parent = base.transform;
			this.uploadStatusLabel = gameObject.GetComponent<GUIText>();
			if (this.uploadStatusLabel != null)
			{
				this.uploadStatusLabel.anchor = TextAnchor.LowerLeft;
				this.uploadStatusLabel.alignment = TextAlignment.Left;
				this.uploadStatusLabel.text = "Not uploading";
			}
		}
	}

	private void Destroy()
	{
		if (this.uploadStatusLabel != null)
		{
			Everyplay.UploadDidStart -= new Everyplay.UploadDidStartDelegate(this.UploadDidStart);
			Everyplay.UploadDidProgress -= new Everyplay.UploadDidProgressDelegate(this.UploadDidProgress);
			Everyplay.UploadDidComplete -= new Everyplay.UploadDidCompleteDelegate(this.UploadDidComplete);
		}
		Everyplay.RecordingStarted -= new Everyplay.RecordingStartedDelegate(this.RecordingStarted);
		Everyplay.RecordingStopped -= new Everyplay.RecordingStoppedDelegate(this.RecordingStopped);
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 10f, 138f, 48f), "Everyplay"))
		{
			Everyplay.Show();
		}
		if (this.isRecording && GUI.Button(new Rect(10f, 64f, 138f, 48f), "Stop Recording"))
		{
			Everyplay.StopRecording();
		}
		else if (!this.isRecording && GUI.Button(new Rect(10f, 64f, 138f, 48f), "Start Recording"))
		{
			Everyplay.StartRecording();
		}
		if (this.isRecording)
		{
			if (!this.isPaused && GUI.Button(new Rect(160f, 64f, 138f, 48f), "Pause Recording"))
			{
				Everyplay.PauseRecording();
				this.isPaused = true;
			}
			else if (this.isPaused && GUI.Button(new Rect(160f, 64f, 138f, 48f), "Resume Recording"))
			{
				Everyplay.ResumeRecording();
				this.isPaused = false;
			}
		}
		if (this.isRecordingFinished && GUI.Button(new Rect(10f, 118f, 138f, 48f), "Play Last Recording"))
		{
			Everyplay.PlayLastRecording();
		}
		if (this.isRecording && GUI.Button(new Rect(10f, 118f, 138f, 48f), "Take Thumbnail"))
		{
			Everyplay.TakeThumbnail();
		}
		if (this.isRecordingFinished && GUI.Button(new Rect(10f, 172f, 138f, 48f), "Show sharing modal"))
		{
			Everyplay.ShowSharingModal();
		}
	}

	private void RecordingStarted()
	{
		this.isRecording = true;
		this.isPaused = false;
		this.isRecordingFinished = false;
	}

	private void RecordingStopped()
	{
		this.isRecording = false;
		this.isRecordingFinished = true;
	}

	[DebuggerHidden]
	private IEnumerator ResetUploadStatusAfterDelay(float time)
	{
		EveryplayTest.u003cResetUploadStatusAfterDelayu003ec__Iterator7 variable = null;
		return variable;
	}

	private void Start()
	{
		if (this.uploadStatusLabel != null)
		{
			Everyplay.UploadDidStart += new Everyplay.UploadDidStartDelegate(this.UploadDidStart);
			Everyplay.UploadDidProgress += new Everyplay.UploadDidProgressDelegate(this.UploadDidProgress);
			Everyplay.UploadDidComplete += new Everyplay.UploadDidCompleteDelegate(this.UploadDidComplete);
		}
		Everyplay.RecordingStarted += new Everyplay.RecordingStartedDelegate(this.RecordingStarted);
		Everyplay.RecordingStopped += new Everyplay.RecordingStoppedDelegate(this.RecordingStopped);
	}

	private void UploadDidComplete(int videoId)
	{
		this.uploadStatusLabel.text = string.Concat("Upload ", videoId, " completed.");
		base.StartCoroutine(this.ResetUploadStatusAfterDelay(2f));
	}

	private void UploadDidProgress(int videoId, float progress)
	{
		this.uploadStatusLabel.text = string.Concat(new object[] { "Upload ", videoId, " is ", Mathf.RoundToInt((float)progress * 100f), "% completed." });
	}

	private void UploadDidStart(int videoId)
	{
		this.uploadStatusLabel.text = string.Concat("Upload ", videoId, " started.");
	}
}