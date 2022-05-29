using System;
using System.Collections.Generic;
using UnityEngine;

public class AGSGameDataMap : AGSSyncable
{
	public AGSGameDataMap(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	public AGSGameDataMap(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	public AGSSyncableAccumulatingNumber GetAccumulatingNumber(string name)
	{
		return base.GetAGSSyncable<AGSSyncableAccumulatingNumber>(AGSSyncable.SyncableMethod.getAccumulatingNumber, name);
	}

	public HashSet<string> GetAccumulatingNumberKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getAccumulatingNumberKeys);
	}

	public AGSSyncableDeveloperString getDeveloperString(string name)
	{
		return base.GetAGSSyncable<AGSSyncableDeveloperString>(AGSSyncable.SyncableMethod.getDeveloperString, name);
	}

	public HashSet<string> getDeveloperStringKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getDeveloperStringKeys);
	}

	public AGSSyncableNumber GetHighestNumber(string name)
	{
		return base.GetAGSSyncable<AGSSyncableNumber>(AGSSyncable.SyncableMethod.getHighestNumber, name);
	}

	public HashSet<string> GetHighestNumberKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getHighestNumberKeys);
	}

	public AGSSyncableNumberList GetHighNumberList(string name)
	{
		return base.GetAGSSyncable<AGSSyncableNumberList>(AGSSyncable.SyncableMethod.getHighNumberList, name);
	}

	public HashSet<string> GetHighNumberListKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getHighNumberListKeys);
	}

	public AGSSyncableNumber GetLatestNumber(string name)
	{
		return base.GetAGSSyncable<AGSSyncableNumber>(AGSSyncable.SyncableMethod.getLatestNumber, name);
	}

	public HashSet<string> GetLatestNumberKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getLatestNumberKeys);
	}

	public AGSSyncableNumberList GetLatestNumberList(string name)
	{
		return base.GetAGSSyncable<AGSSyncableNumberList>(AGSSyncable.SyncableMethod.getLatestNumberList, name);
	}

	public HashSet<string> GetLatestNumberListKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getLatestNumberListKeys);
	}

	public AGSSyncableString GetLatestString(string name)
	{
		return base.GetAGSSyncable<AGSSyncableString>(AGSSyncable.SyncableMethod.getLatestString, name);
	}

	public HashSet<string> GetLatestStringKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getLatestStringKeys);
	}

	public AGSSyncableStringList GetLatestStringList(string name)
	{
		return base.GetAGSSyncable<AGSSyncableStringList>(AGSSyncable.SyncableMethod.getLatestStringList, name);
	}

	public HashSet<string> GetLatestStringListKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getLatestStringListKeys);
	}

	public AGSSyncableNumber GetLowestNumber(string name)
	{
		return base.GetAGSSyncable<AGSSyncableNumber>(AGSSyncable.SyncableMethod.getLowestNumber, name);
	}

	public HashSet<string> GetLowestNumberKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getLowestNumberKeys);
	}

	public AGSSyncableNumberList GetLowNumberList(string name)
	{
		return base.GetAGSSyncable<AGSSyncableNumberList>(AGSSyncable.SyncableMethod.getLowNumberList, name);
	}

	public HashSet<string> GetLowNumberListKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getLowNumberListKeys);
	}

	public AGSGameDataMap GetMap(string name)
	{
		return base.GetAGSSyncable<AGSGameDataMap>(AGSSyncable.SyncableMethod.getMap, name);
	}

	public HashSet<string> GetMapKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getMapKeys);
	}

	public AGSSyncableStringSet GetStringSet(string name)
	{
		return base.GetAGSSyncable<AGSSyncableStringSet>(AGSSyncable.SyncableMethod.getStringSet, name);
	}

	public HashSet<string> GetStringSetKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getStringSetKeys);
	}
}