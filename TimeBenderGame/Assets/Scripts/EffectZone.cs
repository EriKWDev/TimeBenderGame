using UnityEngine;
using System.Collections;

[AddComponentMenu("ErikWDev/Effect Zone for Third Person Camera")]
[RequireComponent (typeof (BoxCollider))]
public class EffectZone : MonoBehaviour {

	public enum Effects {
		None,
		SlowMotion
	}

	public Effects effect;

	public bool showLines = false;
	public bool fillCube = false;
	public Color color = Color.red; 

	private bool playerIsInside = false;
	private Collider playerCollider;

	[Header("Zone Effect Settings : SlowMotion")]
	[SerializeField]
	private float newTimeScale = 0.5f;
	[SerializeField]
	private float changeSpeed = 4;

	void Start() {
		playerCollider = GameObject.FindGameObjectWithTag ("Player").GetComponent<Collider> ();
	}

	void Update() {
		SetValues (playerCollider, playerIsInside);
	}

	void OnDrawGizmos() {
		if(showLines && !fillCube)
			DrawCube (transform.position, transform.rotation, GetComponent<BoxCollider> ().size);
		if(fillCube)
			DrawCube (transform.position, transform.rotation, GetComponent<BoxCollider> ().size, true);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			playerIsInside = true;
		}
	}

	void OnTriggerStay (Collider other) {
		if (other.gameObject.tag == "Player") {
			playerIsInside = true;
		}
	}
		
	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Player") {
			playerIsInside = false;
		}
	}

	private void SetValues(Collider other) {
		SetValues (other, false);
	}

	private void SetValues(Collider other, bool isInside) {
		switch (effect) {

		case Effects.None:
		default:
			break;

		case Effects.SlowMotion:
			Time.timeScale = (isInside == true ? Mathf.Lerp (Time.timeScale, newTimeScale, changeSpeed * Time.deltaTime) : Mathf.Lerp (Time.timeScale, 1f, changeSpeed * Time.deltaTime));
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
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
