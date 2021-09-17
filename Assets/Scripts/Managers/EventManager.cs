using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
	private Dictionary<string, UnityEvent> dictionary;

	private static EventManager eventManager;
	public static EventManager Instance
	{
		get
		{
			if (!eventManager)
			{
				eventManager = FindObjectOfType<EventManager>() as EventManager;
				if (!eventManager)
				{
					Debug.LogError("No Event Manager found");
				}
				else
				{
					eventManager.Init();
				}
			}
			return eventManager;
		}
	}

	private void Init()
	{
		if(dictionary == null)
			dictionary = new Dictionary<string, UnityEvent>();
	}

	public static void StartListening(string eventName, UnityAction listener)
	{
		UnityEvent thisEvent = null;
		if (Instance.dictionary.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.AddListener(listener);
		}
		else
		{
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			Instance.dictionary.Add(eventName, thisEvent);
		}
	}

	public static void StopListening(string eventName, UnityAction listener)
	{
		if (eventManager == null) return;
		UnityEvent thisEvent = null;
		if (Instance.dictionary.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.RemoveListener(listener);
		}
	}

	public static void TriggerEvent(string eventName)
	{
		UnityEvent thisEvent = null;
		if (Instance.dictionary.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.Invoke();
		}
	}
}
