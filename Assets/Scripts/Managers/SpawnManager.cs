using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
	[SerializeField] GameObject[] obstaclePrefab;         
	[SerializeField] GameObject[] enemiesPrefab;           

	[SerializeField] Transform obstacleSpawnPoint;	
	[SerializeField] Transform enemiesSpawnPoint;

	UnityAction StopSpawningListener;
	UnityAction UpdateEnemiesCounterListener;

	float[] obsDeltaDistances;								//copy of obstacle delta distance, for every obstacle in obstacles prefab array
	List<GameObject> enemiesList = new List<GameObject>();	//list of all enemies istanciated
	bool endlessRun;										//copy of level settings for endless run
	int enemiesDeathCounter;								//player kills counter. Eventually triggers "AllnemiesDeath" event if all enemies got killed

	List<List<GameObject>> obstaclesInstanciated;			//List that contains, for each type of obstacles in obstacles prefab, its istances
	bool stopSpawning = false;										

	private void Awake()
	{
		endlessRun = LevelManager.Instance.LevelSettings.endlessRun;
		StopSpawningListener += StopSpawning;
		UpdateEnemiesCounterListener += UpdateEnemiesDeathCounter;
	}

	// Start is called before the first frame update
	void Start()
    {
		obstaclesInstanciated = new List<List<GameObject>>();
		obsDeltaDistances = new float[obstaclePrefab.Length];
		enemiesDeathCounter = 0;

		InitialEnemiesSpawn();
		InitialObstaclesSpawn();

		StartCoroutine(ObstacleSpawn());

		if (endlessRun)
			StartCoroutine(EnemiesRespawn());
		
	}

	private void OnEnable()
	{
		if (!endlessRun)
		{
			EventManager.StartListening(EventsNameList.ApproachingFinishLine, StopSpawningListener);
			EventManager.StartListening(EventsNameList.LevelComplete, StopSpawningListener);
			EventManager.StartListening(EventsNameList.AllEnemiesDeath, StopSpawningListener);
			EventManager.StartListening(EventsNameList.AnEnemyIsDead, UpdateEnemiesCounterListener);
		}
		EventManager.StartListening(EventsNameList.PlayerDeath, StopSpawningListener);
		
	}

	private void OnDisable()
	{
		if (!endlessRun)
		{
			EventManager.StopListening(EventsNameList.ApproachingFinishLine, StopSpawningListener);
			EventManager.StopListening(EventsNameList.LevelComplete, StopSpawningListener);
			EventManager.StopListening(EventsNameList.AllEnemiesDeath, StopSpawningListener);
			EventManager.StopListening(EventsNameList.AnEnemyIsDead, UpdateEnemiesCounterListener);
		}
		EventManager.StopListening(EventsNameList.PlayerDeath, StopSpawningListener);
	}

	//Listener for enemy death
	private void UpdateEnemiesDeathCounter()
	{
		enemiesDeathCounter++;
		if (enemiesDeathCounter == enemiesList.Count && !endlessRun)
			EventManager.TriggerEvent(EventsNameList.AllEnemiesDeath);
	}

	//Spawn a number of enemies specified in level settings SO, in a matrix-like order
	private void InitialEnemiesSpawn()
	{
		int enemiesCount = LevelManager.Instance.LevelSettings.numberOfEnemies;			//total number of enemies to spawn
		int rows = Mathf.FloorToInt(enemiesCount / 3);									//rows of the matrix
		int res = enemiesCount % 3;														//cols of the matrix

		Vector3 spawnPos = enemiesSpawnPoint.position + new Vector3(2.5f, 0f, 2.5f);	//starting spawn pos

		//for each rows and cols, instantiate and enemy and add it to the list
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
		//for each type of obstacles in array
		for(int i = 0; i < obstaclePrefab.Length; i++)
		{
			//create a new list  
			List<GameObject> tempList = new List<GameObject>();
			for(int j = 0; j < 5; j++)
			{
				//add obstacle istance to the list
				tempList.Add(Instantiate(obstaclePrefab[i], obstacleSpawnPoint.position, Quaternion.identity));
				tempList[j].SetActive(false);
			}
			//Add created list into main obstacle list
			obstaclesInstanciated.Add(tempList);
			//Save obstacle delta distance
			obsDeltaDistances[i] = obstaclePrefab[i].GetComponentInChildren<Obstacle>().DeltaDistance;
		}
	}

	private void StopSpawning()
	{
		stopSpawning = true;
		StopAllCoroutines();
	}

	IEnumerator ObstacleSpawn()
	{
		float lastSpawnPoint = 0f;		// Z value of the last spawn position, in world space
		int obstacleIndex = -1;			// index of obstacles prefab array 
		bool spawnToLeftSide = true;	// bool to manage spawn rotation

		while (true)
		{
			if (stopSpawning)
				break;

			yield return null;

			//Distance to last spawn point
			float deltaDistance = GameManager.Instance.DistanceWalked - lastSpawnPoint;

			//if obstacleIndex < 0, select randomly another index
			if (obstacleIndex < 0)
				obstacleIndex = Random.Range(0, obstaclePrefab.Length);

			//If there is enough distance between last obstacle spawned and the actual desired spawn position
			if (deltaDistance >= obsDeltaDistances[obstacleIndex])
			{
				//Invert spawn rotation
				spawnToLeftSide = !spawnToLeftSide;

				//Retrive enumerator from the selected obstacle container list
				IEnumerator<GameObject> enumerator = obstaclesInstanciated[obstacleIndex].GetEnumerator();

				while (enumerator.MoveNext())
				{
					GameObject obs = enumerator.Current as GameObject;
					//If obs is not active OR its child isn't OR its position is far enough from the player (so it's offscreen)
					if (!obs.activeSelf || !obs.transform.GetChild(0).gameObject.activeSelf || obs.transform.position.z + 150f < obstacleSpawnPoint.position.z)
					{
						//Re-enable the obstacle
						obs.SetActive(true);
						obs.transform.GetChild(0).gameObject.SetActive(true);
						//Update its position
						ReplaceObject(ref obs, spawnToLeftSide);
						//Update last spawn position
						lastSpawnPoint = GameManager.Instance.DistanceWalked;
						break;
					}
				}
				enumerator.Reset();
				obstacleIndex = -1;
			}
		}
	}

	private void ReplaceObject(ref GameObject obs, bool spawnToLeftSide)
	{
		obs.transform.position = obstacleSpawnPoint.position;
		obs.transform.rotation = (spawnToLeftSide) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity;
	}
	
	//Coroutine called if Endless Run mode is enabled. Resurrect dead enemies every 2 seconds
	IEnumerator EnemiesRespawn()
	{
		while (true)
		{
			if (stopSpawning)
				break;

			foreach (GameObject enemy in enemiesList)
			{
				if (enemy.TryGetComponent(out MinionBehaviour minion))
				{
					if (minion.ActorIsDead)
						minion.Resurrect();
				}
			}
			yield return new WaitForSeconds(2f);
		}
	}
}
