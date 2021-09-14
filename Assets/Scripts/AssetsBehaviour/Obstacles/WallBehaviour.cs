using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : Obstacle
{
	[SerializeField] ObstaclesSO attributes;
	

	protected override void SlowDownActor(Actor actor)
	{
		actor.OnObstacleHitted(attributes.toughness);
	}
}
