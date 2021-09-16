using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[Header("Level Settings")]
	[SerializeField] LevelSettingsSO levelSettings;
	[SerializeField] PlayerSettingsSO playerSettings;

	public LevelSettingsSO LevelSettings { get { return levelSettings; } }
	public PlayerSettingsSO PlayerSettings { get { return playerSettings; } }

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


}
