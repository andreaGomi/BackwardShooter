using UnityEngine;

[CreateAssetMenu(fileName = "EnemySettings", menuName = "ScriptableObjects/Settings/EnemySettings")]
public class EnemySettingsSO : ScriptableObject
{
	public float approachingDistance = 2f;
	public float distanceCheckInterval = .5f;
}
