using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "ScriptableObjects/Settings/Level")]
public class LevelSettingsSO : ScriptableObject
{
	[Range(0, 1)] public float obstacleDeceleration;
	public float playerSpeedTraslation;
}
