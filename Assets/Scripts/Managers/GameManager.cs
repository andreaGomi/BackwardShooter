using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI counterText;

	[HideInInspector] public UnityEvent OnLevelStart;

	private static GameManager gameManager = null;
	public static GameManager Instance
	{
		get
		{
			if (!gameManager)
			{
				gameManager = FindObjectOfType<GameManager>();

				if (!gameManager)
				{
					Debug.Log("No GameManager in current scene");
				}
			}
			return gameManager;
		}
	}

	float countDown;

	private void Start()
	{
		Instance.countDown = LevelManager.Instance.levelSettings.levelCountDown;
		StartCoroutine(StartCountDown());
	}

	IEnumerator StartCountDown()
	{
		float counter = countDown;

		yield return null;

		while (counter > 0)
		{
			counterText.text = ((int)counter).ToString();
			yield return null;
			counter -= Time.deltaTime;
		}

		counterText.enabled = false;
		OnLevelStart.Invoke();
	}

	public static void PlayerDiedListener()
	{

	}
}
