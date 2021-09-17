using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
	[SerializeField] GameObject gameOverPanel;
	[SerializeField] TextMeshProUGUI gameOverText;
	[SerializeField] TextMeshProUGUI distanceText;

	private void Start()
	{
		gameOverPanel.SetActive(false);
		GameManager.Instance.OnGameOver.AddListener(GameOverListener);
		GameManager.Instance.OnPlayerWin.AddListener(PlayerWonListener);
	}

	private void GameOverListener()
	{
		gameOverPanel.SetActive(true);
		gameOverText.text = "GAME OVER";
		distanceText.text = "Distance run: " + Mathf.CeilToInt(GameManager.Instance.DistanceWalked) + " meter";
	}

	private void PlayerWonListener()
	{
		gameOverPanel.SetActive(true);
		gameOverText.text = "YOU WIN!";
		distanceText.text = "Distance run: " + Mathf.CeilToInt(GameManager.Instance.DistanceWalked) + " meter";
	}
}
