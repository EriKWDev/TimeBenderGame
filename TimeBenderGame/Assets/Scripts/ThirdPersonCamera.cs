using UnityEngine;
using System.Collections;

[AddComponentMenu("ErikWDev/Third Person Camera Follow Script")]
[DisallowMultipleComponent]
public class ThirdPersonCamera : MonoBehaviour {

	[Header("Debugging")]
	public bool debug = false;

	[Header("Public Values")]
	public Transform followTransform;

	public enum CameraStates {
		Behind,
		BehindWithMouse,
		FirstPerson,
		Target,
		Small,
		SmallWithMouse,
		StickToObject,
		Free,
		Orbit,
		StickToObjectAndChangeTarget,
		None
	}

	public CameraStates defaultCameraState = CameraStates.Behind;
	public CameraStates cameraState = CameraStates.Behind;

	[Header("Private Values")]
	[SerializeField]
	private float distanceAway;
	[SerializeField]
	private float distanceUp;
	[SerializeField]
	private float offsetSides;

	[Header("Camera State Settings : Mouse Settings")]
	[SerializeField]
	private float xRotateSpeed;
	[SerializeField]
	private float yRotateSpeed;

	[Header("Camera State Settings : Target")]
	public bool target = false;
	public KeyCode targetKey = KeyCode.Tab;
	[SerializeField]
	private float targetFOV;

	[Header("Camera State Settings : Small")]
	public bool small = false;
	[SerializeField]
	private float smallFOV;
	[SerializeField]
	private float heightDecrease;

	[Header("Camera State Settings : StickToObject")]
	public GameObject objectToStickTo;

	[Header("Camera State Settings : StickToObjectAndChangeTarget")]
	public GameObject newTarget;

	[Header("Camera State Settings : Orbit")]
	public float orbitSpeed;
	public bool orbit = false;

	[Header("Smoothing and Damping")]
	[SerializeField]
	private float camSmoothDampTime = 0.1f;
	[SerializeField]
	private Vector3 velocityCamSmooth = Vector3.one;
	[SerializeField]
	private float lerpValue;
	[SerializeField]
	private float FOVLerpValue;

	private Vector3 lookDir;
	private Vector3 targetPosition;
	private float defaultFOV;

	private Vector3 characterOffset;

	void Start() {
		lookDir = followTransform.forward;

		defaultFOV = GetComponent<Camera> ().fieldOfView;

		transform.LookAt (targetPosition);
	}

