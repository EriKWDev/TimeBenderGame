using UnityEngine;
using System.Collections;

public class ThrusterScript : MonoBehaviour {
    
    public float thrusterStrength;
    public float thrusterDistance;
    public Transform[] thrusters;

	private Rigidbody thisRigidbody;

    void Awake() {
        thisRigidbody = GetComponent<Rigidbody>();
    }


    void FixedUpdate() {
        RaycastHit hit;
        foreach (Transform thruster in thrusters) {
            Vector3 downwardForce;
            float distancePercentage;

            if (Physics.Raycast(thruster.position, thruster.up * -1f, out hit, thrusterDistance)) {
                distancePercentage = 1f - (hit.distance / thrusterDistance);

                downwardForce = transform.up * thrusterStrength * distancePercentage;
                downwardForce = downwardForce * Time.deltaTime * thisRigidbody.mass;

                thisRigidbody.AddForceAtPosition(downwardForce, thruster.position);
            }
        }
    }
}
