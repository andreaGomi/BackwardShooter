using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : Obstacle
{
	[SerializeField] ObstaclesSO attributes;
	

	protected override void SlowDownActor(Actor actor)
	{
		//Debug.Log("Slowing Down " + actor.gameObject.tag + " of " + attributes.toughness);
		actor.OnObstacleHitted(attributes.toughness);
	}
}
