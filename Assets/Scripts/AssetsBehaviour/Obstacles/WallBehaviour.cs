using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : Obstacle
{
	protected override void SlowDownActor(Actor actor)
	{
		actor.OnObstacleHitted(Attributes.toughness);
	}
}
