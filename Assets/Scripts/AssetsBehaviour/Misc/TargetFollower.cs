using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Attach this class to an object in order to makes it follow the specified target
/// </summary>
public class TargetFollower : MonoBehaviour
{
	[Tooltip("Leave empty to make the script seeks the player")]
	[SerializeField] string tagToFollow = "";						//tag to follow. If tag is an empty string, follow the object tagged as player
	[SerializeField] bool startListenToFinishLineEvent = true;		//should this instance of the class listen to "Approach Finish Line" event
	UnityAction FinishLineListener;

	Transform target;		//target to follow
	float zOffset;			//initial offset from the target
	bool follow = true;

	private void Awake()
	{
		FinishLineListener += StopFollowTarget;
	}

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
		catch(UnityException)
		{
			Debug.LogError("No Object found with tag \"" + tagToFollow + "\".");
		}
	}

	private void OnEnable()
	{
		if (startListenToFinishLineEvent)
			EventManager.StartListening(EventsNameList.ApproachingFinishLine, FinishLineListener);
	}

	private void OnDisable()
	{
		if (startListenToFinishLineEvent)
			EventManager.StopListening(EventsNameList.ApproachingFinishLine, FinishLineListener);
	}

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
