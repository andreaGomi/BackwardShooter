using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class retrive user touch/mouse input for player traslation and set it in player behaviour class  
/// </summary>
public class UserInputHandler : MonoBehaviour
{
	UnityAction StartLevelListener;
	UnityAction GameOverListener;

	[SerializeField] Joystick joystick;
	
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

	void Update()
    {
		if (joystick)
			UpdateUserInput();
	}

	private void UpdateUserInput()
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
