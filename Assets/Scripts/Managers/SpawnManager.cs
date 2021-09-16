using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	[SerializeField] GameObject[] obstaclePrefab;
	[SerializeField] GameObject[] enemiesPrefab;

	[SerializeField] Transform obstacleSpawnPoint;
	[SerializeField] Transform enemiesSpawnPoint;


	List<GameObject> enemiesList = new List<GameObject>();
	bool stopSpawning;
	bool endlessRun;
	bool waveMode;

	float distanceWalked;
	

	// Start is called before the first frame update
	void Start()
    {
		stopSpawning = false;
		endlessRun = LevelManager.Instance.LevelSettings.endlessRun;
		waveMode = LevelManager.Instance.LevelSettings.waveMode;
		distanceWalked = 0f;

		InitialEnemySpawn();

		if(!endlessRun)
			GameManager.Instance.OnReachingFinishLine.AddListener(OnFinishLineListener);

		if (!waveMode)
			StartCoroutine(ObstacleSpawn());
		else
			StartCoroutine(WaveSpawnMode());
	}

	// Update is called once per frame
	void Update()
    {

    }

	private void InitialEnemySpawn()
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

	private void OnFinishLineListener()
	{
		stopSpawning = true;
	}

	IEnumerator ObstacleSpawn()
	{
		float lastSpawnPoint = 0f;
		int obstacleIndex = -1;
		bool spawnToLeftSide = true;
		Obstacle obsToSpawn = null;

		System.Random prng = new System.Random();

		while (true)
		{
			if (stopSpawning)
				break;

			float deltaDistance = GameManager.Instance.DistanceWalked - lastSpawnPoint;

			if (obstacleIndex < 0)
			{
				obstacleIndex = prng.Next(0, 2);
				obsToSpawn = obstaclePrefab[obstacleIndex].GetComponent<Obstacle>();
			}

			if (obsToSpawn && deltaDistance >= obsToSpawn.DeltaDistance)
			{
				obstacleIndex = -1;
				spawnToLeftSide = !spawnToLeftSide;
				Instantiate(obsToSpawn.gameObject, obstacleSpawnPoint.position, (spawnToLeftSide)? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity);
			}

			yield return null;
		}
	}

	IEnumerator WaveSpawnMode()
	{
		yield return null;
	}
}
