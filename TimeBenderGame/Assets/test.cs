using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	public Chronos.GlobalClock clock;

	[SerializeField]
	private bool rewind = false;
	[SerializeField]
	private float timeToRewind = 2f;
	[SerializeField]
	private float rewindTimeScale = -2f;
	[SerializeField]
	private float smooth = 10f;
	[SerializeField]
	private float tmp = 0f;

	void Start () {
		rewind = false;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space) && !rewind) {
			rewind = true;
			tmp = timeToRewind;
			GetComponent<AudioSource> ().Play ();
		}

		if (rewind == true) {
			Camera.main.GetComponent<GlitchEffect> ().enabled = true;

			if (tmp <= 0f) {
				rewind = false;
			}

			clock.localTimeScale = Mathf.Lerp (clock.localTimeScale, rewindTimeScale, Time.deltaTime * smooth);

			tmp -= Time.deltaTime;
		} else {
			clock.localTimeScale = 1f;
			Camera.main.GetComponent<GlitchEffect> ().enabled = false;
		}
	}

	public void goBackInTime (float secondsToGoBack) {
		float tmp = secondsToGoBack;
		float oldValue = clock.localTimeScale;

		clock.localTimeScale = -secondsToGoBack;
	}
}
