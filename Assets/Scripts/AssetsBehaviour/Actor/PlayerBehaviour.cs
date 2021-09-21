using UnityEngine;

public class PlayerBehaviour : Actor, IDamagable
{
	// Traslation input amunt
	public Vector3 Traslation { get; set; } = Vector3.zero;

	void Update()
    {
		if (!startRunning)
			return;
    }

	// Main Run Control Function. Updates rigidbody velocity
	protected override void ManageRun()
	{
		currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, attributes.acceleration);
		Vector3 newVel = (transform.forward + Traslation) * currentSpeed;
		rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, newVel, .1f);
	}

	protected override void ActorDeath()
	{
		base.ActorDeath();
		EventManager.TriggerEvent(EventsNameList.PlayerDeath);
	}

	public void TakeDamage(float dam)
	{
		currentHealth -= dam;
		if (currentHealth <= 0)
			ActorDeath();
	}
	
}
