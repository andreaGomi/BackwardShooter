using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[Header("Level Settings")]
	[SerializeField] LevelSettingsSO settings;
	public LevelSettingsSO levelSettings { get { return settings; } }

	private static LevelManager levelManager = null;
	public static LevelManager Inistance
	{
		get
		{
			if (!levelManager)
			{
				levelManager = FindObjectOfType<LevelManager>();

				if (!levelManager)
				{
					Debug.Log("No Level Manager in current scene");
				}
			}
			return levelManager;
		}
	}


}
