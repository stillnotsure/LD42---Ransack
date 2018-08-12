using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

	Camera camera;
	Vector3 origPosition;

	public float shakeDuration;
	public float shakeMagnitude;
	public float dampening;

	public float multiplier;


	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera>();
		origPosition = camera.transform.position;
	}
	
	void Update(){
		if (shakeDuration > 0) {
			camera.transform.localPosition = origPosition + Random.insideUnitSphere * shakeMagnitude;
			shakeDuration -= Time.deltaTime * dampening * multiplier;
		} else {
			shakeMagnitude= 0f;
			shakeDuration= 0f;
			camera.transform.localPosition = origPosition;
		}
	}
	
	public void AddScreenshake(float duration, float magnitude){
		shakeMagnitude += magnitude * multiplier;
		shakeDuration += duration * multiplier;
	}
}

