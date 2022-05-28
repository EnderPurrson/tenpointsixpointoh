using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prime31
{
	public class ThreadingCallbackHelper : MonoBehaviour
	{
		private List<Action> _actions = new List<Action>();

		private List<Action> _currentActions = new List<Action>();

		public void addActionToQueue(Action action)
		{
			lock (_actions)
			{
				_actions.Add(action);
			}
		}

		private void Update()
		{
			lock (_actions)
			{
				_currentActions.AddRange((global::System.Collections.Generic.IEnumerable<Action>)_actions);
				_actions.Clear();
			}
			for (int i = 0; i < _currentActions.get_Count(); i++)
			{
				_currentActions.get_Item(i).Invoke();
			}
			_currentActions.Clear();
		}

		public void disableIfEmpty()
		{
			lock (_actions)
			{
				if (_actions.get_Count() == 0)
				{
					base.enabled = false;
				}
			}
		}
	}
}
