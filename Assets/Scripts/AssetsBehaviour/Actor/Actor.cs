using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Actor : MonoBehaviour
{
	[SerializeField] protected ActorsAttributesSO attributes;

	public bool ActorIsDead { get; protected set; } = false;

	UnityAction LevelStartListener;
	UnityAction StopActorListener;

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

	protected virtual void Awake()
	{
		mainRunControl = true;
		startRunning = false;
		currentHealth = attributes.health;
		targetSpeed = attributes.maxSpeed;
		maxSpeed = attributes.maxSpeed;
		currentSpeed = 0f;
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
		obstacleInfluence = LevelManager.Instance.LevelSettings.obstacleHitInfluence;

		LevelStartListener += LevelStarted;
		StopActorListener += StopRunning;
	}

	private void OnEnable()
	{
		EventManager.StartListening(EventsNameList.LevelStarted, LevelStartListener);
		EventManager.StartListening(EventsNameList.PlayerDeath, StopActorListener);
		EventManager.StartListening(EventsNameList.AllEnemiesDeath, StopActorListener);
		EventManager.StartListening(EventsNameList.LevelComplete, StopActorListener);
	}

	private void OnDisable()
	{
		EventManager.StopListening(EventsNameList.LevelStarted, LevelStartListener);
		EventManager.StopListening(EventsNameList.PlayerDeath, StopActorListener);
		EventManager.StopListening(EventsNameList.AllEnemiesDeath, StopActorListener);
		EventManager.StopListening(EventsNameList.LevelComplete, StopActorListener);
	}

	protected virtual void FixedUpdate()
	{
		if (!startRunning)
			return;

		if (mainRunControl)
			ManageRun();
		
	}

	protected virtual void ManageRun()
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

		targetSpeed = Mathf.Clamp(currentSpeed - unitDec, 0f, currentSpeed);
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
		Debug.Log("Decrementing..");
		mainRunControl = false;

		//yield return new WaitForFixedUpdate();
		//Debug.Log("Current Vel: " + rigidBody.velocity.magnitude + " -- Target: " + targetSpeed);
		while (rigidBody.velocity.magnitude > targetSpeed + .1f)
		{
			//Debug.Log("Coro - magn: " + rigidBody.velocity.magnitude);
			//Debug.Log("Coro - Target: " + targetSpeed);
			currentSpeed = Mathf.Lerp(targetSpeed, currentSpeed, obstacleInfluence);
			rigidBody.velocity = transform.forward * currentSpeed;
			yield return new WaitForFixedUpdate();
		}

		mainRunControl = true;
		targetSpeed = maxSpeed;

		Debug.Log("Stop Decrementing");
	}

	protected abstract void ActorDeath();

	public void StopRunning()
	{
		//Debug.Log("Chiamarta a stop");
		startRunning = false;
		rb.velocity = Vector3.zero;
	}
}
