using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DebugButtonsManager : MonoBehaviour
{
	private static DebugButtonsManager _instance;

	private static bool _topPanelOpened;

	private readonly static List<DebugButtonsManager.TopBarButton> _tbButtons;

	static DebugButtonsManager()
	{
		DebugButtonsManager._topPanelOpened = true;
		DebugButtonsManager._tbButtons = new List<DebugButtonsManager.TopBarButton>();
	}

	public DebugButtonsManager()
	{
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public static void ShowTopBarButton(string text, int width, Action onClickAction)
	{
		if (DebugButtonsManager._instance == null)
		{
			DebugButtonsManager._instance = (new GameObject("DebugButtonsManager")).AddComponent<DebugButtonsManager>();
		}
		DebugButtonsManager.TopBarButton topBarButton = DebugButtonsManager._tbButtons.FirstOrDefault<DebugButtonsManager.TopBarButton>((DebugButtonsManager.TopBarButton b) => b.Text == text);
		if (topBarButton != null)
		{
			topBarButton.NeedShow = true;
			return;
		}
		DebugButtonsManager.TopBarButton topBarButton1 = new DebugButtonsManager.TopBarButton(text, width, onClickAction);
		DebugButtonsManager._tbButtons.Add(topBarButton1);
	}

	private class TopBarButton
	{
		public bool NeedShow;

		public Action Act
		{
			get;
			private set;
		}

		public string Text
		{
			get;
			private set;
		}

		public int Width
		{
			get;
			private set;
		}

		public TopBarButton(string text, int width, Action act)
		{
			this.Text = text;
			this.Width = width;
			this.Act = act;
		}
	}
}