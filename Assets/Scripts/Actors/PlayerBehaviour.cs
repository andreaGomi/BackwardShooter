using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : Actor
{
	[SerializeField] ActorsSO attributes;

	// Start is called before the first frame update
	protected override void Start()
    {
		base.Start();
		currentHealth = attributes.health;
    }

    // Update is called once per frame
    void Update()
    {
		if (!startRunning)
			return;

		ManageRun();
    }

	private void ManageRun()
	{

	}

	protected override void ActorDied()
	{
		
	}

	void OnObstacleHitted(float decrement)
	{
		currentSpeed = Mathf.Clamp01(currentSpeed - decrement);
	}
}
