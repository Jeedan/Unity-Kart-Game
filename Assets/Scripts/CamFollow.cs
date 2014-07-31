using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {

	public Transform target;
	public Vector3 offset;
	public float camDistance = 1.0f;
	public float camHeight = 1.0f;
	public float damping = 1.0f;

	// Use this for initialization
	void Start () {
		offset.z = camDistance;
		offset.y = camHeight;
		offset.x = 0;
		offset = target.transform.position - transform.position;
	}

	void LateUpdate () {
		offset.z = camDistance;
		offset.y = camHeight;
		offset.x = 0;
		float currentAngle = transform.eulerAngles.y;
		float desiredAngle = target.transform.eulerAngles.y;
		float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);

		Quaternion rotation = Quaternion.Euler (0, angle, 0);
		transform.position = target.transform.position - (rotation * offset);
		transform.LookAt (target.transform);
	}
}
