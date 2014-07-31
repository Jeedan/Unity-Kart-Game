using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	//public float turnSpeed = 20.0f;
	//public float maxSqrVelocity = 5.0f;
	//public float maxYrot = 90;
	//public float minYrot = -90;
	//float turnRot;
	//float rotationZ = 0.0f;
	//public bool throttling = false;

	public string throttleKey = "Vertical";
	public string steerKey = "Horizontal";
	public float speed = 5.0f;
	public float steerFactor = 0.25f; // is a percentage so btw 0-100% = 0.0-1.0f
	public float maxForwardSpeed = 150f;
	float throttleInput;
	float steeringInput;
	KartController kartController;

	// Use this for initialization
	void Start () {
		kartController = gameObject.GetComponent<KartController>();
	}

	void Update(){
		throttleInput = Input.GetAxis(throttleKey);
		steeringInput = Input.GetAxis (steerKey);
	}
	// Update is called once per frame
	void FixedUpdate() {
		kartController.Accelerate (throttleInput, speed, maxForwardSpeed);
		kartController.Steering(steeringInput,steerFactor);
	}
}
