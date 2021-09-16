using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{
	[Tooltip("Leave empty to make the script seeks the player")]
	[SerializeField] string tagToFollow = "";
	[SerializeField] bool startListenToFinishLineEvent = true;

	Transform target;
	float zOffset;
	bool follow = true;

	private void Awake()
	{
		if(startListenToFinishLineEvent)
			GameManager.Instance.OnReachingFinishLine.AddListener(StopFollowTarget);
	}

	// Start is called before the first frame update
	void Start()
    {
		try
		{
			GameObject _Object = (tagToFollow == "") ? GameObject.FindGameObjectWithTag("Player") : GameObject.FindGameObjectWithTag(tagToFollow);
			if (_Object)
			{
				target = _Object.transform;
				zOffset = transform.position.z - target.position.z;
			}
		}
		catch(UnityException e)
		{
			Debug.LogError("No Object found with tag \"" + tagToFollow + "\".");
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (target && follow)
		{
			Vector3 newPos = new Vector3(transform.position.x, transform.position.y, target.position.z + zOffset);
			transform.position = newPos;
		}
    }

	private void StopFollowTarget()
	{
		follow = false;
	}
}
