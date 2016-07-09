using UnityEngine;
using System.Collections;

public class Thruster : MonoBehaviour {

    public Renderer playerRenderer;

	void Update () {
        GetComponent<TrailRenderer>().material = playerRenderer.material;
        GetComponent<Renderer>().material = playerRenderer.material;
	}
}
