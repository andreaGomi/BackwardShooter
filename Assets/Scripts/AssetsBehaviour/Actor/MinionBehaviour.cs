using System.Collections;
using UnityEngine;

public class MinionBehaviour : Actor, IDamagable
{
	[SerializeField] EnemySettingsSO approachSettings;

	PlayerBehaviour player;
	Vector3 distance;
	bool tryReachPlayer = false;
	Shooter shooter = null;

	private void Start()
	{
		player = FindObjectOfType<PlayerBehaviour>();
		if (player)
		{
			shooter = player.GetComponent<Shooter>();
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
		rigidBody.velocity = Vector3.zero;
		startRunning = false;
		ActorIsDead = true;
		EventManager.TriggerEvent(EventsNameList.AnEnemyIsDead);
	}

	public void Resurrect()
	{
		currentHealth = attributes.health;
		ActorIsDead = false;
		startRunning = true;
	}

	public void TakeDamage(float damage)
	{
		currentHealth -= damage;

		GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.red, Color.green, currentHealth / attributes.health);

		if(currentHealth <= 0)
		{
			ActorDeath();
		}
	}

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
