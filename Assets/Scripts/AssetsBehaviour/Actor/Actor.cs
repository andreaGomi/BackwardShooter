using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
	[SerializeField] protected ActorsSO attributes;

	protected bool startRunning;
	protected float currentHealth;
	protected float currentSpeed;
	protected float targetSpeed;

	protected Animator animator;
	protected Rigidbody rigidBody;
	public Rigidbody rb { get { return rigidBody; } }

	Coroutine obstacleHittedCoroutine = null;
	bool mainRunControl;
	float obstacleInfluence;
	float maxSpeed;

	protected void Awake()
	{
		mainRunControl = true;
		startRunning = false;
		currentHealth = attributes.health;
		targetSpeed = attributes.maxSpeed;
		maxSpeed = attributes.maxSpeed;
		currentSpeed = 0f;
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
		obstacleInfluence = LevelManager.Instance.levelSettings.obstacleHitInfluence;
		GameManager.Instance.OnLevelStart.AddListener(LevelStarted);
	}

	protected virtual void Start()
	{
		
	}

	private void FixedUpdate()
	{
		if (!startRunning)
			return;

		if (mainRunControl)
			ManageRun();

		//Debug.Log(gameObject.tag + " velocity: " + rigidBody.velocity.magnitude);
	}

	private void ManageRun()
	{
		//Debug.Log(gameObject.tag + " Main control");
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, attributes.acceleration);
		rigidBody.velocity = transform.forward * currentSpeed;
		//Debug.Log("Main - magn: " + rigidBody.velocity.magnitude);
		//Debug.Log("Main - Target: " + targetSpeed);
	}

	void LevelStarted()
	{
		startRunning = true;
	}

	public void OnObstacleHitted(float decrement)
	{
		//Debug.Log("DEC_: " + decrement);
		//Debug.Log("OLD targetSpeed: " + targetSpeed);
		//Debug.Log("Deceleration: " + decrement);
		float unitDec = Mathf.Clamp(decrement - attributes.toughness, 0f, decrement);
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
			currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, obstacleInfluence);
			rigidBody.velocity = transform.forward * currentSpeed;
			yield return new WaitForFixedUpdate();
		}

		mainRunControl = true;
		targetSpeed = maxSpeed;

		//Debug.Log("Stop Decrementing");
	}

	public abstract void ActorDied();
}
