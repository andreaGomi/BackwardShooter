/// <summary>
/// Specialization for Obsatcle class
/// </summary>
public class WallBehaviour : Obstacle
{
	/// <summary>
	/// Slow down the specified actor 
	/// </summary>
	/// <param name="actor">The actor to slow down</param>
	protected override void SlowDownActor(Actor actor)
	{
		actor.OnObstacleHitted(Attributes.toughness);
	}
}
