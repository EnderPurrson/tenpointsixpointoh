using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UnityDataConnector : MonoBehaviour
{
	public string webServiceUrl = string.Empty;

	public string spreadsheetId = string.Empty;

	public string worksheetName = string.Empty;

	public string password = string.Empty;

	public float maxWaitTime = 10f;

	public GameObject dataDestinationObject;

	public string statisticsWorksheetName = "Statistics";

	public bool debugMode;

	private bool updating;

	private string currentStatus;

	private JsonData[] ssObjects;

	private bool saveToGS;

	private Rect guiBoxRect;

	private Rect guiButtonRect;

	private Rect guiButtonRect2;

	private Rect guiButtonRect3;

	public UnityDataConnector()
	{
	}

	private void Connect()
	{
		if (this.updating)
		{
			return;
		}
		this.updating = true;
		base.StartCoroutine(this.GetData());
	}

	[DebuggerHidden]
	private IEnumerator GetData()
	{
		UnityDataConnector.u003cGetDatau003ec__Iterator78 variable = null;
		return variable;
	}

	private void OnGUI()
	{
		GUI.Box(this.guiBoxRect, this.currentStatus);
		if (GUI.Button(this.guiButtonRect, "Update From Google Spreadsheet"))
		{
			this.Connect();
		}
		this.saveToGS = GUI.Toggle(this.guiButtonRect2, this.saveToGS, "Save Stats To Google Spreadsheet");
		if (GUI.Button(this.guiButtonRect3, "Reset Balls values"))
		{
			this.dataDestinationObject.SendMessage("ResetBalls");
		}
	}

	public void SaveDataOnTheCloud(string ballName, float collisionMagnitude)
	{
		if (this.saveToGS)
		{
			base.StartCoroutine(this.SendData(ballName, collisionMagnitude));
		}
	}

	[DebuggerHidden]
	private IEnumerator SendData(string ballName, float collisionMagnitude)
	{
		UnityDataConnector.u003cSendDatau003ec__Iterator79 variable = null;
		return variable;
	}

	private void Start()
	{
		this.updating = false;
		this.currentStatus = "Offline";
		this.saveToGS = false;
		this.guiBoxRect = new Rect(10f, 10f, 310f, 140f);
		this.guiButtonRect = new Rect(30f, 40f, 270f, 30f);
		this.guiButtonRect2 = new Rect(30f, 75f, 270f, 30f);
		this.guiButtonRect3 = new Rect(30f, 110f, 270f, 30f);
	}
}