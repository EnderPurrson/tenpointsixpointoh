using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreController : MonoBehaviour
{
	public int currentScore;

	public string[] addScoreString = new string[] { string.Empty, string.Empty, string.Empty };

	public int sumScore;

	public float[] timerAddScoreShow = new float[3];

	public float maxTimerMessage = 2f;

	public float maxTimerSumMessage = 4f;

	private float timeOldHeadShot;

	private List<string> pictNameList = new List<string>();

	private float timeShowPict;

	private float minTimeShowPict = 1f;

	private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

	public PlayerScoreController()
	{
	}

	private void AddScoreMessage(string _message, int _addScore)
	{
		this.addScoreString[2] = this.addScoreString[1];
		this.addScoreString[1] = _message;
		if (this.timerAddScoreShow[0] <= 0f)
		{
			this.sumScore = _addScore;
		}
		else
		{
			this.sumScore += _addScore;
		}
		this.addScoreString[0] = this.sumScore.ToString();
		this.timerAddScoreShow[2] = this.timerAddScoreShow[1];
		this.timerAddScoreShow[1] = this.maxTimerMessage;
		this.timerAddScoreShow[0] = this.maxTimerSumMessage;
	}

	public void AddScoreOnEvent(PlayerEventScoreController.ScoreEvent _event, float _koef = 1f)
	{
		if (Application.isEditor)
		{
			Debug.Log(_event.ToString());
		}
		if ((_event == PlayerEventScoreController.ScoreEvent.deadHeadShot || _event == PlayerEventScoreController.ScoreEvent.deadHeadShot) && Time.time - this.timeOldHeadShot < 1.5f)
		{
			_event = PlayerEventScoreController.ScoreEvent.doubleHeadShot;
		}
		int item = (int)((float)PlayerEventScoreController.scoreOnEvent[_event.ToString()] * _koef);
		if (item == 0)
		{
			return;
		}
		this.currentScore = WeaponManager.sharedManager.myNetworkStartTable.score;
		this.currentScore += item;
		string str = PlayerEventScoreController.messageOnEvent[_event.ToString()];
		if (!string.IsNullOrEmpty(str))
		{
			this.AddScoreMessage(string.Concat(new object[] { "+", item, " ", LocalizationStore.Get(str) }), item);
		}
		string item1 = PlayerEventScoreController.pictureNameOnEvent[_event.ToString()];
		if (!string.IsNullOrEmpty(item1) && InGameGUI.sharedInGameGUI != null)
		{
			bool flag = true;
			if (item1.Equals("Kill") && WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.multiKill > 0)
			{
				flag = false;
			}
			if (flag && !this.pictNameList.Contains(item1))
			{
				this.pictNameList.Add(item1);
			}
		}
		GlobalGameController.Score = this.currentScore;
		WeaponManager.sharedManager.myNetworkStartTable.score = this.currentScore;
		WeaponManager.sharedManager.myNetworkStartTable.SynhScore();
	}

	private void Start()
	{
		if (!Defs.isMulti || (!Defs.isInet || base.GetComponent<PhotonView>().isMine) && (Defs.isInet || base.GetComponent<NetworkView>().isMine))
		{
			foreach (KeyValuePair<string, string> keyValuePair in PlayerEventScoreController.audioClipNameOnEvent)
			{
				string value = keyValuePair.Value;
				AudioClip audioClip = Resources.Load(string.Concat("ScoreEventSounds/", value)) as AudioClip;
				if (audioClip == null)
				{
					continue;
				}
				this.clips.Add(keyValuePair.Key, audioClip);
			}
		}
		else
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		this.timeShowPict += Time.deltaTime;
		if (this.timeShowPict > this.minTimeShowPict && this.pictNameList.Count > 0)
		{
			string item = this.pictNameList[0];
			this.timeShowPict = 0f;
			InGameGUI.sharedInGameGUI.timerShowScorePict = InGameGUI.sharedInGameGUI.maxTimerShowScorePict;
			InGameGUI.sharedInGameGUI.scorePictName = item;
			if (this.clips.ContainsKey(item) && Defs.isSoundFX)
			{
				NGUITools.PlaySound(this.clips[item]);
			}
			this.pictNameList.RemoveAt(0);
		}
		if (this.timerAddScoreShow[2] > 0f)
		{
			this.timerAddScoreShow[2] -= Time.deltaTime;
		}
		if (this.timerAddScoreShow[1] > 0f)
		{
			this.timerAddScoreShow[1] -= Time.deltaTime;
		}
		if (this.timerAddScoreShow[0] > 0f)
		{
			this.timerAddScoreShow[0] -= Time.deltaTime;
		}
	}
}