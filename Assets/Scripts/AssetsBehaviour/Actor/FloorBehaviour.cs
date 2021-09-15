using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBehaviour : MonoBehaviour
{
	//DEBUG
	bool follow = true;

	Transform playerTrans = null;
	float zOffset;

	private void Start()
	{
		playerTrans = FindObjectOfType<PlayerBehaviour>().transform;
		zOffset = playerTrans.position.z - transform.position.z;
	}

	private void Update()
	{
		if (playerTrans && follow)
		{
			zOffset = playerTrans.position.z - transform.position.z;
			transform.position = transform.position + new Vector3(0f, 0f, zOffset);
		}
	}

}
