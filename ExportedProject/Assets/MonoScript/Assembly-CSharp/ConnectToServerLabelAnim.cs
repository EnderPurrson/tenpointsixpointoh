using System;
using UnityEngine;

public class ConnectToServerLabelAnim : MonoBehaviour
{
	public UILabel myLabel;

	public string startText;

	private int stateLabel;

	private float timer;

	private float maxTimer = 1f;

	public ConnectToServerLabelAnim()
	{
	}

	private void Start()
	{
		this.timer = this.maxTimer;
		this.startText = LocalizationStore.Key_0564;
		this.myLabel.text = this.startText;
	}

	private void Update()
	{
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			this.timer = this.maxTimer;
			this.stateLabel++;
			if (this.stateLabel > 3)
			{
				this.stateLabel = 0;
			}
			string empty = string.Empty;
			for (int i = 0; i < this.stateLabel; i++)
			{
				empty = string.Concat(empty, ".");
			}
			this.myLabel.text = string.Format("{0} {1}", this.startText, empty);
		}
	}
}