using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Unity.Linq
{
	public static class GameObjectExtensions
	{
		public static GameObject Add(this GameObject parent, GameObject childOriginal, TransformCloneType cloneType = 0, bool? setActive = null, string specifiedName = null)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (childOriginal == null)
			{
				throw new ArgumentNullException("childOriginal");
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(childOriginal);
			Transform transforms = gameObject.transform;
			RectTransform rectTransform = transforms as RectTransform;
			if (rectTransform == null)
			{
				Transform transforms1 = parent.transform;
				transforms.parent = transforms1;
				switch (cloneType)
				{
					case TransformCloneType.KeepOriginal:
					{
						Transform transforms2 = childOriginal.transform;
						transforms.localPosition = transforms2.localPosition;
						transforms.localScale = transforms2.localScale;
						transforms.localRotation = transforms2.localRotation;
						break;
					}
					case TransformCloneType.FollowParent:
					{
						transforms.localPosition = transforms1.localPosition;
						transforms.localScale = transforms1.localScale;
						transforms.localRotation = transforms1.localRotation;
						break;
					}
					case TransformCloneType.Origin:
					{
						transforms.localPosition = Vector3.zero;
						transforms.localScale = Vector3.one;
						transforms.localRotation = Quaternion.identity;
						break;
					}
					case TransformCloneType.DoNothing:
					{
						break;
					}
					default:
					{
						goto case TransformCloneType.DoNothing;
					}
				}
			}
			else
			{
				rectTransform.SetParent(parent.transform, false);
			}
			gameObject.layer = parent.layer;
			if (setActive.HasValue)
			{
				gameObject.SetActive(setActive.Value);
			}
			if (specifiedName != null)
			{
				gameObject.name = specifiedName;
			}
			return gameObject;
		}

		public static List<GameObject> Add(this GameObject parent, IEnumerable<GameObject> childOriginals, TransformCloneType cloneType = 0, bool? setActive = null, string specifiedName = null)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (childOriginals == null)
			{
				throw new ArgumentNullException("childOriginals");
			}
			List<GameObject> gameObjects = new List<GameObject>();
			IEnumerator<GameObject> enumerator = childOriginals.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GameObject current = enumerator.Current;
					GameObject gameObject = parent.Add(current, cloneType, setActive, specifiedName);
					gameObjects.Add(gameObject);
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			return gameObjects;
		}

		public static GameObject AddAfterSelf(this GameObject parent, GameObject childOriginal, TransformCloneType cloneType = 0, bool? setActive = null, string specifiedName = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex() + 1;
			GameObject gameObject1 = gameObject.Add(childOriginal, cloneType, setActive, specifiedName);
			gameObject1.transform.SetSiblingIndex(siblingIndex);
			return gameObject1;
		}

		public static List<GameObject> AddAfterSelf(this GameObject parent, IEnumerable<GameObject> childOriginals, TransformCloneType cloneType = 0, bool? setActive = null, string specifiedName = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex() + 1;
			List<GameObject> gameObjects = gameObject.Add(childOriginals, cloneType, setActive, specifiedName);
			for (int i = gameObjects.Count - 1; i >= 0; i--)
			{
				gameObjects[i].transform.SetSiblingIndex(siblingIndex);
			}
			return gameObjects;
		}

		public static GameObject AddBeforeSelf(this GameObject parent, GameObject childOriginal, TransformCloneType cloneType = 0, bool? setActive = null, string specifiedName = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex();
			GameObject gameObject1 = gameObject.Add(childOriginal, cloneType, setActive, specifiedName);
			gameObject1.transform.SetSiblingIndex(siblingIndex);
			return gameObject1;
		}

		public static List<GameObject> AddBeforeSelf(this GameObject parent, IEnumerable<GameObject> childOriginals, TransformCloneType cloneType = 0, bool? setActive = null, string specifiedName = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex();
			List<GameObject> gameObjects = gameObject.Add(childOriginals, cloneType, setActive, specifiedName);
			for (int i = gameObjects.Count - 1; i >= 0; i--)
			{
				gameObjects[i].transform.SetSiblingIndex(siblingIndex);
			}
			return gameObjects;
		}

		public static GameObject AddFirst(this GameObject parent, GameObject childOriginal, TransformCloneType cloneType = 0, bool? setActive = null, string specifiedName = null)
		{
			GameObject gameObject = parent.Add(childOriginal, cloneType, setActive, specifiedName);
			gameObject.transform.SetAsFirstSibling();
			return gameObject;
		}

		public static List<GameObject> AddFirst(this GameObject parent, IEnumerable<GameObject> childOriginals, TransformCloneType cloneType = 0, bool? setActive = null, string specifiedName = null)
		{
			List<GameObject> gameObjects = parent.Add(childOriginals, cloneType, setActive, specifiedName);
			for (int i = gameObjects.Count - 1; i >= 0; i--)
			{
				gameObjects[i].transform.SetAsFirstSibling();
			}
			return gameObjects;
		}

		public static IEnumerable<GameObject> AfterSelf(this GameObject origin)
		{
			return origin.AfterSelfCore(null, false);
		}

		public static IEnumerable<GameObject> AfterSelf(this GameObject origin, string name)
		{
			return origin.AfterSelfCore(name, false);
		}

		public static IEnumerable<GameObject> AfterSelfAndSelf(this GameObject origin)
		{
			return origin.AfterSelfCore(null, true);
		}

		public static IEnumerable<GameObject> AfterSelfAndSelf(this GameObject origin, string name)
		{
			return origin.AfterSelfCore(name, true);
		}

		[DebuggerHidden]
		private static IEnumerable<GameObject> AfterSelfCore(this GameObject origin, string nameFilter, bool withSelf)
		{
			GameObjectExtensions.u003cAfterSelfCoreu003ec__IteratorA3 variable = null;
			return variable;
		}

		[DebuggerHidden]
		public static IEnumerable<GameObject> Ancestors(this IEnumerable<GameObject> source)
		{
			GameObjectExtensions.u003cAncestorsu003ec__Iterator92 variable = null;
			return variable;
		}

		[DebuggerHidden]
		public static IEnumerable<GameObject> Ancestors(IEnumerable<GameObject> source, string name)
		{
			GameObjectExtensions.u003cAncestorsu003ec__Iterator93 variable = null;
			return variable;
		}

		public static IEnumerable<GameObject> Ancestors(this GameObject origin)
		{
			return GameObjectExtensions.AncestorsCore(origin, null, false);
		}

		public static IEnumerable<GameObject> Ancestors(this GameObject origin, string name)
		{
			return GameObjectExtensions.AncestorsCore(origin, null, false);
		}

		[DebuggerHidden]
		public static IEnumerable<GameObject> AncestorsAndSelf(IEnumerable<GameObject> source)
		{
			GameObjectExtensions.u003cAncestorsAndSelfu003ec__Iterator94 variable = null;
			return variable;
		}

		[DebuggerHidden]
		public static IEnumerable<GameObject> AncestorsAndSelf(this IEnumerable<GameObject> source, string name)
		{
			GameObjectExtensions.u003cAncestorsAndSelfu003ec__Iterator95 variable = null;
			return variable;
		}

		public static IEnumerable<GameObject> AncestorsAndSelf(this GameObject origin)
		{
			return GameObjectExtensions.AncestorsCore(origin, null, true);
		}

		public static IEnumerable<GameObject> AncestorsAndSelf(this GameObject origin, string name)
		{
			return GameObjectExtensions.AncestorsCore(origin, name, true);
		}

		[DebuggerHidden]
		private static IEnumerable<GameObject> AncestorsCore(GameObject origin, string nameFilter, bool withSelf)
		{
			GameObjectExtensions.u003cAncestorsCoreu003ec__IteratorA0 variable = null;
			return variable;
		}

		public static IEnumerable<GameObject> BeforeSelf(this GameObject origin)
		{
			return origin.BeforeSelfCore(null, false);
		}

		public static IEnumerable<GameObject> BeforeSelf(this GameObject origin, string name)
		{
			return origin.BeforeSelfCore(name, false);
		}

		public static IEnumerable<GameObject> BeforeSelfAndSelf(this GameObject origin)
		{
			return origin.BeforeSelfCore(null, true);
		}

		public static IEnumerable<GameObject> BeforeSelfAndSelf(this GameObject origin, string name)
		{
			return origin.BeforeSelfCore(name, true);
		}

		[DebuggerHidden]
		private static IEnumerable<GameObject> BeforeSelfCore(this GameObject origin, string nameFilter, bool withSelf)
		{
			GameObjectExtensions.u003cBeforeSelfCoreu003ec__IteratorA2 variable = null;
			return variable;
		}

		public static GameObject Child(this GameObject origin, string name)
		{
			if (origin == null)
			{
				return null;
			}
			Transform transforms = origin.transform.FindChild(name);
			if (transforms == null)
			{
				return null;
			}
			return transforms.gameObject;
		}

		[DebuggerHidden]
		public static IEnumerable<GameObject> Children(this IEnumerable<GameObject> source)
		{
			GameObjectExtensions.u003cChildrenu003ec__Iterator9A variable = null;
			return variable;
		}

		[DebuggerHidden]
		public static IEnumerable<GameObject> Children(this IEnumerable<GameObject> source, string name)
		{
			GameObjectExtensions.u003cChildrenu003ec__Iterator9B variable = null;
			return variable;
		}

		public static IEnumerable<GameObject> Children(this GameObject origin)
		{
			return origin.ChildrenCore(null, false);
		}

		public static IEnumerable<GameObject> Children(this GameObject origin, string name)
		{
			return origin.ChildrenCore(name, false);
		}

		[DebuggerHidden]
		public static IEnumerable<GameObject> ChildrenAndSelf(this IEnumerable<GameObject> source)
		{
			GameObjectExtensions.u003cChildrenAndSelfu003ec__Iterator9C variable = null;
			return variable;
		}

		[DebuggerHidden]
		public static IEnumerable<GameObject> ChildrenAndSelf(this IEnumerable<GameObject> source, string name)
		{
			GameObjectExtensions.u003cChildrenAndSelfu003ec__Iterator9D variable = null;
			return variable;
		}

		public static IEnumerable<GameObject> ChildrenAndSelf(this GameObject origin)
		{
			return origin.ChildrenCore(null, true);
		}

		public static IEnumerable<GameObject> ChildrenAndSelf(this GameObject origin, string name)
		{
			return origin.ChildrenCore(name, true);
		}

		[DebuggerHidden]
		private static IEnumerable<GameObject> ChildrenCore(this GameObject origin, string nameFilter, bool withSelf)
		{
			GameObjectExtensions.u003cChildrenCoreu003ec__Iterator9F variable = null;
			return variable;
		}

		[DebuggerHidden]
		public static IEnumerable<GameObject> Descendants(this IEnumerable<GameObject> source)
		{
			GameObjectExtensions.u003cDescendantsu003ec__Iterator96 variable = null;
			return variable;
		}

		[DebuggerHidden]
		public static IEnumerable<GameObject> Descendants(IEnumerable<GameObject> source, string name)
		{
			GameObjectExtensions.u003cDescendantsu003ec__Iterator97 variable = null;
			return variable;
		}

		public static IEnumerable<GameObject> Descendants(this GameObject origin)
		{
			return GameObjectExtensions.DescendantsCore(origin, null, false);
		}

		public static IEnumerable<GameObject> Descendants(this GameObject origin, string name)
		{
			return GameObjectExtensions.DescendantsCore(origin, name, false);
		}

		[DebuggerHidden]
		public static IEnumerable<GameObject> DescendantsAndSelf(this IEnumerable<GameObject> source)
		{
			GameObjectExtensions.u003cDescendantsAndSelfu003ec__Iterator98 variable = null;
			return variable;
		}

		[DebuggerHidden]
		public static IEnumerable<GameObject> DescendantsAndSelf(this IEnumerable<GameObject> source, string name)
		{
			GameObjectExtensions.u003cDescendantsAndSelfu003ec__Iterator99 variable = null;
			return variable;
		}

		public static IEnumerable<GameObject> DescendantsAndSelf(this GameObject origin)
		{
			return GameObjectExtensions.DescendantsCore(origin, null, true);
		}

		public static IEnumerable<GameObject> DescendantsAndSelf(this GameObject origin, string name)
		{
			return GameObjectExtensions.DescendantsCore(origin, name, true);
		}

		[DebuggerHidden]
		private static IEnumerable<GameObject> DescendantsCore(GameObject origin, string nameFilter, bool withSelf)
		{
			GameObjectExtensions.u003cDescendantsCoreu003ec__IteratorA1 variable = null;
			return variable;
		}

		public static void Destroy(this IEnumerable<GameObject> source, bool useDestroyImmediate = false)
		{
			foreach (GameObject gameObject in new List<GameObject>(source))
			{
				gameObject.Destroy(useDestroyImmediate);
			}
		}

		public static void Destroy(this GameObject self, bool useDestroyImmediate = false)
		{
			if (self == null)
			{
				return;
			}
			self.SetActive(false);
			self.transform.parent = null;
			self.transform.SetParent(null);
			if (!useDestroyImmediate)
			{
				UnityEngine.Object.Destroy(self);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(self);
			}
		}

		public static GameObject MoveToAfterSelf(this GameObject parent, GameObject child, TransformMoveType moveType = 2, bool? setActive = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex() + 1;
			gameObject.MoveToLast(child, moveType, setActive);
			child.transform.SetSiblingIndex(siblingIndex);
			return child;
		}

		public static List<GameObject> MoveToAfterSelf(this GameObject parent, IEnumerable<GameObject> childs, TransformMoveType moveType = 2, bool? setActive = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex() + 1;
			List<GameObject> last = gameObject.MoveToLast(childs, moveType, setActive);
			for (int i = last.Count - 1; i >= 0; i--)
			{
				last[i].transform.SetSiblingIndex(siblingIndex);
			}
			return last;
		}

		public static GameObject MoveToBeforeSelf(this GameObject parent, GameObject child, TransformMoveType moveType = 2, bool? setActive = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex();
			gameObject.MoveToLast(child, moveType, setActive);
			child.transform.SetSiblingIndex(siblingIndex);
			return child;
		}

		public static List<GameObject> MoveToBeforeSelf(this GameObject parent, IEnumerable<GameObject> childs, TransformMoveType moveType = 2, bool? setActive = null)
		{
			GameObject gameObject = parent.Parent();
			if (gameObject == null)
			{
				throw new InvalidOperationException("The parent root is null");
			}
			int siblingIndex = parent.transform.GetSiblingIndex();
			List<GameObject> last = gameObject.MoveToLast(childs, moveType, setActive);
			for (int i = last.Count - 1; i >= 0; i--)
			{
				last[i].transform.SetSiblingIndex(siblingIndex);
			}
			return last;
		}

		public static GameObject MoveToFirst(this GameObject parent, GameObject child, TransformMoveType moveType = 2, bool? setActive = null)
		{
			parent.MoveToLast(child, moveType, setActive);
			child.transform.SetAsFirstSibling();
			return child;
		}

		public static List<GameObject> MoveToFirst(this GameObject parent, IEnumerable<GameObject> childs, TransformMoveType moveType = 2, bool? setActive = null)
		{
			List<GameObject> last = parent.MoveToLast(childs, moveType, setActive);
			for (int i = last.Count - 1; i >= 0; i--)
			{
				last[i].transform.SetAsFirstSibling();
			}
			return last;
		}

		public static GameObject MoveToLast(this GameObject parent, GameObject child, TransformMoveType moveType = 2, bool? setActive = null)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			Transform transforms = child.transform;
			RectTransform rectTransform = transforms as RectTransform;
			if (rectTransform == null)
			{
				Transform transforms1 = parent.transform;
				transforms.parent = transforms1;
				switch (moveType)
				{
					case TransformMoveType.FollowParent:
					{
						transforms.localPosition = transforms1.localPosition;
						transforms.localScale = transforms1.localScale;
						transforms.localRotation = transforms1.localRotation;
						break;
					}
					case TransformMoveType.Origin:
					{
						transforms.localPosition = Vector3.zero;
						transforms.localScale = Vector3.one;
						transforms.localRotation = Quaternion.identity;
						break;
					}
					case TransformMoveType.DoNothing:
					{
						break;
					}
					default:
					{
						goto case TransformMoveType.DoNothing;
					}
				}
			}
			else
			{
				rectTransform.SetParent(parent.transform, false);
			}
			child.layer = parent.layer;
			if (setActive.HasValue)
			{
				child.SetActive(setActive.Value);
			}
			return child;
		}

		public static List<GameObject> MoveToLast(this GameObject parent, IEnumerable<GameObject> childs, TransformMoveType moveType = 2, bool? setActive = null)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			if (childs == null)
			{
				throw new ArgumentNullException("childs");
			}
			List<GameObject> gameObjects = new List<GameObject>();
			IEnumerator<GameObject> enumerator = childs.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GameObject current = enumerator.Current;
					bool? nullable = null;
					gameObjects.Add(parent.MoveToLast(current, moveType, nullable));
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			return gameObjects;
		}

		[DebuggerHidden]
		public static IEnumerable<T> OfComponent<T>(this IEnumerable<GameObject> source)
		where T : Component
		{
			GameObjectExtensions.u003cOfComponentu003ec__Iterator9E<T> variable = null;
			return variable;
		}

		public static GameObject Parent(this GameObject origin)
		{
			if (origin == null)
			{
				return null;
			}
			Transform transforms = origin.transform.parent;
			if (transforms == null)
			{
				return null;
			}
			return transforms.gameObject;
		}
	}
}