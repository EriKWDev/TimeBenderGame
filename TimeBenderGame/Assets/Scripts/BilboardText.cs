using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class BilboardText : MonoBehaviour {

	private GameObject cam;

	void Start() {
		cam = Camera.main.gameObject;

	}

	void Update () {
		transform.rotation = cam.transform.rotation;
	}
}
