using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBehaviour : Actor
{
	private void OnCollisionEnter(Collision coll)
	{
		if(coll.gameObject.TryGetComponent(out PlayerBehaviour player))
		{
			player.ActorDied();
		}
	}
	
	public override void ActorDied()
	{
		Debug.Log("Minion died");
	}
}
