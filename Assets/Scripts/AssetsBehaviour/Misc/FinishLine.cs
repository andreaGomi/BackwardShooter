using UnityEngine;
using UnityEngine.Events;

public class FinishLine : MonoBehaviour
{
	[HideInInspector] public UnityEvent OnLevelEnd; 

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			EventManager.TriggerEvent(EventsNameList.LevelComplete);
		}
	}
}
