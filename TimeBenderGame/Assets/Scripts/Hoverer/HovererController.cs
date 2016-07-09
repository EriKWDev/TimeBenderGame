using UnityEngine;
using System.Collections;

public class HovererController : MonoBehaviour {

    public float acceleration;
    public float rotationRate;

    public float turnRotationAngle;
    public float turnRotationSeekSpeed;
    private Camera playerCamera;

    public Material[] playerMaterials;

    private float rotationVelocity;
    private float groundAngleVelocity;
    private float originalAcceleration;

	private Rigidbody thisRigidbody;

    void Awake() {
        thisRigidbody = GetComponent<Rigidbody>();
        originalAcceleration = acceleration;
        playerCamera = GetComponentInChildren<Camera>();
        //transform.position = new Vector3(0f, 14f, 0f);
        GetComponent<Renderer>().material = playerMaterials[GameObject.FindGameObjectsWithTag("Player").Length - 1];
    }

	void FixedUpdate () {
        if (Physics.Raycast(transform.position, transform.up * -1f, 3f)) {
            thisRigidbody.drag = 1f;

            Vector3 forwardForce = transform.forward * acceleration * Input.GetAxis("Vertical");

            forwardForce = forwardForce * Time.deltaTime * thisRigidbody.mass;

			thisRigidbody.AddForce(forwardForce);
        } else {
            thisRigidbody.drag = 0f;
        }

        Vector3 turnTorque = Vector3.up * rotationRate * Input.GetAxis("Horizontal");

        turnTorque = turnTorque * Time.deltaTime * thisRigidbody.mass;
        thisRigidbody.AddTorque(turnTorque);

        Vector3 newRotation = transform.eulerAngles;
        newRotation.z = Mathf.SmoothDampAngle(newRotation.z, Input.GetAxis ("Horizontal") * -turnRotationAngle, ref rotationVelocity, turnRotationSeekSpeed);
	}

    void Update () {
        if (Input.GetKeyDown (KeyCode.R)) {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
