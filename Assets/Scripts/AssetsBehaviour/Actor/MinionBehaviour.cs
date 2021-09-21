using System.Collections;
using UnityEngine;

/// <summary>
/// Specialization of class Actor, representing an enemy
/// </summary>
public class MinionBehaviour : Actor, IDamagable
{
	//approaching settings SO
	[SerializeField] EnemySettingsSO approachSettings;

	PlayerBehaviour player;
	Vector3 distance;	//distance to the player
	bool tryReachPlayer = false;	//whether this actor shoud try to approach player

	private void Start()
	{

		player = FindObjectOfType<PlayerBehaviour>();
		if (player)
		{
			distance = player.transform.position - transform.position;
			StartCoroutine(CheckPlayerDistance());
		}
		else
		{
			Debug.LogError("No player found!");
			distance = Vector3.zero;
		}
	}

	private void OnCollisionEnter(Collision coll)
	{
		if(coll.gameObject.TryGetComponent(out PlayerBehaviour player))
		{
			if(player.TryGetComponent(out IDamagable dam))
			{
				//if this actor collide with the player, damage it
				player.TakeDamage(1f);
				startRunning = false;
			}
		}
	}

	protected override void ManageRun()
	{
		if (tryReachPlayer)
		{
			currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, attributes.acceleration);
			rigidBody.velocity = distance.normalized * currentSpeed;
		}
		else
		{
			currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, attributes.acceleration);
			rigidBody.velocity = transform.forward * currentSpeed;
		}
	}

	protected override void ActorDeath()
	{
		base.ActorDeath();
		EventManager.TriggerEvent(EventsNameList.AnEnemyIsDead);
	}

	//Method callde when Endless run mode is enabled. Resurrect the actor
	public void Resurrect()
	{
		animator.SetTrigger("Respawn");
		currentHealth = attributes.health;
		ActorIsDead = false;
		startRunning = true;
		rigidBody.isKinematic = false;
		GetComponent<CapsuleCollider>().enabled = true;

		transform.GetChild(0).localRotation = Quaternion.identity;
	}

	public void TakeDamage(float damage)
	{
		currentHealth -= damage;

		if(currentHealth <= 0)
		{
			ActorDeath();
		}
	}

	/// <summary>
	/// Coroutine that check if this actor is near enough to approach the player, based on level settings SO
	/// </summary>
	IEnumerator CheckPlayerDistance()
	{
		while (true)
		{
			distance = player.transform.position - transform.position;

			if (distance.magnitude <= approachSettings.approachingDistance)
				tryReachPlayer = true;
			else
				tryReachPlayer = false;

			yield return new WaitForSeconds(approachSettings.distanceCheckInterval);
		}
	}
}
