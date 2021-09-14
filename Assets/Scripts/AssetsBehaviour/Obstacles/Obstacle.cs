using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out Actor actor))
		{
			SlowDownActor(actor);
			gameObject.SetActive(false);
		}
	}

	protected abstract void SlowDownActor(Actor actor);
}
