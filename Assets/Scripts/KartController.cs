using UnityEngine;
using System.Collections;

public class KartController : MonoBehaviour
{

    //public float turnSpeed = 20.0f;
    // public float maxSqrVelocity = 11.0f;
    //public float maxYrot = 90;
    //public float minYrot = -90;

    private float forwardAccel = 0.0f;
    private Rigidbody _rigidbody;
    private Vector3 moveDirection;
    private float maxSqrVel = 88.0f;
    [SerializeField] Transform FrontWheels;
    [SerializeField] Transform RearWheels;

    [SerializeField] bool isGrounded = false;
    [SerializeField] float rayLength = 5.0f;
    [SerializeField] float gravity = 9.81f;
    // Use this for initialization
    void Start()
    {
        _rigidbody = rigidbody;
        _rigidbody.freezeRotation = true;
        //_rigidbody.centerOfMass = new Vector3 (0.0f, -2.5f, 0.0f);
    }

    //void FixedUpdate()
    //{
    //    //Debug.Log(_rigidbody.velocity.sqrMagnitude);
    //}


    void CheckIfGrounded(float input)
    {
        Ray rayFront = new Ray(FrontWheels.transform.position, -transform.up);
        RaycastHit rayHit;
        if (Physics.Raycast(rayFront, out rayHit, rayLength, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
            float slopeAngle = Vector3.Dot(Vector3.up, rayHit.normal);
            Debug.Log(slopeAngle);
            if (slopeAngle > 0.7f && slopeAngle < 1.0f)
            {
                // if we are moving up a slope
                // we store our angle and the grounds angle
                // then we lerp our angle to the grounds angle 
                // so that our car looks like it is moving up a ramp, which it is
                Vector3 a = transform.eulerAngles;
                Vector3 b = rayHit.transform.eulerAngles;
                transform.eulerAngles = new Vector3(Mathf.LerpAngle(a.x, -b.x, 5f * Time.deltaTime), transform.eulerAngles.y, transform.eulerAngles.z);

                // we seem to loose velocity when moving up a ramp, so we add a bit of speed
                // this is not realistic but should be fine for an arcade racer
                if (input > 0.01)
                    _rigidbody.velocity += transform.forward * 1.3f;
            }
            else
            {
                // reset our X-axis rotation to 0
                Vector3 a = transform.eulerAngles;
                transform.eulerAngles = new Vector3(Mathf.LerpAngle(a.x, 0, Time.deltaTime), transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }
        else
        {
            isGrounded = false;
            Vector3 a = transform.eulerAngles;
            transform.eulerAngles = new Vector3(Mathf.LerpAngle(a.x, 0, Time.deltaTime), transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }

    public void Accelerate(float input, float speed, float maxSpeed)
    {
        float maxReverseSpeed = -maxSpeed * 0.5f;
        CheckIfGrounded(input);
        if (isGrounded)
        {
            if (input > 0.0f)
                if (forwardAccel < maxSpeed)
                    forwardAccel += speed;
            if (input < 0.0f)
                if (forwardAccel > maxReverseSpeed)
                    forwardAccel -= speed;
            if (input == 0.0f)
            {
                forwardAccel *= 0.8f;
            }

            moveDirection = new Vector3(0.0f, 0.0f, forwardAccel);

            _rigidbody.AddRelativeForce(moveDirection, ForceMode.Acceleration);
        }
        else
        {
            if (input == 0.0f)
            {
                forwardAccel *= 0.8f;
            }

            moveDirection = new Vector3(0.0f, 0.0f, forwardAccel);
            _rigidbody.AddRelativeForce(moveDirection + (transform.up * -gravity), ForceMode.Acceleration);
        }

        // limit the velocity so we don't have errors
        if (_rigidbody.velocity.sqrMagnitude > maxSqrVel)
            _rigidbody.velocity *= 0.99f;
    }

    public void Steering(float input, float steerFactor)
    {
        Vector3 kartAngles = transform.eulerAngles;
        float rotation = 0.0f;
        if (Mathf.Abs(input) > 0.01f)
        {
            rotation = kartAngles.y + ((forwardAccel * (steerFactor * Mathf.Sign(input))) * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotation, transform.eulerAngles.z);
        }
    }

    void OnDrawGizmos()
    {
        Ray rayFront = new Ray(FrontWheels.transform.position, -transform.up);

        Gizmos.DrawLine(FrontWheels.transform.position, FrontWheels.transform.position + new Vector3(0, -rayLength, 0));
    }
}