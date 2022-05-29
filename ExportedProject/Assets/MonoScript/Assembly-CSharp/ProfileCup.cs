using Rilisoft;
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UICenterOnPanelComponent))]
[RequireComponent(typeof(UISprite))]
public class ProfileCup : MonoBehaviour
{
	private UISprite _cup;

	[SerializeField]
	public RatingSystem.RatingLeague League;

	public GameObject Outline;

	private LeaguesGUIController _controller;

	private UICenterOnPanelComponent _centerMonitor;

	public UISprite Cup
	{
		get
		{
			UISprite uISprite = this._cup;
			if (uISprite == null)
			{
				UISprite component = base.GetComponent<UISprite>();
				UISprite uISprite1 = component;
				this._cup = component;
				uISprite = uISprite1;
			}
			return uISprite;
		}
	}

	public ProfileCup()
	{
	}

	private void OnCentered()
	{
		this._controller.CupCentered(this);
	}

	private void OnEnable()
	{
		this.Outline.SetActive(this.League == RatingSystem.instance.currentLeague);
		this.Cup.spriteName = string.Format("{0} {1}", this.League, 3 - RatingSystem.instance.DivisionInLeague(this.League));
	}

	private void Start()
	{
		this._controller = base.gameObject.GetComponentInParents<LeaguesGUIController>();
		this._centerMonitor = base.GetComponent<UICenterOnPanelComponent>();
		this._centerMonitor.OnCentered.RemoveListener(new UnityAction(this.OnCentered));
		this._centerMonitor.OnCentered.AddListener(new UnityAction(this.OnCentered));
	}
}