using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Controller))]
public class Player : MonoBehaviour
{
    [SerializeField]
    float jumpHight = 4;
    [SerializeField]
    float timeTojumpApex = 0.4f;

    [SerializeField]
    [Range(0.1f,1f)]
    float accelrationTimeAirbone;
    [SerializeField]
    [Range(0.1f, 1f)]
    float accelrationTimeGround;
    
    float jumpVelocity;
    float gravity;

    [SerializeField]
    float moveSpeed=6;
    Vector3 velocity;

    Controller controller;

    float velocityXSmoothing;
    void Start()
    {
        controller = GetComponent<Controller>();
        gravity =-(2 * jumpHight) / Mathf.Pow(timeTojumpApex,2f);
        jumpVelocity = Mathf.Abs(gravity) * timeTojumpApex;
    }

     void Update()
    {
        if (controller.collisonInfo.above || controller.collisonInfo.below)
        {
            velocity.y = 0;
        }
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Input.GetKeyDown(KeyCode.Space) && controller.collisonInfo.below)
        {
            velocity.y = jumpVelocity;
        }
        float timeForVelocity = controller.collisonInfo.below ? accelrationTimeAirbone : accelrationTimeGround;
        float targetVelocity = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocityXSmoothing, timeForVelocity);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity*Time.deltaTime);
    }


}
