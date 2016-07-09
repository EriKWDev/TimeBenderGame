using UnityEngine;
using System.Collections;

[AddComponentMenu("ErikWDev/Third Person Character Logic Script")]
[DisallowMultipleComponent]
public class ThirdPersonPlayer : MonoBehaviour {

	[Header("Debug")]
	public bool debug = false;

	[Header("Private Values")]
	[SerializeField]
	public ThirdPersonCamera gamecam;
	[SerializeField]
	private float directionSpeed = 3f;

	private float speed = 0f;
	private float direction = 0f;
	private float h = 0f;
	private float v = 0f;

	void Update () {
		h = Input.GetAxis ("Horizontal");
		v = Input.GetAxis ("Vertical");

		StickToWorldSpace (this.transform, gamecam.transform, ref direction, ref speed);
	}

	public void StickToWorldSpace(Transform root, Transform camera, ref float directionOut, ref float speedOut) {
		Vector3 rootDirection = root.forward;

		Vector3 stickDirection = new Vector3 (h, 0f, v);

		speedOut = stickDirection.sqrMagnitude;

		Vector3 cameraDirection = camera.forward;
		cameraDirection.y = 0f;
		Quaternion referentialShift = Quaternion.FromToRotation (Vector3.forward, rootDirection);

		Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross (moveDirection, rootDirection);

		if (debug) {
			Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
			Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), axisSign, Color.red);
			Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
			Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);
		}

		float angleRootToMove = Vector3.Angle (rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);
		angleRootToMove /= 180f;

		directionOut = angleRootToMove + directionSpeed;
	}
}
