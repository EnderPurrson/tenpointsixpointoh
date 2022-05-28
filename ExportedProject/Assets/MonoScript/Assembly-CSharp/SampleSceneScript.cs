using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Linq;
using UnityEngine;

public class SampleSceneScript : MonoBehaviour
{
	[CompilerGenerated]
	private static Func<GameObject, bool> _003C_003Ef__am_0024cache0;

	[CompilerGenerated]
	private static Func<GameObject, bool> _003C_003Ef__am_0024cache1;

	[CompilerGenerated]
	private static Func<GameObject, bool> _003C_003Ef__am_0024cache2;

	[CompilerGenerated]
	private static Func<GameObject, GameObject> _003C_003Ef__am_0024cache3;

	[CompilerGenerated]
	private static Func<GameObject, bool> _003C_003Ef__am_0024cache4;

	[CompilerGenerated]
	private static Func<GameObject, GameObject> _003C_003Ef__am_0024cache5;

	private void OnGUI()
	{
		GameObject gameObject = GameObject.Find("Origin");
		if (GUILayout.Button("Parent"))
		{
			Debug.Log("------Parent");
			GameObject gameObject2 = gameObject.Parent();
			Debug.Log(gameObject2.name);
		}
		if (GUILayout.Button("Child"))
		{
			Debug.Log("------Child");
			GameObject gameObject3 = gameObject.Child("Sphere_B");
			Debug.Log(gameObject3.name);
		}
		if (GUILayout.Button("Descendants"))
		{
			Debug.Log("------Descendants");
			IEnumerable<GameObject> enumerable = gameObject.Descendants();
			foreach (GameObject item in enumerable)
			{
				Debug.Log(item.name);
			}
		}
		if (GUILayout.Button("name filter overload"))
		{
			Debug.Log("name filter overload");
			IEnumerable<GameObject> enumerable2 = gameObject.Descendants("Group");
			foreach (GameObject item2 in enumerable2)
			{
				Debug.Log(item2.name);
			}
		}
		if (GUILayout.Button("OfComponent"))
		{
			Debug.Log("------OfComponent");
			IEnumerable<SphereCollider> enumerable3 = gameObject.Descendants().OfComponent<SphereCollider>();
			foreach (SphereCollider item3 in enumerable3)
			{
				Debug.Log("Sphere:" + item3.name + " Radius:" + item3.radius);
			}
			IEnumerable<GameObject> source = gameObject.Descendants();
			if (_003C_003Ef__am_0024cache0 == null)
			{
				_003C_003Ef__am_0024cache0 = _003COnGUI_003Em__18E;
			}
			source.Where(_003C_003Ef__am_0024cache0).OfComponent<BoxCollider2D>();
		}
		if (GUILayout.Button("LINQ"))
		{
			Debug.Log("------LINQ");
			IEnumerable<GameObject> source2 = gameObject.Children();
			if (_003C_003Ef__am_0024cache1 == null)
			{
				_003C_003Ef__am_0024cache1 = _003COnGUI_003Em__18F;
			}
			IEnumerable<GameObject> enumerable4 = source2.Where(_003C_003Ef__am_0024cache1);
			foreach (GameObject item4 in enumerable4)
			{
				Debug.Log(item4.name);
			}
		}
		if (GUILayout.Button("Add"))
		{
			gameObject.Add(new GameObject[3]
			{
				new GameObject("lastChild1"),
				new GameObject("lastChild2"),
				new GameObject("lastChild3")
			});
			gameObject.AddFirst(new GameObject[3]
			{
				new GameObject("firstChild1"),
				new GameObject("firstChild2"),
				new GameObject("firstChild3")
			});
			gameObject.AddBeforeSelf(new GameObject[3]
			{
				new GameObject("beforeSelf1"),
				new GameObject("beforeSelf2"),
				new GameObject("beforeSelf3")
			});
			gameObject.AddAfterSelf(new GameObject[3]
			{
				new GameObject("afterSelf1"),
				new GameObject("afterSelf2"),
				new GameObject("afterSelf3")
			});
			IEnumerable<GameObject> source3 = Resources.FindObjectsOfTypeAll<GameObject>().Cast<GameObject>();
			if (_003C_003Ef__am_0024cache2 == null)
			{
				_003C_003Ef__am_0024cache2 = _003COnGUI_003Em__190;
			}
			IEnumerable<GameObject> source4 = source3.Where(_003C_003Ef__am_0024cache2);
			if (_003C_003Ef__am_0024cache3 == null)
			{
				_003C_003Ef__am_0024cache3 = _003COnGUI_003Em__191;
			}
			source4.Select(_003C_003Ef__am_0024cache3).Destroy();
		}
		if (GUILayout.Button("MoveTo"))
		{
			gameObject.MoveToLast(new GameObject[3]
			{
				new GameObject("lastChild1(Original)"),
				new GameObject("lastChild2(Original)"),
				new GameObject("lastChild3(Original)")
			});
			gameObject.MoveToFirst(new GameObject[3]
			{
				new GameObject("firstChild1(Original)"),
				new GameObject("firstChild2(Original)"),
				new GameObject("firstChild3(Original)")
			});
			gameObject.MoveToBeforeSelf(new GameObject[3]
			{
				new GameObject("beforeSelf1(Original)"),
				new GameObject("beforeSelf2(Original)"),
				new GameObject("beforeSelf3(Original)")
			});
			gameObject.MoveToAfterSelf(new GameObject[3]
			{
				new GameObject("afterSelf1(Original)"),
				new GameObject("afterSelf2(Original)"),
				new GameObject("afterSelf3(Original)")
			});
		}
		if (GUILayout.Button("Destroy"))
		{
			IEnumerable<GameObject> source5 = gameObject.transform.root.gameObject.Descendants();
			if (_003C_003Ef__am_0024cache4 == null)
			{
				_003C_003Ef__am_0024cache4 = _003COnGUI_003Em__192;
			}
			IEnumerable<GameObject> source6 = source5.Where(_003C_003Ef__am_0024cache4);
			if (_003C_003Ef__am_0024cache5 == null)
			{
				_003C_003Ef__am_0024cache5 = _003COnGUI_003Em__193;
			}
			source6.Select(_003C_003Ef__am_0024cache5).Destroy();
		}
	}

	[CompilerGenerated]
	private static bool _003COnGUI_003Em__18E(GameObject x)
	{
		return x.tag == "foobar";
	}

	[CompilerGenerated]
	private static bool _003COnGUI_003Em__18F(GameObject x)
	{
		return x.name.EndsWith("B");
	}

	[CompilerGenerated]
	private static bool _003COnGUI_003Em__190(GameObject x)
	{
		return x.Parent() == null && !x.name.Contains("Camera") && x.name != "Root" && x.name != string.Empty && x.name != "HandlesGO" && !x.name.StartsWith("Scene") && !x.name.Contains("Light") && !x.name.Contains("Materials");
	}

	[CompilerGenerated]
	private static GameObject _003COnGUI_003Em__191(GameObject x)
	{
		Debug.Log(x.name);
		return x;
	}

	[CompilerGenerated]
	private static bool _003COnGUI_003Em__192(GameObject x)
	{
		return x.name.EndsWith("(Clone)") || x.name.EndsWith("(Original)");
	}

	[CompilerGenerated]
	private static GameObject _003COnGUI_003Em__193(GameObject x)
	{
		Debug.Log(x.name);
		return x;
	}
}
