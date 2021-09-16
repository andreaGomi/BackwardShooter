using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class Shooter : MonoBehaviour, IShooter
{
	[SerializeField] WeaponsSO weaponStats;

	float timer = 0f;
	public List<Actor> EnemiesList { get; private set; } = new List<Actor>();

	bool startShooting;
	
	private void Start()
	{
		GameManager.Instance.OnLevelStart.AddListener(OnLevelStart);

		SphereCollider col = GetComponent<SphereCollider>();
		col.isTrigger = true;
		col.radius = weaponStats.range;

		//FillList();
		startShooting = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent(out Actor actor))
		{
			if (!EnemiesList.Contains(actor))
				EnemiesList.Add(actor);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent(out Actor actor))
		{
			if (EnemiesList.Contains(actor))
				EnemiesList.Remove(actor);
		}
	}

	private void Update()
	{
		ManageShooting();
	}

	//private void FillList()
	//{
	//	GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
	//	for (int i = 0; i < enemies.Length; i++)
	//	{
	//		EnemiesList.Add(enemies[i].GetComponent<Actor>());
	//	}
	//}

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

	private void OnLevelStart()
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
