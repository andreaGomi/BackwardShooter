using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
	[SerializeField] ObstaclesSO attributes;
	public ObstaclesSO Attributes { get { return attributes; } }

	[SerializeField] float deltaDistance = 1f;
	/// <summary>
	/// How far another obstacle can be spawned after this one
	/// </summary>
	public float DeltaDistance { get { return deltaDistance; } }
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent(out Actor actor))
		{
			//If an actor triggers the collider, slow down it and disable current gameobject
			SlowDownActor(actor);
			gameObject.SetActive(false);
		}
	}

	protected abstract void SlowDownActor(Actor actor);
}
