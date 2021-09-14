using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : Actor
{
	[SerializeField] ActorsSO attributes;

	float traslSpeed;
	float maxSpeed;
	Coroutine obstacleHittedCoroutine = null;
	bool mainRunControl = true;

	float obstacleDec;

	// Start is called before the first frame update
	protected override void Start()
    {
		base.Start();
		traslSpeed = LevelManager.Inistance.levelSettings.playerSpeedTraslation;
		obstacleDec = LevelManager.Inistance.levelSettings.obstacleDeceleration;
		currentHealth = attributes.health;
		targetSpeed = attributes.maxSpeed;
		maxSpeed = attributes.maxSpeed;
	}
	

    void Update()
    {
		if (!startRunning)
			return;

		ManagePlayerInput();
    }

	private void FixedUpdate()
	{
		if (!startRunning)
			return;

		if (mainRunControl)
			ManageRun();
	}

	private void ManagePlayerInput()
	{
		float h = Input.GetAxis("Horizontal") * Time.deltaTime * traslSpeed;

		Vector3 newPos = new Vector3(h, 0f, 0f);

		rigidBody.MovePosition(rigidBody.position + newPos);
	}

	private void ManageRun()
	{
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, attributes.acceleration);
		rigidBody.velocity = transform.forward * currentSpeed;
		Debug.Log("Main - magn: " + rigidBody.velocity.magnitude);
		Debug.Log("Main - Target: " + targetSpeed);
	}

	public override void OnObstacleHitted(float decrement)
	{
		targetSpeed = Mathf.Clamp01(currentSpeed - decrement);

		if (obstacleHittedCoroutine != null)
		{
			Debug.Log("STOPPING CORO");
			StopCoroutine(obstacleHittedCoroutine);
		}
		obstacleHittedCoroutine = StartCoroutine(DecrementSpeedOverTime());
	}

	IEnumerator DecrementSpeedOverTime()
	{
		Debug.Log("Decrementing..");
		mainRunControl = false;

		while(rigidBody.velocity.magnitude > targetSpeed)
		{
			Debug.Log("Coro - magn: " + rigidBody.velocity.magnitude);
			Debug.Log("Coro - Target: " + targetSpeed);
			currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, obstacleDec);
			rigidBody.velocity = transform.forward * currentSpeed;
			yield return new WaitForFixedUpdate();
		}

		mainRunControl = true;
		targetSpeed = maxSpeed;

		Debug.Log("Stop Decrementing");
	}

	protected override void ActorDied()
	{
		Debug.Log("Player died");
	}
}