	void LateUpdate() {

		{
			cameraState = defaultCameraState;

			if (Input.GetKey (targetKey) || target == true) {
				cameraState = CameraStates.Target;
			}
			if (orbit == true) {
				cameraState = CameraStates.Orbit;
			}
			if (small == true) {
				cameraState = CameraStates.Small;
			}
			if (objectToStickTo != null && newTarget == null) {
				cameraState = CameraStates.StickToObject;
			}
			if (objectToStickTo != null && newTarget != null && Input.GetKey (KeyCode.C)) {
				cameraState = CameraStates.StickToObjectAndChangeTarget;
			}
		}


		if (debug) {
			Debug.DrawRay (followTransform.position + (-lookDir * distanceAway), Vector3.up * distanceUp, Color.red);
			Debug.DrawRay (followTransform.position, -1f * lookDir * distanceAway, Color.blue);
			Debug.DrawLine (followTransform.position, targetPosition, Color.magenta);

			Debug.DrawRay (transform.position, lookDir, Color.green);
		}

		switch (cameraState) {
		case CameraStates.Behind:
		default:
			characterOffset = Vector3.Lerp (characterOffset, followTransform.position + new Vector3 (0f, distanceUp, 0f), lerpValue * Time.deltaTime);
			GetComponent<Camera> ().fieldOfView = Mathf.SmoothStep (GetComponent<Camera> ().fieldOfView, defaultFOV, FOVLerpValue * Time.deltaTime);

			lookDir = characterOffset - transform.position;
			lookDir.y = 0f;
			lookDir.Normalize ();

			targetPosition = characterOffset + followTransform.up * distanceUp - lookDir * distanceAway;

			CompensateForWalls (characterOffset, ref targetPosition);
			SmoothPosition (transform.position, targetPosition);
			transform.LookAt (characterOffset);
			break;
		
		case CameraStates.BehindWithMouse:
			characterOffset = Vector3.Lerp (characterOffset, followTransform.position + new Vector3 (0f, distanceUp, 0f), lerpValue * Time.deltaTime);
			GetComponent<Camera> ().fieldOfView = Mathf.SmoothStep (GetComponent<Camera> ().fieldOfView, defaultFOV, FOVLerpValue * Time.deltaTime);

			transform.RotateAround (characterOffset, Vector3.up, xRotateSpeed * Input.GetAxis ("Mouse X"));
			transform.RotateAround (characterOffset, -Vector3.right, yRotateSpeed * Input.GetAxis ("Mouse Y"));

			targetPosition = (transform.position - characterOffset).normalized * distanceAway + characterOffset;

			CompensateForWalls (characterOffset, ref targetPosition);
			SmoothPosition (transform.position, targetPosition);
			transform.LookAt (characterOffset);
			break;

		case CameraStates.Target:
			characterOffset = Vector3.Lerp (characterOffset, followTransform.position + new Vector3 (0f, distanceUp, 0f) + (transform.right * offsetSides), lerpValue * Time.deltaTime);
			GetComponent<Camera> ().fieldOfView = Mathf.SmoothStep (GetComponent<Camera> ().fieldOfView, targetFOV, FOVLerpValue * Time.deltaTime);
			targetPosition = characterOffset + followTransform.up * distanceUp - followTransform.forward * distanceAway;

			CompensateForWalls (characterOffset, ref targetPosition);
			SmoothPosition (transform.position, targetPosition);

			transform.LookAt (characterOffset);
			break;

		case CameraStates.Small:
			characterOffset = Vector3.Lerp (characterOffset, followTransform.position + new Vector3 (0f, distanceUp, 0f), lerpValue * Time.deltaTime);
			GetComponent<Camera> ().fieldOfView = Mathf.SmoothStep (GetComponent<Camera> ().fieldOfView, smallFOV, FOVLerpValue * Time.deltaTime);
			targetPosition = characterOffset + followTransform.up * (distanceUp - heightDecrease) - followTransform.forward * distanceAway;

			CompensateForWalls (characterOffset, ref targetPosition);
			SmoothPosition (transform.position, targetPosition);
			transform.LookAt (characterOffset);
			break;

		case CameraStates.StickToObject:
			characterOffset = Vector3.Lerp (characterOffset, followTransform.position + new Vector3 (0f, distanceUp, 0f), lerpValue * Time.deltaTime);
			GetComponent<Camera> ().fieldOfView = Mathf.SmoothStep (GetComponent<Camera> ().fieldOfView, defaultFOV, FOVLerpValue * Time.deltaTime);
			targetPosition = objectToStickTo.transform.position;

			SmoothPosition (transform.position, targetPosition);
			transform.LookAt (characterOffset);
			break;

		case CameraStates.StickToObjectAndChangeTarget:
			characterOffset = Vector3.Lerp (characterOffset, followTransform.position + new Vector3 (0f, distanceUp, 0f), lerpValue * Time.deltaTime);
			GetComponent<Camera> ().fieldOfView = Mathf.SmoothStep (GetComponent<Camera> ().fieldOfView, defaultFOV, FOVLerpValue * Time.deltaTime);
			targetPosition = objectToStickTo.transform.position;

			SmoothPosition (transform.position, targetPosition);
			transform.LookAt (newTarget.transform.position);
			break;

		case CameraStates.Orbit:
			characterOffset = Vector3.Lerp (characterOffset, followTransform.position + new Vector3 (0f, distanceUp, 0f), lerpValue * Time.deltaTime);
			GetComponent<Camera> ().fieldOfView = Mathf.SmoothStep (GetComponent<Camera> ().fieldOfView, defaultFOV, FOVLerpValue * Time.deltaTime);

			transform.RotateAround (followTransform.position, Vector3.up, orbitSpeed * Time.deltaTime);
			targetPosition = (transform.position - characterOffset).normalized * distanceAway + characterOffset + followTransform.up * distanceUp ;
			targetPosition.y = characterOffset.y;

			SmoothPosition (transform.position, targetPosition);

			transform.LookAt (characterOffset);
			break;
		
		}
	}

	private void SmoothPosition(Vector3 fromPos, Vector3 toPos) {
		transform.position = Vector3.SmoothDamp (fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
		//transform.position = Vector3.Lerp (fromPos, toPos, Time.deltaTime * lerpValue);
	}

	private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget) {
		if (debug) {
			Debug.DrawLine (fromObject, toTarget, Color.cyan);
		}

		RaycastHit wallHit = new RaycastHit ();
		if (Physics.Linecast (fromObject, toTarget, out wallHit)) {
			if (debug) {
				Debug.DrawRay (wallHit.point, Vector3.left, Color.red);
			}	

			if (wallHit.collider.gameObject.tag != "EffectZone") {
				toTarget = new Vector3 (wallHit.point.x, wallHit.point.y, wallHit.point.z);
			}
		}
	}
}
