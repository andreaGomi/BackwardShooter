using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehaviour : Actor
{
	[HideInInspector] public UnityEvent OnPlayerDied;
	[HideInInspector] public UnityEvent OnRunOver;

	float traslSpeed;

	// Start is called before the first frame update
	protected override void Start()
    {
		base.Start();
		traslSpeed = LevelManager.Instance.levelSettings.playerSpeedTraslation;
	}
	
    void Update()
    {
		if (!startRunning)
			return;

		ManagePlayerInput();
    }

	private void ManagePlayerInput()
	{
		float h = Input.GetAxis("Horizontal") * Time.deltaTime * traslSpeed;

		Vector3 newPos = new Vector3(h, 0f, 0f);

		rigidBody.MovePosition(rigidBody.position + newPos);
	}
	
	public override void ActorDied()
	{
		Debug.Log("Player died");
		rigidBody.velocity = Vector3.zero;
		startRunning = false;
	}
}
