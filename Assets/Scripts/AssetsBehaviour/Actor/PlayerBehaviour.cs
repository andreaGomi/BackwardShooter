using UnityEngine;

public class PlayerBehaviour : Actor, IDamagable
{
	float traslSpeed;
	public Vector3 Traslation { get; set; } = Vector3.zero;
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
    }

	protected override void ManageRun()
	{
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, attributes.acceleration);
		rigidBody.velocity = (transform.forward + Traslation) * currentSpeed;
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
