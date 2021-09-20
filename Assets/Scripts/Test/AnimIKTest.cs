using UnityEngine;

public class AnimIKTest : MonoBehaviour
{
	public Transform target;
	Animator animator;

    void Start()
    {
		animator = GetComponentInChildren<Animator>();
    }

	private void OnAnimatorIK(int layerIndex)
	{
		Debug.Log("HEY");
		animator.SetIKPosition(AvatarIKGoal.LeftHand, target.position);
		animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
		animator.SetIKPosition(AvatarIKGoal.RightHand, target.position);
		animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
	}
	
}
