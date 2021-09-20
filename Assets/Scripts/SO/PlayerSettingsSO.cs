using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/Settings/PlayerSettings")]
public class PlayerSettingsSO : ScriptableObject
{
	public float winPlatformPlayerTrlsSpeed = 5f;
	public float mobPlatformPlayerTrlsSpeed = 50f;
}
