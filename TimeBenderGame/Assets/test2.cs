using UnityEngine;
using System.Collections;

public class test2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.E)) {
			Collider[] colliders = Physics.OverlapSphere (transform.position, 5f);
			foreach (Collider c in colliders) {
				if (c.GetComponent<Rigidbody> () != null) {
					c.GetComponent<Rigidbody> ().AddExplosionForce (1000f, transform.position, 5f, 3f);
				}
			}
		}
	}
}
