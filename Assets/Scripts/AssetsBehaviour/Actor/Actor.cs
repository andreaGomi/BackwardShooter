using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
	protected bool startRunning;
	protected float currentHealth;
	protected float currentSpeed;
	protected float targetSpeed;

	protected Animator animator;
	protected Rigidbody rigidBody;
	public Rigidbody rb { get { return rigidBody; } }

	protected void Awake()
	{
		GameManager.Instance.OnLevelStart.AddListener(LevelStarted);
	}

	protected virtual void Start()
	{
		startRunning = false;
		currentSpeed = 0f;
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
	}

	void LevelStarted()
	{
		startRunning = true;
	}

	public abstract void OnObstacleHitted(float decrement);

	public abstract void ActorDied();
	
}
