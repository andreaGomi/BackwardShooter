using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "ScriptableObjects/Settings/Level")]
public class LevelSettingsSO : ScriptableObject
{
	[Range(0.01f, 1f)] public float obstacleHitInfluence;
	public float playerSpeedTraslation;
	public bool endlessRun;
	public float levelLength_mt;
	public float levelCountDown;
}
