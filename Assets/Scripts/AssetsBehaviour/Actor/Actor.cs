using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class represents an Actor, which is an object that is able to move whitin the level, use physics and interact with other objects
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Actor : MonoBehaviour
{
	//Actor stats SO
	[SerializeField] protected ActorsAttributesSO attributes;

	//wheater this actor is dead or not
	public bool ActorIsDead { get; protected set; } = false;

	UnityAction LevelStartListener;
	UnityAction StopActorListener;

	protected bool startRunning;	//Allow updating rigidbody velocity
	protected float currentHealth;
	protected float currentSpeed;	
	protected float targetSpeed;	//Velocity that the rigidbody should reach

	protected Animator animator;
	protected Rigidbody rigidBody;
	public Rigidbody RigidBody { get { return rigidBody; } }

	Coroutine obstacleHittedCoroutine = null;	//Coroutine that manage velocity decreasing after hit an obstacle
	bool mainRunControl;						//if rigidbody velocity should calculate whitin coroutine or ManageRun function
	float obstacleInfluence;					//holder for level settings obstacle's influence
	float maxSpeed;								//holder for actor' settings max speed

	protected virtual void Awake()
	{
		mainRunControl = true;
		startRunning = false;
		currentHealth = attributes.health;
		targetSpeed = attributes.maxSpeed;
		maxSpeed = attributes.maxSpeed;
		currentSpeed = 0f;
		animator = GetComponentInChildren<Animator>();
		rigidBody = GetComponent<Rigidbody>();

		if(LevelManager.Instance)
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

	//Update rigidbody velocity, always going towards object forward. Can be overridden
	protected virtual void ManageRun()
	{
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, attributes.acceleration);
		rigidBody.velocity = transform.forward * currentSpeed;
	}

	void LevelStarted()
	{
		startRunning = true;
	}

	// Function that is called after hitting an obstacle
	public void OnObstacleHitted(float decrement)
	{
		//Find proper decrement for this actor based on its attributes
		float unitDec = Mathf.Clamp(decrement - attributes.toughness, 0f, decrement);

		// is there a valid decrement? If not, return
		if (unitDec == 0)
			return;

		//Don't allow target speed to go below zero
		targetSpeed = Mathf.Clamp(currentSpeed - unitDec, 0f, currentSpeed);

		if (obstacleHittedCoroutine != null)
		{
			//If an instance of the coroutine is currently going on, stop it before calling a new one
			StopCoroutine(obstacleHittedCoroutine);
		}
		obstacleHittedCoroutine = StartCoroutine(DecrementSpeedOverTime());
	}

	/// <summary>
	/// Coroutine that is called each time rigidbody velocity shoud be decremented
	/// </summary>
	IEnumerator DecrementSpeedOverTime()
	{
		mainRunControl = false;
		
		//Until current rigidbody velocity hasn't reach the target velocity
		while (rigidBody.velocity.magnitude > targetSpeed + .1f)
		{
			currentSpeed = Mathf.Lerp(targetSpeed, currentSpeed, obstacleInfluence);
			rigidBody.velocity = transform.forward * currentSpeed;
			yield return new WaitForFixedUpdate();
		}

		mainRunControl = true;

		//Reset target speed to max speed
		targetSpeed = maxSpeed;

		obstacleHittedCoroutine = null;
	}

	/// <summary>
	/// Stop current actor each time level as ended
	/// </summary>
	private void StopRunning()
	{
		startRunning = false;
		rigidBody.velocity = Vector3.zero;
		rigidBody.isKinematic = true;

		if(!ActorIsDead)
			animator.SetTrigger("Win");
	}

	/// <summary>
	/// Set all current actor class' attributes properly when the current actor is dead
	/// </summary>
	protected virtual void ActorDeath()
	{
		//Freeze rigidbody
		rigidBody.velocity = Vector3.zero;
		rigidBody.isKinematic = true;

		//Stop updating rigidbody velocity
		startRunning = false;

		ActorIsDead = true;
		GetComponent<CapsuleCollider>().enabled = false;

		//Trigger death animation
		animator.SetTrigger("Die");
	}
}
