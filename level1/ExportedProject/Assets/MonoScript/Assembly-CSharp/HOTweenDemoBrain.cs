using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

public class HOTweenDemoBrain : MonoBehaviour
{
	public Transform CubeTrans1;

	public Transform CubeTrans2;

	public Transform CubeTrans3;

	public string SampleString;

	public float SampleFloat;

	private void Start()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		HOTween.Init(true, true, true);
		HOTween.To((object)CubeTrans1, 4f, "position", (object)new Vector3(-3f, 6f, 0f));
		HOTween.To((object)CubeTrans2, 3f, new TweenParms().Prop("position", (object)new Vector3(0f, 6f, 0f), true).Prop("rotation", (object)new Vector3(0f, 1024f, 0f), true).Loops(-1, (LoopType)1)
			.Ease((EaseType)6)
			.OnStepComplete(new TweenCallback(Cube2StepComplete)));
		HOTween.To((object)this, 3f, new TweenParms().Prop("SampleString", (object)"Hello I'm a sample tweened string").Ease((EaseType)0).Loops(-1, (LoopType)1));
		TweenParms val = new TweenParms().Prop("SampleFloat", (object)27.5f).Ease((EaseType)0).Loops(-1, (LoopType)1);
		HOTween.To((object)this, 3f, val);
		Color color = CubeTrans3.GetComponent<Renderer>().material.color;
		color.a = 0f;
		Sequence val2 = new Sequence(new SequenceParms().Loops(-1, (LoopType)1));
		val2.Append((IHOTweenComponent)(object)HOTween.To((object)CubeTrans3, 1f, new TweenParms().Prop("rotation", (object)new Vector3(360f, 0f, 0f))));
		val2.Append((IHOTweenComponent)(object)HOTween.To((object)CubeTrans3, 1f, new TweenParms().Prop("position", (object)new Vector3(0f, 6f, 0f), true)));
		val2.Append((IHOTweenComponent)(object)HOTween.To((object)CubeTrans3, 1f, new TweenParms().Prop("rotation", (object)new Vector3(0f, 360f, 0f))));
		val2.Insert(((ABSTweenComponent)val2).get_duration() * 0.5f, (IHOTweenComponent)(object)HOTween.To((object)CubeTrans3.GetComponent<Renderer>().material, ((ABSTweenComponent)val2).get_duration() * 0.5f, new TweenParms().Prop("color", (object)color)));
		((ABSTweenComponent)val2).Play();
	}

	private void OnGUI()
	{
		GUILayout.Label("String tween: " + SampleString);
		GUILayout.Label("Float tween: " + SampleFloat);
	}

	private void Cube2StepComplete()
	{
		Debug.Log("HOTween: Cube 2 Step Complete");
	}
}
