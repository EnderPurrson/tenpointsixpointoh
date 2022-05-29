using Rilisoft;
using System;
using UnityEngine;

public sealed class Message : MonoBehaviour
{
	public GUIStyle labelStyle;

	public Rect rect = Tools.SuccessMessageRect();

	public string message = "Purchases restored";

	public int depth = -2;

	private float _startTime;

	public float OnScreenTime = 3f;

	public Message()
	{
	}

	private void OnGUI()
	{
		if (Time.realtimeSinceStartup - this._startTime >= (TrainingController.TrainingCompleted ? this.OnScreenTime : this.OnScreenTime / 2f))
		{
			this.Remove();
			return;
		}
		this.rect = Tools.SuccessMessageRect();
		GUI.depth = this.depth;
		this.labelStyle.fontSize = Player_move_c.FontSizeForMessages;
		GUI.Label(this.rect, this.message, this.labelStyle);
	}

	private void Remove()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this._startTime = Time.realtimeSinceStartup;
	}
}