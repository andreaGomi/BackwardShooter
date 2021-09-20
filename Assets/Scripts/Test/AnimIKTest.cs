using UnityEngine;

public class AnimIKTest : MonoBehaviour
{
	public Transform target;
	Animator animator;

    void Start()
    {
		if(!TryGetComponent(out animator))
		{
			if(transform.GetChild(0).TryGetComponent(out animator))
			{
				Debug.Log("Figlio");
			}
		}
		else
		{
			Debug.Log("Padre");
		}
    }

	private void OnAnimatorIK(int layerIndex)
	{
		animator.SetIKPosition(AvatarIKGoal.LeftHand, target.position);
		animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
		animator.SetIKPosition(AvatarIKGoal.RightHand, target.position);
		animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
	}
	
}
