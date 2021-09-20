using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	[Header("Level Settings")]
	[SerializeField] LevelSettingsSO levelSettings;

	public LevelSettingsSO LevelSettings { get { return levelSettings; } }

	private static LevelManager levelManager = null;
	public static LevelManager Instance
	{
		get
		{
			if (!levelManager)
			{
				levelManager = FindObjectOfType(typeof(LevelManager)) as LevelManager;

				if (!levelManager)
				{
					Debug.Log("No Level Manager in current scene");
				}
			}
			return levelManager;
		}
	}

	public void ReloadLevel()
	{
		SceneManager.LoadScene(0);
	}

	public void QuitApplication()
	{
		Application.Quit();
	}
}
