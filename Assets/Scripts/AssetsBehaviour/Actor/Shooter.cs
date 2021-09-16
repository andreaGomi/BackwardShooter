using UnityEngine;
using System.Collections.Generic;
using System;

public class Shooter : MonoBehaviour, IShooter
{
	[SerializeField] WeaponsSO weaponStats;

	float timer = 0f;
	public List<Actor> EnemiesList { get; private set; } = new List<Actor>();

	bool startShooting;

	private void Start()
	{
		GameManager.Instance.OnLevelStart.AddListener(LevelStartListener);
		startShooting = false;
		FetchEnemies();
	}

	private void Update()
	{
		ManageShooting();
	}

	private void FetchEnemies()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach(GameObject o in enemies)
		{
			EnemiesList.Add(o.GetComponent<Actor>());
		}
	}

	private void ManageShooting()
	{
		if (!startShooting)
			return;

		timer += Time.deltaTime;
		if(timer >= weaponStats.fireRate && EnemiesList.Count > 0)
		{
			timer = 0f;
			Actor target = FindClosestEnemy();
			if(target && target.TryGetComponent(out IDamagable dam))
			{
				Shoot(dam);
			}
		}
	}
	
	private Actor FindClosestEnemy()
	{
		float lastDistance = float.MaxValue;
		Actor res = null;
		foreach(Actor a in EnemiesList)
		{
			float distance = (a.gameObject.GetComponent<Transform>().position - transform.position).magnitude;
			if(distance < lastDistance && distance <= weaponStats.range)
			{
				res = a;
				lastDistance = distance;
			}
		}
		return res;
	}

	private void LevelStartListener()
	{
		startShooting = true;
	}

	public void SetShooting(bool val = true)
	{
		startShooting = val;
	}

	public void Shoot(IDamagable dam)
	{
		dam.TakeDamage(weaponStats.damage);

		//Gestire qua i vari vfx...
	}
}
