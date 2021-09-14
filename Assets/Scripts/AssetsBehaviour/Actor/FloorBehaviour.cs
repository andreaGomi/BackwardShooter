using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloorBehaviour : MonoBehaviour
{
	Rigidbody playerRB = null;
	Rigidbody thisRB = null;

	private void Awake()
	{
		playerRB = FindObjectOfType<PlayerBehaviour>().rb;

		thisRB = GetComponent<Rigidbody>();
		thisRB.useGravity = false;
		thisRB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		if(playerRB)
			thisRB.velocity = playerRB.velocity;
    }
}
