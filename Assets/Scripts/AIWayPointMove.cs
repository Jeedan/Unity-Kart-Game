using UnityEngine;
using System.Collections;

public class AIWayPointMove : MonoBehaviour
{
    public Transform[] waypoint;
    public int currentWaypoint = 0;
    private KartController kartController;

    public float speed = 5.0f;
    public float maxSpeed = 150.0f;
    public float steerFactor = 0.25f;

    public float distanceToNextWaypoint = 1.0f;
    private Transform _transform;
    private Rigidbody _rigidbody;
    Vector3 direction;
    // Use this for initialization
    void Start()
    {
        _transform = transform;
        _rigidbody = rigidbody;
        kartController = GetComponent<KartController>();
    }

    void Update()
    {
        float dist = Vector3.Distance(waypoint[currentWaypoint].transform.position, transform.position);
        direction = new Vector3(0.0f, 0.0f, (waypoint[currentWaypoint].transform.position - _transform.position).z);

        if (dist < distanceToNextWaypoint)
            currentWaypoint++;

        if (currentWaypoint == 14)
            currentWaypoint = 14;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 a = _transform.eulerAngles;
        Vector3 b = waypoint[currentWaypoint].transform.eulerAngles;
        _transform.eulerAngles = new Vector3(_transform.eulerAngles.x, Mathf.LerpAngle(a.y, b.y, 2f* Time.deltaTime), _transform.eulerAngles.z);

        //_rigidbody.AddForce(direction * speed * Time.deltaTime, ForceMode.VelocityChange);
        if (currentWaypoint != 15)
            kartController.Accelerate(Mathf.Abs((transform.position.z - waypoint[currentWaypoint].transform.position.z)), speed, maxSpeed);


        //if (_rigidbody.velocity.sqrMagnitude > 80)
        //  _rigidbody.velocity *= 0.99f;

        Debug.Log(_rigidbody.velocity.sqrMagnitude);
    }
}
