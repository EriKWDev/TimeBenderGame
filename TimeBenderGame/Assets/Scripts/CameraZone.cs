using UnityEngine;
using System.Collections;

[AddComponentMenu("ErikWDev/Camera Zone for Third Person Camera")]
[RequireComponent (typeof (BoxCollider))]
public class CameraZone : MonoBehaviour {

	public ThirdPersonCamera.CameraStates cameraZoneEffect;

	public bool showLines = false;
	public bool fillCube = false;
	public Color color = Color.red; 

	[Header("Zone Effect Settings : StickToObject")]
	public GameObject objectToStickTo;

	[Header("Zone Effect Settings : StickToObjectAndChangeTarget")]
	public GameObject newTarget;

	[Header("Zone Effect Settings : Orbit")]
	public float orbitSpeed = 0f;

	void OnDrawGizmos() {
		if(showLines && !fillCube)
			DrawCube (transform.position, transform.rotation, GetComponent<BoxCollider> ().size);
		if(fillCube)
			DrawCube (transform.position, transform.rotation, GetComponent<BoxCollider> ().size, true);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			SetValues (other);
		}
	}

	void OnTriggerStay (Collider other) {
		if (other.gameObject.tag == "Player") {
			SetValues (other);
		}
	}
		
	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Player") {
			SetValues (other, true);
		}
	}

	private void SetValues(Collider other) {
		SetValues (other, false);
	}

	private void SetValues(Collider other, bool exit) {
		switch (cameraZoneEffect) {

		case ThirdPersonCamera.CameraStates.StickToObject:
		default:
			other.GetComponent<ThirdPersonPlayer> ().gamecam.objectToStickTo = (exit == false ? objectToStickTo : null);
			break;

		case ThirdPersonCamera.CameraStates.StickToObjectAndChangeTarget:
			other.GetComponent<ThirdPersonPlayer> ().gamecam.objectToStickTo = (exit == false ? objectToStickTo : null);
			other.GetComponent<ThirdPersonPlayer> ().gamecam.newTarget = (exit == false ? newTarget : null);
			break;

		case ThirdPersonCamera.CameraStates.Target:
			other.GetComponent<ThirdPersonPlayer> ().gamecam.target = (exit == false ? true : false);
			break;

		case ThirdPersonCamera.CameraStates.Small:
			other.GetComponent<ThirdPersonPlayer> ().gamecam.small = (exit == false ? true : false);
			break;

		case ThirdPersonCamera.CameraStates.Orbit:
			other.GetComponent<ThirdPersonPlayer> ().gamecam.orbit = (exit == false ? true : false);
			other.GetComponent<ThirdPersonPlayer> ().gamecam.orbitSpeed = (exit == false ? orbitSpeed : 0f);
			break;
		}
	}
		

	public void DrawCube (Vector3 position, Quaternion rotation, Vector3 scale) {
		DrawCube (position, rotation, scale, color, false);
	}

	public void DrawCube (Vector3 position, Quaternion rotation, Vector3 scale, bool fillCube) {
		DrawCube (position, rotation, scale, color, fillCube);
	}

	public void DrawCube (Vector3 position, Quaternion rotation, Vector3 scale, Color cubeColor, bool fillCube) {
		Matrix4x4 cubeTransform = Matrix4x4.TRS (position, rotation, scale);
		Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

		Gizmos.matrix = cubeTransform;
		Gizmos.color = cubeColor;

		if(!fillCube)
			Gizmos.DrawWireCube (Vector3.zero, Vector3.one);
		if(fillCube)
			Gizmos.DrawCube (Vector3.zero, Vector3.one);
		
		Gizmos.matrix = oldGizmosMatrix;
	}
}
