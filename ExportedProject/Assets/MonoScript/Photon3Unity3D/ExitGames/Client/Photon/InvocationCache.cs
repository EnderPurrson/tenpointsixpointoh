using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExitGames.Client.Photon
{
	internal class InvocationCache
	{
		private class CachedOperation
		{
			public int InvocationId
			{
				[CompilerGenerated]
				get
				{
					return _003CInvocationId_003Ek__BackingField;
				}
				[CompilerGenerated]
				set
				{
					_003CInvocationId_003Ek__BackingField = value;
				}
			}

			public Action Action
			{
				[CompilerGenerated]
				get
				{
					return _003CAction_003Ek__BackingField;
				}
				[CompilerGenerated]
				set
				{
					_003CAction_003Ek__BackingField = value;
				}
			}
		}

		private readonly LinkedList<CachedOperation> cache = new LinkedList<CachedOperation>();

		private int nextInvocationId = 1;

		public int NextInvocationId
		{
			get
			{
				return nextInvocationId;
			}
		}

		public int Count
		{
			get
			{
				return cache.get_Count();
			}
		}

		public void Reset()
		{
			lock (cache)
			{
				nextInvocationId = 1;
				cache.Clear();
			}
		}

		public void Invoke(int invocationId, Action action)
		{
			lock (cache)
			{
				if (invocationId < nextInvocationId)
				{
					return;
				}
				if (invocationId == nextInvocationId)
				{
					nextInvocationId++;
					action.Invoke();
					if (cache.get_Count() > 0)
					{
						LinkedListNode<CachedOperation> val = cache.get_First();
						while (val != null && val.get_Value().InvocationId == nextInvocationId)
						{
							nextInvocationId++;
							val.get_Value().Action.Invoke();
							val = val.get_Next();
							cache.RemoveFirst();
						}
					}
					return;
				}
				CachedOperation cachedOperation = new CachedOperation
				{
					InvocationId = invocationId,
					Action = action
				};
				if (cache.get_Count() == 0)
				{
					cache.AddLast(cachedOperation);
					return;
				}
				for (LinkedListNode<CachedOperation> val2 = cache.get_First(); val2 != null; val2 = val2.get_Next())
				{
					if (val2.get_Value().InvocationId > invocationId)
					{
						cache.AddBefore(val2, cachedOperation);
						return;
					}
				}
				cache.AddLast(cachedOperation);
			}
		}
	}
}
