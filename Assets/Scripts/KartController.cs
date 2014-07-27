using UnityEngine;
using System.Collections;

public class KartController : MonoBehaviour {

	//public float turnSpeed = 20.0f;
	// public float maxSqrVelocity = 11.0f;
	//public float maxYrot = 90;
	//public float minYrot = -90;
	
	Rigidbody _rigidbody;
	float forwardAccel = 0.0f;

	public Transform FrontWheels;
	public Transform RearWheels;

	// Use this for initialization
	void Start () {
		_rigidbody = rigidbody;
		//_rigidbody.centerOfMass = new Vector3 (0.0f, -2.5f, 0.0f);
	}

	public void Accelerate(float input, float speed, float maxSpeed){
		float maxReverseSpeed = maxSpeed * 0.5f;
		if (input > 0.0f)
			if (forwardAccel < maxSpeed)
				forwardAccel += speed;
		if (input < 0.0f)
			if (forwardAccel > maxReverseSpeed)
				forwardAccel -= speed;
		if (input == 0.0f) {
			if (forwardAccel > 0)
				forwardAccel -= speed;
			else if (forwardAccel < 0)
				forwardAccel += speed;
		}
		
		Vector3 moveDirection = new Vector3 (0.0f, 0.0f, forwardAccel);
		_rigidbody.AddRelativeForce(moveDirection, ForceMode.Acceleration);
	}

	public void HorizontalTurnRotation(float input, float steerFactor){
		Vector3 kartAngles = transform.eulerAngles;
		float rotation = 0.0f;
		if (Mathf.Abs(input) > 0.01f) {
			rotation = kartAngles.y + ((forwardAccel * (steerFactor * Mathf.Sign (input))) * Time.deltaTime);
			transform.eulerAngles = new Vector3 (0.0f, rotation, 0.0f);
		}
	}
}