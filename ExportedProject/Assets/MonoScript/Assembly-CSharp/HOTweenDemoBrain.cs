using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class HOTweenDemoBrain : MonoBehaviour
{
	public Transform CubeTrans1;

	public Transform CubeTrans2;

	public Transform CubeTrans3;

	public string SampleString;

	public float SampleFloat;

	public HOTweenDemoBrain()
	{
	}

	private void Cube2StepComplete()
	{
		Debug.Log("HOTween: Cube 2 Step Complete");
	}

	private void OnGUI()
	{
		GUILayout.Label(string.Concat("String tween: ", this.SampleString), new GUILayoutOption[0]);
		GUILayout.Label(string.Concat("Float tween: ", this.SampleFloat), new GUILayoutOption[0]);
	}

	private void Start()
	{
		HOTween.Init(true, true, true);
		HOTween.To(this.CubeTrans1, 4f, "position", new Vector3(-3f, 6f, 0f));
		HOTween.To(this.CubeTrans2, 3f, (new TweenParms()).Prop("position", new Vector3(0f, 6f, 0f), true).Prop("rotation", new Vector3(0f, 1024f, 0f), true).Loops(-1, LoopType.Yoyo).Ease(EaseType.EaseInOutQuad).OnStepComplete(new TweenDelegate.TweenCallback(this.Cube2StepComplete)));
		HOTween.To(this, 3f, (new TweenParms()).Prop("SampleString", "Hello I'm a sample tweened string").Ease(EaseType.Linear).Loops(-1, LoopType.Yoyo));
		TweenParms tweenParm = (new TweenParms()).Prop("SampleFloat", 27.5f).Ease(EaseType.Linear).Loops(-1, LoopType.Yoyo);
		HOTween.To(this, 3f, tweenParm);
		Color component = this.CubeTrans3.GetComponent<Renderer>().material.color;
		component.a = 0f;
		Sequence sequence = new Sequence((new SequenceParms()).Loops(-1, LoopType.Yoyo));
		sequence.Append(HOTween.To(this.CubeTrans3, 1f, (new TweenParms()).Prop("rotation", new Vector3(360f, 0f, 0f))));
		sequence.Append(HOTween.To(this.CubeTrans3, 1f, (new TweenParms()).Prop("position", new Vector3(0f, 6f, 0f), true)));
		sequence.Append(HOTween.To(this.CubeTrans3, 1f, (new TweenParms()).Prop("rotation", new Vector3(0f, 360f, 0f))));
		sequence.Insert(sequence.duration * 0.5f, HOTween.To(this.CubeTrans3.GetComponent<Renderer>().material, sequence.duration * 0.5f, (new TweenParms()).Prop("color", component)));
		sequence.Play();
	}
}