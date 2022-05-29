using System;
using UnityEngine;

public class EnemiesLabel : MonoBehaviour
{
	private UILabel _label;

	private ZombieCreator _zombieCreator;

	public EnemiesLabel()
	{
	}

	private void Start()
	{
		bool flag = !Defs.isMulti;
		base.gameObject.SetActive(flag);
		if (!flag)
		{
			return;
		}
		this._label = base.GetComponent<UILabel>();
		this._zombieCreator = GameObject.FindGameObjectWithTag("GameController").GetComponent<ZombieCreator>();
	}

	private void Update()
	{
		this._label.text = string.Format("{0}", ZombieCreator.NumOfEnemisesToKill - this._zombieCreator.NumOfDeadZombies);
	}
}