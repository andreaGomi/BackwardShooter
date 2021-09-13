using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
	protected bool startRunning;
	protected float currentHealth;
	protected float currentSpeed;

	protected Animator animator;

	protected void Awake()
	{
		GameManager.Inistance.OnLevelStart.AddListener(LevelStarted);
	}

	protected virtual void Start()
	{
		startRunning = false;
		currentSpeed = 0f;
		animator = GetComponent<Animator>();
	}

	void LevelStarted()
	{
		startRunning = true;
		Debug.Log("Actor started");
	}

	protected abstract void ActorDied();
	
}
