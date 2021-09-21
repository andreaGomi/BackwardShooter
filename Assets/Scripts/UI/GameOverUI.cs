using UnityEngine;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// This Class manage the UI after level ended
/// </summary>
public class GameOverUI : MonoBehaviour
{
	[SerializeField] GameObject gameOverPanel;
	[SerializeField] TextMeshProUGUI gameOverText;
	[SerializeField] TextMeshProUGUI distanceText;

	UnityAction GameOverListener;
	UnityAction LevelCompleteListener;
	UnityAction AllEnemiesDiedListener;

	private void Awake()
	{
		GameOverListener += ShowGameOveUI;
		LevelCompleteListener += ShowLevelCompleteUI;
		AllEnemiesDiedListener += ShowAllEnemiesDiedUI;
	}

	private void Start()
	{
		gameOverPanel.SetActive(false);
	}

	private void OnEnable()
	{
		EventManager.StartListening(EventsNameList.LevelComplete, LevelCompleteListener);
		EventManager.StartListening(EventsNameList.AllEnemiesDeath, AllEnemiesDiedListener);
		EventManager.StartListening(EventsNameList.PlayerDeath, GameOverListener);
	}

	private void OnDisable()
	{
		EventManager.StopListening(EventsNameList.LevelComplete, LevelCompleteListener);
		EventManager.StopListening(EventsNameList.AllEnemiesDeath, AllEnemiesDiedListener);
		EventManager.StopListening(EventsNameList.PlayerDeath, GameOverListener);
	}
	
	private void ShowGameOveUI()
	{
		gameOverPanel.SetActive(true);
		gameOverText.text = "GAME OVER\nYou've been cathced!";
		distanceText.text = "Distance run: " + Mathf.CeilToInt(GameManager.Instance.DistanceWalked) + " meter";
	}

	private void ShowLevelCompleteUI()
	{
		gameOverPanel.SetActive(true);
		gameOverText.text = "YOU WIN!\nYou've comlpeted the run!";
		distanceText.text = "Distance run: " + Mathf.CeilToInt(GameManager.Instance.DistanceWalked) + " meter";
	}

	private void ShowAllEnemiesDiedUI()
	{
		gameOverPanel.SetActive(true);
		gameOverText.text = "YOU WIN!\nYou've killed all the enemies!";
		distanceText.text = "Distance run: " + Mathf.CeilToInt(GameManager.Instance.DistanceWalked) + " meter";
	}
}
