using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBehaviour : Actor
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
		traslSpeed = LevelManager.Instance.levelSettings.playerSpeedTraslation;
		obstacleDec = LevelManager.Instance.levelSettings.obstacleHitInfluence;
		currentHealth = attributes.health;
		targetSpeed = attributes.maxSpeed;
		maxSpeed = attributes.maxSpeed;
	}

	private void OnCollisionEnter(Collision coll)
	{
		if(coll.gameObject.TryGetComponent(out PlayerBehaviour player))
		{
			player.ActorDied();
		}
	}

	void Update()
	{
		if (!startRunning)
			return;
	}

	private void FixedUpdate()
	{
		if (!startRunning)
			return;

		if (mainRunControl)
			ManageRun();
	}

	private void ManageRun()
	{
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, attributes.acceleration);
		rigidBody.velocity = transform.forward * currentSpeed;
		//Debug.Log("Main - magn: " + rigidBody.velocity.magnitude);
		//Debug.Log("Main - Target: " + targetSpeed);
	}

	public override void OnObstacleHitted(float decrement)
	{
		//Debug.Log("DEC_: " + decrement);
		//Debug.Log("OLD targetSpeed: " + targetSpeed);
		//Debug.Log("Deceleration: " + decrement);
		float unitDec = Mathf.Clamp(decrement - attributes.thoughness, 0f, decrement);
		//Debug.Log("DEC: " + unitDec);
		if (unitDec == 0)
			return;

		targetSpeed = Mathf.Clamp01(currentSpeed - unitDec);
		//Debug.Log("NEW targetSpeed: " + targetSpeed);

		if (obstacleHittedCoroutine != null)
		{
			//Debug.Log("STOPPING CORO");
			StopCoroutine(obstacleHittedCoroutine);
		}
		obstacleHittedCoroutine = StartCoroutine(DecrementSpeedOverTime());
	}

	IEnumerator DecrementSpeedOverTime()
	{
		//Debug.Log("Decrementing..");
		mainRunControl = false;

		yield return new WaitForFixedUpdate();

		while (rigidBody.velocity.magnitude > targetSpeed + .1f)
		{
			//Debug.Log("Coro - magn: " + rigidBody.velocity.magnitude);
			//Debug.Log("Coro - Target: " + targetSpeed);
			currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, obstacleDec);
			rigidBody.velocity = transform.forward * currentSpeed;
			yield return new WaitForFixedUpdate();
		}

		mainRunControl = true;
		targetSpeed = maxSpeed;

		//Debug.Log("Stop Decrementing");
	}

	public override void ActorDied()
	{
		Debug.Log("Minion died");
	}
}
