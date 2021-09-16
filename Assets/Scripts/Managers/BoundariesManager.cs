using UnityEngine;

public class BoundariesManager : MonoBehaviour
{
	[SerializeField] Transform leftWall;
	[SerializeField] Transform rightWall;

	private void Awake()
	{
		Vector2 boundaries = Vector2.zero;
		boundaries.y = rightWall.TransformPoint(rightWall.position).x;
		boundaries.x = leftWall.TransformPoint(leftWall.position).x;

		//LevelManager.Instance.StageBoudaries = boundaries;
	}
}
