using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
	[HideInInspector] public UnityEvent OnAllEnemiesDeath;

	[SerializeField] GameObject[] obstaclePrefab;
	[SerializeField] GameObject[] enemiesPrefab;

	[SerializeField] Transform obstacleSpawnPoint;
	[SerializeField] Transform enemiesSpawnPoint;

	float[] obsDeltaDistances;
	List<GameObject> enemiesList = new List<GameObject>();
	bool stopSpawning;
	bool endlessRun;

	List<List<GameObject>> obstaclesInstanciated; 

	// Start is called before the first frame update
	void Start()
    {
		stopSpawning = false;
		endlessRun = LevelManager.Instance.LevelSettings.endlessRun;
		obstaclesInstanciated = new List<List<GameObject>>();
		obsDeltaDistances = new float[obstaclePrefab.Length];

		InitialEnemiesSpawn();
		InitialObstaclesSpawn();

		StartCoroutine(ObstacleSpawn());

		if (endlessRun)
			StartCoroutine(EnemiesRespawn());
		else
		{
			GameManager.Instance.OnReachingFinishLine.AddListener(StopSpawning);
			StartCoroutine(CheckForEnemiesDeath());
		}

		GameManager.Instance.OnGameOver.AddListener(StopSpawning);
		GameManager.Instance.OnPlayerWin.AddListener(StopSpawning);
		FindObjectOfType<PlayerBehaviour>().OnPlayerDied.AddListener(StopSpawning);
	}
	
	// Update is called once per frame
	void Update()
    {

    }

	private void InitialEnemiesSpawn()
	{
		int enemiesCount = LevelManager.Instance.LevelSettings.numberOfEnemies;
		int rows = Mathf.FloorToInt(enemiesCount / 3);
		int res = enemiesCount % 3;

		Vector3 spawnPos = enemiesSpawnPoint.position + new Vector3(2.5f, 1f, 2.5f);
		for (int i = 0; i < rows; i++)
		{
			for(int j = 0; j < 3; j++)
			{
				enemiesList.Add(Instantiate(enemiesPrefab[0], (spawnPos - new Vector3(j * 2.5f, 0f, i * 2.5f)), Quaternion.identity));
			}
		}
		for (int k = 0; k < res; k++)
		{
			enemiesList.Add(Instantiate(enemiesPrefab[0], (spawnPos - new Vector3(k * 2.5f, 0f, 3 * 2.5f)), Quaternion.identity));
		}
	}

	private void InitialObstaclesSpawn()
	{
		for(int i = 0; i < obstaclePrefab.Length; i++)
		{
			List<GameObject> tempList = new List<GameObject>();
			for(int j = 0; j < 5; j++)
			{
				tempList.Add(Instantiate(obstaclePrefab[i], obstacleSpawnPoint.position, Quaternion.identity));
				tempList[j].SetActive(false);
			}
			obstaclesInstanciated.Add(tempList);
			obsDeltaDistances[i] = obstaclePrefab[i].GetComponentInChildren<Obstacle>().DeltaDistance;
		}
	}

	private void StopSpawning()
	{
		stopSpawning = true;
	}

	IEnumerator ObstacleSpawn()
	{
		float lastSpawnPoint = 0f;
		int obstacleIndex = -1;
		bool spawnToLeftSide = true;

		while (true)
		{
			if (stopSpawning)
				break;

			float deltaDistance = GameManager.Instance.DistanceWalked - lastSpawnPoint;

			if (obstacleIndex <= 0)
				obstacleIndex = UnityEngine.Random.Range(0, obstaclePrefab.Length);

			if (deltaDistance >= obsDeltaDistances[obstacleIndex])
			{
				spawnToLeftSide = !spawnToLeftSide;

				IEnumerator<GameObject> enumerator = obstaclesInstanciated[obstacleIndex].GetEnumerator();

				while (enumerator.MoveNext())
				{
					GameObject obs = enumerator.Current as GameObject;
					if (!obs.activeSelf || !obs.transform.GetChild(0).gameObject.activeSelf || obs.transform.position.z + 150f < obstacleSpawnPoint.position.z)
					{
						obs.SetActive(true);
						obs.transform.GetChild(0).gameObject.SetActive(true);
						ReplaceObject(ref obs, spawnToLeftSide);
						lastSpawnPoint = GameManager.Instance.DistanceWalked;
						break;
					}
				}
				enumerator.Reset();
				obstacleIndex = 0;
			}

			yield return null;
		}
		
	}

	private void ReplaceObject(ref GameObject obs, bool spawnToLeftSide)
	{
		obs.transform.position = obstacleSpawnPoint.position;
		obs.transform.rotation = (spawnToLeftSide) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity;
	}
	
	IEnumerator EnemiesRespawn()
	{
		while (true)
		{
			if (stopSpawning)
			{
				StopAllEnemies();
				break;
			}
			foreach (GameObject enemy in enemiesList)
			{
				if (enemy.TryGetComponent(out MinionBehaviour minion))
				{
					if (minion.ActorIsDead)
						minion.Resurrect();
				}
			}
			yield return new WaitForSeconds(1f);
		}
	}

	private void StopAllEnemies()
	{
		StopAllCoroutines();
		foreach(GameObject o in enemiesList)
		{
			o.GetComponent<Actor>().StopRunning();
			Debug.Log("Stop");
		}
	}

	IEnumerator CheckForEnemiesDeath()
	{
		bool allDeath = false;
		while (!allDeath)
		{
			yield return new WaitForSeconds(1f);
			bool res = true;
			foreach(GameObject o in enemiesList)
			{
				if(o.TryGetComponent(out MinionBehaviour minion))
				{
					res &= minion.ActorIsDead;
				}
			}
			allDeath = res;
		}
		Debug.Log("All death");
		OnAllEnemiesDeath.Invoke();
	}
}
