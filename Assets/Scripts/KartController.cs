using UnityEngine;
using System.Collections;

public class KartController : MonoBehaviour
{

    //public float turnSpeed = 20.0f;
    // public float maxSqrVelocity = 11.0f;
    //public float maxYrot = 90;
    //public float minYrot = -90;

    private float forwardAccel = 0.0f;
    private bool isGrounded = false;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private Vector3 moveDirection;
    [SerializeField] float maxSqrVel = 88.0f;
    [SerializeField] Transform RaycastTransform;
    [SerializeField] Transform[] FrontWheels;

    [SerializeField] float rayLength = 5.0f;
    [SerializeField] float gravity = 9.81f;
    [SerializeField] float friction = 0.8f;

    // Use this for initialization
    void Start()
    {
        _transform = transform;
        _rigidbody = rigidbody;
        _rigidbody.freezeRotation = true;
        //_rigidbody.centerOfMass = new Vector3 (0.0f, -2.5f, 0.0f);
    }

    void CheckIfGrounded(float input)
    {
        Ray rayFront = new Ray(RaycastTransform.transform.position, -transform.up);
        RaycastHit rayHit;
        if (Physics.Raycast(rayFront, out rayHit, rayLength, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
            float slopeAngle = Vector3.Dot(Vector3.up, rayHit.normal);
            
            if (slopeAngle > 0.7f && slopeAngle < 1.0f)
            {
                // if we are moving up a slope
                // we store our angle and the grounds angle
                // then we lerp our angle to the grounds angle 
                // so that our car looks like it is moving up a ramp, which it is
                if (input > 0.01)
                    _rigidbody.velocity += _transform.forward * 2.0f;
                
                Vector3 a = _transform.eulerAngles;
                Vector3 b = rayHit.transform.eulerAngles;
                _transform.eulerAngles = new Vector3(Mathf.LerpAngle(a.x, b.x, 5f * Time.deltaTime), _transform.eulerAngles.y, _transform.eulerAngles.z);

                // we seem to loose velocity when moving up a ramp, so we add a bit of speed
                // this is not realistic but should be fine for an arcade racer
             
            }
            else
            {
                // reset our X-axis rotation to 0
                Vector3 a = _transform.eulerAngles;
                _transform.eulerAngles = new Vector3(Mathf.LerpAngle(a.x, 0, Time.deltaTime), _transform.eulerAngles.y, _transform.eulerAngles.z);

                // here we reset our karts position to be above the ground, because when we collide we sometimes get pushed down due 
                // to rigidbody physics which results in getting stuck under the ground.
                Vector3 offset = rayHit.transform.position - _transform.position;
                if (offset.y < rayLength)
                {

                    _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0.0f, _rigidbody.velocity.z);
                    _transform.position = new Vector3(_transform.position.x, rayHit.transform.position.y + rayLength + 0.001f, _transform.position.z);
                }
            }
        }
        else
        {
            isGrounded = false;
            // align with the ground
            Vector3 a = _transform.eulerAngles;
            _transform.eulerAngles = new Vector3(Mathf.LerpAngle(a.x, 0, Time.deltaTime), _transform.eulerAngles.y, _transform.eulerAngles.z);
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

            moveDirection = new Vector3(0.0f, 0.0f, forwardAccel);
            _rigidbody.AddRelativeForce(moveDirection, ForceMode.Acceleration);
        }
        else
        {
            moveDirection = new Vector3(0.0f, 0.0f, forwardAccel);
            _rigidbody.AddRelativeForce(moveDirection + (_transform.up * -gravity), ForceMode.Acceleration);
        }

        // friction
        if (input == 0.0f)
        {
            forwardAccel *= friction;
        }

        Debug.Log(_rigidbody.velocity.sqrMagnitude);
        // limit the velocity so we don't have errors
        if (_rigidbody.velocity.sqrMagnitude > maxSqrVel)
            _rigidbody.velocity *= 0.99f;
    }

    public void Steering(float input, float steerFactor)
    {
        Vector3 kartAngles = _transform.eulerAngles;
        
        float rotation = 0.0f;
        float rot2 = 0.0f;
        float rot3 = 0.0f;
        if (Mathf.Abs(input) > 0.01f)
        {
            rotation = kartAngles.y + ((forwardAccel * (steerFactor * Mathf.Sign(input))) * Time.deltaTime);
            _transform.eulerAngles = new Vector3(_transform.eulerAngles.x, rotation, _transform.eulerAngles.z);
           for (int i = 0; i < FrontWheels.Length-1; i++)
			{
                Vector3 wheelAngle1 = FrontWheels[i].localEulerAngles;
                rot2 = wheelAngle1.y + ((forwardAccel * (steerFactor * Mathf.Sign(input))) * Time.deltaTime);
                FrontWheels[0].localEulerAngles = new Vector3(0.0f, rot2, 0.0f);
                FrontWheels[1].localEulerAngles = new Vector3(0.0f, rot2, 0.0f);

                float clampY = Mathf.Clamp(FrontWheels[0].localEulerAngles.y, 160, 200);
                float clampY2 = Mathf.Clamp(FrontWheels[1].localEulerAngles.y, 160, 200);
                FrontWheels[0].localEulerAngles = new Vector3(0.0f, clampY, 0.0f); 
                FrontWheels[1].localEulerAngles = new Vector3(0.0f, clampY2, 0.0f); 
			}
        }
    }

    void OnDrawGizmos()
    {
        Ray rayFront = new Ray(RaycastTransform.transform.position, -transform.up);

        Gizmos.DrawLine(RaycastTransform.transform.position, RaycastTransform.transform.position + new Vector3(0, -rayLength, 0));
    }
}