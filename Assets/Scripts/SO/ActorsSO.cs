using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "ScriptableObjects/Actors")]
public class ActorsSO : ScriptableObject
{
	public float maxSpeed;
	[Range(.1f, 1f)] public float acceleration;
	public float health;
}
