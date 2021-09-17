using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehaviour : Actor, IDamagable
{
	float traslSpeed;
	Vector3 trsl = Vector3.zero;
	Shooter shooter;

	protected override void Awake()
	{
		base.Awake();
		shooter = GetComponent<Shooter>();
	}

	private void Start()
    {
		traslSpeed = LevelManager.Instance.PlayerSettings.playerSpeedTraslation;
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
		trsl = new Vector3(h, 0f, 0f);
	}

	protected override void ManageRun()
	{
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, attributes.acceleration);
		rigidBody.velocity = (transform.forward + trsl) * currentSpeed;
	}

	protected override void ActorDeath()
	{
		rigidBody.velocity = Vector3.zero;
		startRunning = false;
		ActorIsDead = true;
		EventManager.TriggerEvent(EventsNameList.PlayerDeath);
	}

	public void TakeDamage(float dam)
	{
		currentHealth -= dam;
		if (currentHealth <= 0)
			ActorDeath();
	}
}
