using System;
using UnityEngine;

public class UserInputHandler : MonoBehaviour
{

#if UNITY_ANDROID || UNITY_IOS
	Touch touch;
#endif
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
	Vector3 firstTouch = Vector3.zero;
#endif

	float inputMovement;
	float trslSpeed;
	PlayerBehaviour playerScript;
	float screenWidth;

    // Start is called before the first frame update
    void Start()
    {
		inputMovement = 0f;
		trslSpeed = LevelManager.Instance.PlayerSettings.playerSpeedTraslation;
		screenWidth = Screen.width/10;
		playerScript = GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
		ManageMobileInput();
#endif
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
		ManageDesktopInput();
#endif
	}

#if UNITY_ANDROID || UNITY_IOS
	private void ManageMobileInput()
	{
		if(Input.touchCount > 0)
		{
			touch = Input.GetTouch(0);
		}
	}
#endif
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
	private void ManageDesktopInput()
	{
		if (Input.GetMouseButton(0))
		{
			if(firstTouch == Vector3.zero)
			{
				firstTouch = Input.mousePosition;
			}
			else
			{
				Vector3 deltaPos = Input.mousePosition - firstTouch;
				deltaPos /= screenWidth * trslSpeed * Time.deltaTime;
				deltaPos.x = Mathf.Clamp(deltaPos.x, -1f, 1f);
				deltaPos.y = 0f;
				deltaPos.z = 0f;
				playerScript.Traslation = deltaPos;
			}
		}
		else
		{
			firstTouch = Vector3.zero;
		}
	}
#endif
}
