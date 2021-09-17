using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Shooter : MonoBehaviour, IShooter
{
	[SerializeField] WeaponsSO weaponStats;

	UnityAction LevelStartListener;
	UnityAction StopShootingListener;

	float timer = 0f;
	public List<Actor> EnemiesList { get; private set; } = new List<Actor>();

	bool startShooting;

	private void Awake()
	{
		LevelStartListener += EnableShooting;
		StopShootingListener += StopShooting;
	}

	private void Start()
	{
		startShooting = false;
		FetchEnemies();
	}

	private void OnEnable()
	{
		EventManager.StartListening(EventsNameList.LevelStarted, LevelStartListener);
		EventManager.StartListening(EventsNameList.PlayerDeath, StopShootingListener);
		EventManager.StartListening(EventsNameList.AllEnemiesDeath, StopShootingListener);
		EventManager.StartListening(EventsNameList.LevelComplete, StopShootingListener);
	}

	private void OnDisable()
	{
		EventManager.StopListening(EventsNameList.LevelStarted, LevelStartListener);
		EventManager.StopListening(EventsNameList.PlayerDeath, StopShootingListener);
		EventManager.StopListening(EventsNameList.AllEnemiesDeath, StopShootingListener);
		EventManager.StopListening(EventsNameList.LevelComplete, StopShootingListener);
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

	private void EnableShooting()
	{
		startShooting = true;
	}

	private void StopShooting()
	{
		startShooting = false;
	}

	//public void SetShooting(bool val = true)
	//{
	//	startShooting = val;
	//}

	public void Shoot(IDamagable dam)
	{
		dam.TakeDamage(weaponStats.damage);

		//Gestire qua i vari vfx...
	}
}
