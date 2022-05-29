using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BetterList<T>
{
	public T[] buffer;

	public int size;

	[DebuggerHidden]
	public T this[int i]
	{
		get
		{
			return this.buffer[i];
		}
		set
		{
			this.buffer[i] = value;
		}
	}

	public BetterList()
	{
	}

	public void Add(T item)
	{
		if (this.buffer == null || this.size == (int)this.buffer.Length)
		{
			this.AllocateMore();
		}
		T[] tArray = this.buffer;
		BetterList<T> betterList = this;
		int num = betterList.size;
		int num1 = num;
		betterList.size = num + 1;
		tArray[num1] = item;
	}

	private void AllocateMore()
	{
		T[] tArray = (this.buffer == null ? new T[32] : new T[Mathf.Max((int)this.buffer.Length << 1, 32)]);
		if (this.buffer != null && this.size > 0)
		{
			this.buffer.CopyTo(tArray, 0);
		}
		this.buffer = tArray;
	}

	public void Clear()
	{
		this.size = 0;
	}

	public bool Contains(T item)
	{
		if (this.buffer == null)
		{
			return false;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return true;
			}
		}
		return false;
	}

	[DebuggerHidden]
	[DebuggerHidden]
	[DebuggerStepThrough]
	public IEnumerator<T> GetEnumerator()
	{
		BetterList<T>.u003cGetEnumeratoru003ec__IteratorC2 variable = null;
		return variable;
	}

	public int IndexOf(T item)
	{
		if (this.buffer == null)
		{
			return -1;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return i;
			}
		}
		return -1;
	}

	public void Insert(int index, T item)
	{
		if (this.buffer == null || this.size == (int)this.buffer.Length)
		{
			this.AllocateMore();
		}
		if (index <= -1 || index >= this.size)
		{
			this.Add(item);
		}
		else
		{
			for (int i = this.size; i > index; i--)
			{
				this.buffer[i] = this.buffer[i - 1];
			}
			this.buffer[index] = item;
			this.size++;
		}
	}

	public T Pop()
	{
		if (this.buffer == null || this.size == 0)
		{
			return default(T);
		}
		BetterList<T> betterList = this;
		int num = betterList.size - 1;
		int num1 = num;
		betterList.size = num;
		T t = this.buffer[num1];
		this.buffer[this.size] = default(T);
		return t;
	}

	public void Release()
	{
		this.size = 0;
		this.buffer = null;
	}

	public bool Remove(T item)
	{
		if (this.buffer != null)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < this.size; i++)
			{
				if (@default.Equals(this.buffer[i], item))
				{
					this.size--;
					this.buffer[i] = default(T);
					for (int j = i; j < this.size; j++)
					{
						this.buffer[j] = this.buffer[j + 1];
					}
					this.buffer[this.size] = default(T);
					return true;
				}
			}
		}
		return false;
	}

	public void RemoveAt(int index)
	{
		if (this.buffer != null && index > -1 && index < this.size)
		{
			this.size--;
			this.buffer[index] = default(T);
			for (int i = index; i < this.size; i++)
			{
				this.buffer[i] = this.buffer[i + 1];
			}
			this.buffer[this.size] = default(T);
		}
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public void Sort(BetterList<T>.CompareFunc comparer)
	{
		int num = 0;
		int num1 = this.size - 1;
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = num; i < num1; i++)
			{
				if (comparer(this.buffer[i], this.buffer[i + 1]) > 0)
				{
					T t = this.buffer[i];
					this.buffer[i] = this.buffer[i + 1];
					this.buffer[i + 1] = t;
					flag = true;
				}
				else if (!flag)
				{
					num = (i != 0 ? i - 1 : 0);
				}
			}
		}
	}

	public T[] ToArray()
	{
		this.Trim();
		return this.buffer;
	}

	private void Trim()
	{
		if (this.size <= 0)
		{
			this.buffer = null;
		}
		else if (this.size < (int)this.buffer.Length)
		{
			T[] tArray = new T[this.size];
			for (int i = 0; i < this.size; i++)
			{
				tArray[i] = this.buffer[i];
			}
			this.buffer = tArray;
		}
	}

	public delegate int CompareFunc(T left, T right);
}