using UnityEngine;
using UnityEngine.Events;

public class SetIKtarget : MonoBehaviour
{
	//Listener to permit animation override or not
	UnityAction StartLevelListener;
	UnityAction GameOverListener;

	Animator animator;
	bool aimToTarget = false;
	Shooter shooter;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		shooter = GetComponentInParent<Shooter>();
		StartLevelListener += EnableAiming;
		GameOverListener += DisableAiming;
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

	private void OnAnimatorIK(int layerIndex)
	{
		//If is not permitted to override the animation
		if (!aimToTarget)
		{
			animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
			animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
			animator.SetLookAtWeight(0f);
			return;
		}

		Transform enemyPos = shooter.GetNearestEnemytransform();
		if (enemyPos == null)
			return;

		// Find target direction, normalize it and set its altitude nearly at player shoulders
		Vector3 aimDirection = (enemyPos.position - transform.position).normalized * .45f;
		aimDirection += transform.position;
		aimDirection.y += 1.3f;

		animator.SetIKPosition(AvatarIKGoal.LeftHand, aimDirection);
		animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
		animator.SetIKPosition(AvatarIKGoal.RightHand, aimDirection);
		animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
		animator.SetLookAtPosition(aimDirection);
		animator.SetLookAtWeight(1f);
	}

	private void EnableAiming()
	{
		aimToTarget = true;
	}

	private void DisableAiming()
	{
		aimToTarget = false;
	}
}
