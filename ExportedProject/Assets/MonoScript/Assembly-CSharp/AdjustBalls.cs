using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AdjustBalls : MonoBehaviour
{
	public AdjustBalls()
	{
	}

	public void DoSomethingWithTheData(JsonData[] ssObjects)
	{
		OptionalMiddleStruct str = new OptionalMiddleStruct();
		for (int i = 0; i < (int)ssObjects.Length; i++)
		{
			if (ssObjects[i].Keys.Contains("name"))
			{
				str.name = ssObjects[i]["name"].ToString();
			}
			if (ssObjects[i].Keys.Contains("color"))
			{
				str.color = this.GetColor(ssObjects[i]["color"].ToString());
			}
			if (ssObjects[i].Keys.Contains("drag"))
			{
				str.drag = float.Parse(ssObjects[i]["drag"].ToString());
			}
			this.UpdateObjectValues(str);
		}
	}

	private Color GetColor(string color)
	{
		Color color1;
		int num;
		string str = color;
		if (str != null)
		{
			if (AdjustBalls.u003cu003ef__switchu0024map3 == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(11)
				{
					{ "black", 0 },
					{ "blue", 1 },
					{ "clear", 2 },
					{ "cyan", 3 },
					{ "gray", 4 },
					{ "green", 5 },
					{ "grey", 6 },
					{ "magenta", 7 },
					{ "red", 8 },
					{ "white", 9 },
					{ "yellow", 10 }
				};
				AdjustBalls.u003cu003ef__switchu0024map3 = strs;
			}
			if (AdjustBalls.u003cu003ef__switchu0024map3.TryGetValue(str, out num))
			{
				switch (num)
				{
					case 0:
					{
						color1 = Color.black;
						break;
					}
					case 1:
					{
						color1 = Color.blue;
						break;
					}
					case 2:
					{
						color1 = Color.clear;
						break;
					}
					case 3:
					{
						color1 = Color.cyan;
						break;
					}
					case 4:
					{
						color1 = Color.gray;
						break;
					}
					case 5:
					{
						color1 = Color.green;
						break;
					}
					case 6:
					{
						color1 = Color.grey;
						break;
					}
					case 7:
					{
						color1 = Color.magenta;
						break;
					}
					case 8:
					{
						color1 = Color.red;
						break;
					}
					case 9:
					{
						color1 = Color.white;
						break;
					}
					case 10:
					{
						color1 = Color.yellow;
						break;
					}
					default:
					{
						color1 = Color.grey;
						return color1;
					}
				}
			}
			else
			{
				color1 = Color.grey;
				return color1;
			}
		}
		else
		{
			color1 = Color.grey;
			return color1;
		}
		return color1;
	}

	public void ResetBalls()
	{
		OptionalMiddleStruct optionalMiddleStruct = new OptionalMiddleStruct()
		{
			color = Color.white,
			drag = 0f
		};
		string str = "Ball";
		for (int i = 1; i < 4; i++)
		{
			optionalMiddleStruct.name = string.Concat(str, i.ToString());
			this.UpdateObjectValues(optionalMiddleStruct);
		}
	}

	private void UpdateObjectValues(OptionalMiddleStruct container)
	{
		GameObject gameObject = GameObject.Find(container.name);
		gameObject.GetComponent<Renderer>().sharedMaterial.color = container.color;
		gameObject.GetComponent<Rigidbody>().drag = container.drag;
	}
}