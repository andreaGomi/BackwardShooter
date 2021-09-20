using System;
using UnityEngine;
using UnityEngine.Events;

public class UserInputHandler : MonoBehaviour
{
	UnityAction StartLevelListener;
	UnityAction GameOverListener;

	[SerializeField] Joystick joystick;
	
	float trslSpeed;
	PlayerBehaviour playerScript;

	private void Awake()
	{
		StartLevelListener += EnableJoystick;
		GameOverListener += DisableJoystick;
	}

	void Start()
    {
		playerScript = GetComponent<PlayerBehaviour>();
    }

	private void OnEnable()
	{
		EventManager.StartListening(EventsNameList.LevelStarted, StartLevelListener);
		EventManager.StartListening(EventsNameList.PlayerDeath, GameOverListener);
		EventManager.StartListening(EventsNameList.AllEnemiesDeath, GameOverListener);
		EventManager.StartListening(EventsNameList.LevelComplete, GameOverListener);
	}

	private void OnDisable()
	{
		EventManager.StopListening(EventsNameList.LevelStarted, StartLevelListener);
		EventManager.StopListening(EventsNameList.PlayerDeath, GameOverListener);
		EventManager.StopListening(EventsNameList.AllEnemiesDeath, GameOverListener);
		EventManager.StopListening(EventsNameList.LevelComplete, GameOverListener);
	}

	// Update is called once per frame
	void Update()
    {
		if (joystick)
			ManageMobileInput();
	}

	private void ManageMobileInput()
	{
		playerScript.Traslation = new Vector3(joystick.Horizontal, 0f, 0f);
	}

	private void EnableJoystick()
	{
		joystick.gameObject.SetActive(true);
	}

	private void DisableJoystick()
	{
		joystick.gameObject.SetActive(false);
	}
}
