using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class drawMeInGizmos : MonoBehaviour {

	public Color color = Color.red; 
	public Vector3 size = new Vector3 (0.2f, 0.2f, 0.2f);
	public bool debug = false;

	void Update() {
		if (debug) {
			if (transform.parent.GetComponent<CameraZone> () != null && transform.parent.GetComponent<BoxCollider> () != null) {
				Debug.DrawLine (transform.position, new Vector3 (transform.parent.position.x + (transform.parent.GetComponent<BoxCollider> ().size.x / 2f), transform.parent.position.y + (transform.parent.GetComponent<BoxCollider> ().size.y / 2f), transform.parent.position.z + (transform.parent.GetComponent<BoxCollider> ().size.z / 2f)), color);
				Debug.DrawLine (transform.position, new Vector3 (transform.parent.position.x + (transform.parent.GetComponent<BoxCollider> ().size.x / 2f), transform.parent.position.y + (transform.parent.GetComponent<BoxCollider> ().size.y / 2f), transform.parent.position.z - (transform.parent.GetComponent<BoxCollider> ().size.z / 2f)), color);
				Debug.DrawLine (transform.position, new Vector3 (transform.parent.position.x - (transform.parent.GetComponent<BoxCollider> ().size.x / 2f), transform.parent.position.y + (transform.parent.GetComponent<BoxCollider> ().size.y / 2f), transform.parent.position.z - (transform.parent.GetComponent<BoxCollider> ().size.z / 2f)), color);
				Debug.DrawLine (transform.position, new Vector3 (transform.parent.position.x - (transform.parent.GetComponent<BoxCollider> ().size.x / 2f), transform.parent.position.y + (transform.parent.GetComponent<BoxCollider> ().size.y / 2f), transform.parent.position.z + (transform.parent.GetComponent<BoxCollider> ().size.z / 2f)), color);
			}
		}
	}

	void OnDrawGizmos() {
		DrawCube (transform.position, transform.rotation, transform.lossyScale);
	}

	public void DrawCube (Vector3 position, Quaternion rotation, Vector3 scale) {
		DrawCube (position, rotation, scale, color);
	}

	public void DrawCube (Vector3 position, Quaternion rotation, Vector3 scale, Color cubeColor) {
		Matrix4x4 cubeTransform = Matrix4x4.TRS (position, rotation, scale);
		Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

		Gizmos.matrix = cubeTransform;
		Gizmos.color = cubeColor;

		Gizmos.DrawCube (Vector3.zero, Vector3.one);

		Gizmos.matrix = oldGizmosMatrix;
	}
}
