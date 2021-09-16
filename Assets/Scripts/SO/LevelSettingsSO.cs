using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "ScriptableObjects/Settings/LevelSettings")]
public class LevelSettingsSO : ScriptableObject
{
	[Tooltip("How much strong the obstacle's interpolation influence should be. The higher, the faster an actor will be slowed")]
	[Range(0.01f, 1f)] public float obstacleHitInfluence;
	
	[Tooltip("Should obstacles be spawned in incremental waves or one after another")]
	public bool waveMode;
	
	[Tooltip("Should the level has an end")]
	public bool endlessRun;

	[Tooltip("Number of enemies in this level that will be spawned")]
	public int numberOfEnemies;

	[Tooltip("If not endless run, how much the run will last")]
	public float levelLength_mt;

	[Tooltip("Choose a distance to the finish line in meter in which a follower should stop follow the player")]
	public float stopFollowPlayerAt_mt;

	[Tooltip("Count down to level start")]
	public int levelCountDown;
}
