using UnityEngine;

/// <summary>
/// It detects if gameobject tag as "Player" triggers collider
/// </summary>
public class FinishLine : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			EventManager.TriggerEvent(EventsNameList.LevelComplete);
		}
	}
}
