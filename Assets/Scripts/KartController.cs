using UnityEngine;
using System.Collections;

public class KartController : MonoBehaviour {
	
	public float speed = 5.0f;
	public float turnSpeed = 20.0f;
	public float maxSqrVelocity = 5.0f;
	public float maxYrot = 90;
	public float minYrot = -90;

	public Transform FrontWheels;
	public Transform RearWheels;

	Rigidbody _rigidbody;
	float direction;
	float turnRot;
	float rotationZ = 0.0f;
	public bool accelerating = false;
	// Use this for initialization
	void Start () {
		_rigidbody = rigidbody;
	}

	void Update()
	{		
		direction = Input.GetAxis("Vertical");
		Debug.Log (direction);
		if (direction > 0.5f || direction < -0.5f)
			accelerating = true;
		else 
			accelerating = false;

		turnRot = Input.GetAxis ("Horizontal") * turnSpeed * Time.deltaTime;
		turnRot = Mathf.Clamp (turnRot, minYrot, maxYrot);

		//if(Input.GetAxis ("Horizontal") > 0.1f || Input.GetAxis ("Horizontal") < -0.1f)
		//	rotationZ += Input.GetAxis ("Horizontal") * turnSpeed * Time.deltaTime;
		
		//rotationZ = Mathf.Clamp (rotationZ, -90, 90);
		//FrontWheels.transform.localEulerAngles = new Vector3(FrontWheels.transform.localEulerAngles.x,rotationZ,FrontWheels.transform.localEulerAngles.z);

		//theWheel.rotation = Quaternion.RotateTowards(theWheel.rotation, Quaternion.LookRotation(theWheel.right, theWheel.up),turnRot);

		//transform.Rotate(Vector3.up * turnRot);
	}

	void FixedUpdate () {

		if (accelerating) {
			_rigidbody.AddForce (transform.forward * (speed * direction), ForceMode.VelocityChange);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (transform.right, transform.up), turnRot); 
		}
	}
}
