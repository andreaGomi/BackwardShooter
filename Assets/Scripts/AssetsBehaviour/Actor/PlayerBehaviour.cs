using UnityEngine;

public class PlayerBehaviour : Actor, IDamagable
{
	public Vector3 Traslation { get; set; } = Vector3.zero;
	Shooter shooter;

	Vector3 lastVelocity;
	bool aiming;

	protected override void Awake()
	{
		base.Awake();
		shooter = GetComponent<Shooter>();
	}

	private void Start()
    {
		lastVelocity = RigidBody.velocity;
		aiming = true;
	}

	private void OnAnimatorIK(int layerIndex)
	{
		if (!aiming)
			return;
		Debug.Log("HEY");
		animator.SetIKPosition(AvatarIKGoal.LeftHand, shooter.GetNearestEnemytransform().position);
		animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
		animator.SetIKPosition(AvatarIKGoal.RightHand, shooter.GetNearestEnemytransform().position);
		animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
	}

	void Update()
    {
		if (!startRunning)
			return;
    }

	protected override void ManageRun()
	{
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, attributes.acceleration);
		Vector3 newVel = (transform.forward + Traslation) * currentSpeed;
		rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, newVel, .1f);
		lastVelocity = RigidBody.velocity;
	}

	protected override void ActorDeath()
	{
		rigidBody.velocity = Vector3.zero;
		rigidBody.isKinematic = true;
		startRunning = false;
		ActorIsDead = true;
		aiming = false;
		EventManager.TriggerEvent(EventsNameList.PlayerDeath);
		GetComponent<CapsuleCollider>().enabled = false;
		animator.SetTrigger("Die");
	}

	public void TakeDamage(float dam)
	{
		currentHealth -= dam;
		if (currentHealth <= 0)
			ActorDeath();
	}
	
}
