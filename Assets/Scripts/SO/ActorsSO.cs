using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "ScriptableObjects/Actors")]
public class ActorsSO : ScriptableObject
{
	public float maxSpeed;
	[Range(.01f, 1f)] public float acceleration;
	public float health;
	public float toughness;
}
