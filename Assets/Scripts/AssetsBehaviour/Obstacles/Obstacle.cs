using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
	[SerializeField] ObstaclesSO attributes;
	public ObstaclesSO Attributes { get { return attributes; } }

	[SerializeField] float deltaDistance = 1f;
	public float DeltaDistance { get { return deltaDistance; } }
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent(out Actor actor))
		{
			SlowDownActor(actor);
			gameObject.SetActive(false);
		}
	}

	protected abstract void SlowDownActor(Actor actor);
}
