using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Linq;
using UnityEngine;

public class SampleSceneScript : MonoBehaviour
{
	public SampleSceneScript()
	{
	}

	private void OnGUI()
	{
		GameObject gameObject = GameObject.Find("Origin");
		if (GUILayout.Button("Parent", new GUILayoutOption[0]))
		{
			Debug.Log("------Parent");
			Debug.Log(gameObject.Parent().name);
		}
		if (GUILayout.Button("Child", new GUILayoutOption[0]))
		{
			Debug.Log("------Child");
			Debug.Log(gameObject.Child("Sphere_B").name);
		}
		if (GUILayout.Button("Descendants", new GUILayoutOption[0]))
		{
			Debug.Log("------Descendants");
			IEnumerator<GameObject> enumerator = gameObject.Descendants().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Debug.Log(enumerator.Current.name);
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
		}
		if (GUILayout.Button("name filter overload", new GUILayoutOption[0]))
		{
			Debug.Log("name filter overload");
			IEnumerator<GameObject> enumerator1 = gameObject.Descendants("Group").GetEnumerator();
			try
			{
				while (enumerator1.MoveNext())
				{
					Debug.Log(enumerator1.Current.name);
				}
			}
			finally
			{
				if (enumerator1 == null)
				{
				}
				enumerator1.Dispose();
			}
		}
		if (GUILayout.Button("OfComponent", new GUILayoutOption[0]))
		{
			Debug.Log("------OfComponent");
			IEnumerable<SphereCollider> sphereColliders = gameObject.Descendants().OfComponent<SphereCollider>();
			IEnumerator<SphereCollider> enumerator2 = sphereColliders.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					SphereCollider current = enumerator2.Current;
					Debug.Log(string.Concat(new object[] { "Sphere:", current.name, " Radius:", current.radius }));
				}
			}
			finally
			{
				if (enumerator2 == null)
				{
				}
				enumerator2.Dispose();
			}
			(
				from x in gameObject.Descendants()
				where x.tag == "foobar"
				select x).OfComponent<BoxCollider2D>();
		}
		if (GUILayout.Button("LINQ", new GUILayoutOption[0]))
		{
			Debug.Log("------LINQ");
			IEnumerable<GameObject> gameObjects = 
				from x in gameObject.Children()
				where x.name.EndsWith("B")
				select x;
			IEnumerator<GameObject> enumerator3 = gameObjects.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					Debug.Log(enumerator3.Current.name);
				}
			}
			finally
			{
				if (enumerator3 == null)
				{
				}
				enumerator3.Dispose();
			}
		}
		if (GUILayout.Button("Add", new GUILayoutOption[0]))
		{
			GameObject[] gameObjectArray = new GameObject[] { new GameObject("lastChild1"), new GameObject("lastChild2"), new GameObject("lastChild3") };
			bool? nullable = null;
			gameObject.Add((IEnumerable<GameObject>)gameObjectArray, TransformCloneType.KeepOriginal, nullable, null);
			GameObject[] gameObject1 = new GameObject[] { new GameObject("firstChild1"), new GameObject("firstChild2"), new GameObject("firstChild3") };
			bool? nullable1 = null;
			gameObject.AddFirst((IEnumerable<GameObject>)gameObject1, TransformCloneType.KeepOriginal, nullable1, null);
			GameObject[] gameObjectArray1 = new GameObject[] { new GameObject("beforeSelf1"), new GameObject("beforeSelf2"), new GameObject("beforeSelf3") };
			bool? nullable2 = null;
			gameObject.AddBeforeSelf((IEnumerable<GameObject>)gameObjectArray1, TransformCloneType.KeepOriginal, nullable2, null);
			GameObject[] gameObject2 = new GameObject[] { new GameObject("afterSelf1"), new GameObject("afterSelf2"), new GameObject("afterSelf3") };
			bool? nullable3 = null;
			gameObject.AddAfterSelf((IEnumerable<GameObject>)gameObject2, TransformCloneType.KeepOriginal, nullable3, null);
			(
				from GameObject x in Resources.FindObjectsOfTypeAll<GameObject>()
				where (!(x.Parent() == null) || x.name.Contains("Camera") || !(x.name != "Root") || !(x.name != string.Empty) || !(x.name != "HandlesGO") || x.name.StartsWith("Scene") || x.name.Contains("Light") ? false : !x.name.Contains("Materials"))
				select x).Select<GameObject, GameObject>((GameObject x) => {
				Debug.Log(x.name);
				return x;
			}).Destroy(false);
		}
		if (GUILayout.Button("MoveTo", new GUILayoutOption[0]))
		{
			GameObject[] gameObjectArray2 = new GameObject[] { new GameObject("lastChild1(Original)"), new GameObject("lastChild2(Original)"), new GameObject("lastChild3(Original)") };
			bool? nullable4 = null;
			gameObject.MoveToLast((IEnumerable<GameObject>)gameObjectArray2, TransformMoveType.DoNothing, nullable4);
			GameObject[] gameObject3 = new GameObject[] { new GameObject("firstChild1(Original)"), new GameObject("firstChild2(Original)"), new GameObject("firstChild3(Original)") };
			bool? nullable5 = null;
			gameObject.MoveToFirst((IEnumerable<GameObject>)gameObject3, TransformMoveType.DoNothing, nullable5);
			GameObject[] gameObjectArray3 = new GameObject[] { new GameObject("beforeSelf1(Original)"), new GameObject("beforeSelf2(Original)"), new GameObject("beforeSelf3(Original)") };
			bool? nullable6 = null;
			gameObject.MoveToBeforeSelf((IEnumerable<GameObject>)gameObjectArray3, TransformMoveType.DoNothing, nullable6);
			GameObject[] gameObject4 = new GameObject[] { new GameObject("afterSelf1(Original)"), new GameObject("afterSelf2(Original)"), new GameObject("afterSelf3(Original)") };
			bool? nullable7 = null;
			gameObject.MoveToAfterSelf((IEnumerable<GameObject>)gameObject4, TransformMoveType.DoNothing, nullable7);
		}
		if (GUILayout.Button("Destroy", new GUILayoutOption[0]))
		{
			(
				from x in gameObject.transform.root.gameObject.Descendants()
				where (x.name.EndsWith("(Clone)") ? true : x.name.EndsWith("(Original)"))
				select x).Select<GameObject, GameObject>((GameObject x) => {
				Debug.Log(x.name);
				return x;
			}).Destroy(false);
		}
	}
}