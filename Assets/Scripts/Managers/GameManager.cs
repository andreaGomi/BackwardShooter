using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI counterText;

	public float DistanceWalked { get; private set; }

	private static GameManager gameManager = null;
	public static GameManager Instance
	{
		get
		{
			if (!gameManager)
			{
				gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

				if (!gameManager)
				{
					Debug.Log("No GameManager in current scene");
				}
			}
			return gameManager;
		}
	}

	float countDown;
	Transform playerTransform;
	Vector3 playerInitPos;
	float stopFollowDistance;

	private void Awake()
	{
		Instance.countDown = LevelManager.Instance.LevelSettings.levelCountDown;
		Instance.DistanceWalked = 0f;
		StartCoroutine(StartCountDown());

		Instance.playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		Instance.playerInitPos = playerTransform.position;
	}

	private void Start()
	{
		if (!LevelManager.Instance.LevelSettings.endlessRun)
		{
			stopFollowDistance = Mathf.Clamp(LevelManager.Instance.LevelSettings.levelLength_mt - LevelManager.Instance.LevelSettings.stopFollowPlayerAt_mt,
											0f,
											LevelManager.Instance.LevelSettings.levelLength_mt);

			StartCoroutine(CheckDistanceWalked());
		}
	}

	IEnumerator StartCountDown()
	{
		float counter = countDown;

		yield return null;

		while (counter > 0)
		{
			counterText.text = Mathf.CeilToInt(counter).ToString();
			yield return null;
			counter -= Time.deltaTime;
		}

		counterText.text = "RUN!";

		yield return new WaitForSeconds(.5f);

		counterText.enabled = false;
		EventManager.TriggerEvent(EventsNameList.LevelStarted);
	}

	private void Update()
	{
		DistanceWalked = (playerTransform.position - playerInitPos).magnitude;
	}

	IEnumerator CheckDistanceWalked()
	{
		while (true)
		{
			if(DistanceWalked >= stopFollowDistance)
			{
				EventManager.TriggerEvent(EventsNameList.ApproachingFinishLine);
				break;
			}

			yield return new WaitForSeconds(.5f);
		}
	}
}
