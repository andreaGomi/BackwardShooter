using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Game Manager. Keeps track of distance run and count down timer before level start based on Level Settings SO.
/// </summary>
public class GameManager : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI counterText;       //Count down texts
	[SerializeField] Transform finishLine;

	public float DistanceWalked { get; private set; }	//The distance run by the player

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
	float stopFollowDistance;							//(If endless run mode is disabled) At what distance from finish line follower scripts should stop follow its target 

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
			float distanceToRun = LevelManager.Instance.LevelSettings.levelLength_mt;

			//If level length is less than 130mt, that position the finish line properly.
			if(distanceToRun < 130f)
			{
				Vector3 newFinishLinePos = finishLine.position;
				newFinishLinePos.z = playerInitPos.z + distanceToRun;
				finishLine.position = newFinishLinePos;
			}
			stopFollowDistance = Mathf.Clamp(distanceToRun - 130f, 0f, distanceToRun);

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
		UpdateDistance();
	}

	private void UpdateDistance()
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
