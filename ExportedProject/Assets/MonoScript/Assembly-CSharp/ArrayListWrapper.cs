using Rilisoft;
using System;
using System.Collections;
using System.Reflection;

public class ArrayListWrapper
{
	private ArrayList _list = new ArrayList();

	public int Count
	{
		get
		{
			int count;
			using (ArrayListChecker arrayListChecker = new ArrayListChecker(this._list, "_list"))
			{
				count = this._list.Count;
			}
			return count;
		}
	}

	public object this[int index]
	{
		get
		{
			object item;
			using (ArrayListChecker arrayListChecker = new ArrayListChecker(this._list, "_list"))
			{
				item = this._list[index];
			}
			return item;
		}
		set
		{
			using (ArrayListChecker arrayListChecker = new ArrayListChecker(this._list, "_list"))
			{
				this._list[index] = value;
			}
		}
	}

	public ArrayListWrapper()
	{
	}

	public int Add(object item)
	{
		int num;
		using (ArrayListChecker arrayListChecker = new ArrayListChecker(this._list, "_list"))
		{
			num = this._list.Add(item);
		}
		return num;
	}

	public void AddRange(ICollection c)
	{
		using (ArrayListChecker arrayListChecker = new ArrayListChecker(this._list, "_list"))
		{
			this._list.AddRange(c);
		}
	}

	public bool Contains(object item)
	{
		bool flag;
		using (ArrayListChecker arrayListChecker = new ArrayListChecker(this._list, "_list"))
		{
			flag = this._list.Contains(item);
		}
		return flag;
	}

	public void Insert(int index, object obj)
	{
		using (ArrayListChecker arrayListChecker = new ArrayListChecker(this._list, "_list"))
		{
			this._list.Insert(index, obj);
		}
	}

	public void RemoveAt(int index)
	{
		using (ArrayListChecker arrayListChecker = new ArrayListChecker(this._list, "_list"))
		{
			this._list.RemoveAt(index);
		}
	}

	public Array ToArray(Type type)
	{
		Array array;
		using (ArrayListChecker arrayListChecker = new ArrayListChecker(this._list, "_list"))
		{
			array = this._list.ToArray(type);
		}
		return array;
	}
}