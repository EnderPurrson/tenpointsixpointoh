using System;
using System.Collections;

namespace UnityScript.Lang
{
	[Serializable]
	internal class ListUpdateableEnumerator : IEnumerator
	{
		protected IList _list;

		protected int _current;

		public override object Current
		{
			get
			{
				return _list[_current];
			}
		}

		public ListUpdateableEnumerator(IList list)
		{
			_current = -1;
			_list = list;
		}

		public override void Reset()
		{
			_current = -1;
		}

		public override bool MoveNext()
		{
			checked
			{
				_current++;
				return _current < _list.Count;
			}
		}

		public void Update(object newValue)
		{
			_list[_current] = newValue;
		}
	}
}
