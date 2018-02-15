using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
	public bool follow = true;

	public Transform target;
	public float height = 20.0f;
	public float smoothDampTime = 0.5f;

	[SerializeField] private Transform allMapViewPosition = null;

	private Vector3 smoothDampVel;

	void LateUpdate()
	{
		if (!target || !follow)
			return;
		SmoothDampToTarget();
	}

	void SmoothDampToTarget()
	{
		var targetPosition = target.position + Vector3.up * height;
		transform.position = Vector3.SmoothDamp(
			transform.position,
			targetPosition,
			ref smoothDampVel,
			smoothDampTime
		);
	}

	public void GotoAllMapView()
	{
		if (allMapViewPosition == null)
			return;

		transform.position = allMapViewPosition.position;
	}
}
