using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// This class manage the shooting at enemies and holds weapons SO stats.
/// </summary>
public class Shooter : MonoBehaviour, IShooter
{
	[SerializeField] WeaponsSO weaponStats; //SO weapon stats
	//public WeaponsSO WeaponStats { get { return weaponStats; } set { weaponStats = value; } }

	UnityAction LevelStartListener;
	UnityAction StopShootingListener;

	float timer = 0f;	//Timer for manage weapon's fire rate
	public List<Actor> EnemiesList { get; private set; } = new List<Actor>();	//List that holds all current enemies within the level
	public int NearestEnemyIndex { get; private set; } = 0;	//List index of the current enemy which is the nearest to the player

	bool startShooting;	//Is legal to shoot or not

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
		int index = 0;
		foreach(Actor a in EnemiesList)
		{
			float distance = (a.gameObject.GetComponent<Transform>().position - transform.position).magnitude;
			if(distance < lastDistance && distance <= weaponStats.range)
			{
				res = a;
				lastDistance = distance;
				NearestEnemyIndex = index;
			}

			index++;
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

	public void Shoot(IDamagable dam)
	{
		dam.TakeDamage(weaponStats.damage);

		//Gestire qua i vari vfx...
	}

	public Transform GetNearestEnemytransform()
	{
		if (EnemiesList.Count > 0)
			return EnemiesList[NearestEnemyIndex].transform;
		else
			return null;
	}
}
