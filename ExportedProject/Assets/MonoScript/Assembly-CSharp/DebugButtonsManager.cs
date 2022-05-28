using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DebugButtonsManager : MonoBehaviour
{
	private class TopBarButton
	{
		public bool NeedShow = true;

		public string Text { get; private set; }

		public int Width { get; private set; }

		public Action Act { get; private set; }

		public TopBarButton(string text, int width, Action act)
		{
			Text = text;
			Width = width;
			Act = act;
		}
	}

	[CompilerGenerated]
	private sealed class _003CShowTopBarButton_003Ec__AnonStorey33F
	{
		internal string text;

		internal bool _003C_003Em__539(TopBarButton b)
		{
			return b.Text == text;
		}
	}

	private static DebugButtonsManager _instance;

	private static bool _topPanelOpened = true;

	private static readonly List<TopBarButton> _tbButtons = new List<TopBarButton>();

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public static void ShowTopBarButton(string text, int width, Action onClickAction)
	{
		_003CShowTopBarButton_003Ec__AnonStorey33F _003CShowTopBarButton_003Ec__AnonStorey33F = new _003CShowTopBarButton_003Ec__AnonStorey33F();
		_003CShowTopBarButton_003Ec__AnonStorey33F.text = text;
		if (_instance == null)
		{
			GameObject gameObject = new GameObject("DebugButtonsManager");
			_instance = gameObject.AddComponent<DebugButtonsManager>();
		}
		TopBarButton topBarButton = _tbButtons.FirstOrDefault(_003CShowTopBarButton_003Ec__AnonStorey33F._003C_003Em__539);
		if (topBarButton != null)
		{
			topBarButton.NeedShow = true;
			return;
		}
		TopBarButton item = new TopBarButton(_003CShowTopBarButton_003Ec__AnonStorey33F.text, width, onClickAction);
		_tbButtons.Add(item);
	}
}
