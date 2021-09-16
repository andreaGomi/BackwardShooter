using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehaviour : Actor, IDamagable
{
	[HideInInspector] public UnityEvent OnPlayerDied;
	[HideInInspector] public UnityEvent OnRunOver;

	float traslSpeed;
	Vector3 trsl = Vector3.zero;
	Shooter shooter;

	protected override void Awake()
	{
		base.Awake();
		shooter = GetComponent<Shooter>();
		OnPlayerDied.AddListener(ActorDeath);
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

	public override void ActorDeath()
	{
		Debug.Log("Player died");
		rigidBody.velocity = Vector3.zero;
		startRunning = false;
		ActorIsDead = true;
		shooter.SetShooting(false);
	}

	public void TakeDamage(float dam)
	{
		currentHealth -= dam;
		if (currentHealth <= 0)
			OnPlayerDied.Invoke();
	}
}
