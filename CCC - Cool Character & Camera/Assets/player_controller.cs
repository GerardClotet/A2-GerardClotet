using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boxer : MonoBehaviour
{
    // Start is called before the first frame update

    enum PLAYERSTATE
    {
        IDLE,
        FWD,
        LEFT,
        RIGHT,
        BACK,
        JUMP
    }
    [SerializeField]
    GameObject targetLook;

    [SerializeField]
    [Range(1f,3f)]
    float moveSpeed = 1.5f;

    [SerializeField]
    float jump_force = 5;

    float jump = 0;
    Vector2 movement;

    Animator animator;

    bool punch;

    float velocityZ = 0f;
    float velocityX = 0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f;
    public float maximumWalkVelocity = 1f;
    public float maximumRunVelocity = 2f;
    
    [Range(0.1f,0.7f)]
    public float speed = 0.5f;

    // camera shake
    CinemachineImpulseSource cineImpulse;

    PLAYERSTATE states = PLAYERSTATE.IDLE;
    void Start()
    {
        movement = new Vector2();
        movement = Vector2.zero;
        animator = GetComponent<Animator>();
        cineImpulse = GetComponent<CinemachineImpulseSource>();



    }
    //handles acceleration & deceleration of velocity
    void ChangeVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool backPressed, bool runPressed, float currentMaxVelocity)
    {

        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
       //     transform.position += Vector3.forward  * velocityZ*speed;
        }
        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
           // transform.position += Vector3.left   * velocityX * speed;
        }
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
         //   transform.position += Vector3.right  * velocityX * speed;

        }
        if (backPressed && velocityZ > -currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * acceleration;
           // transform.position += Vector3.back  * velocityZ * speed;

        }

        //deceleration
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
          //  transform.position += Vector3.forward  * velocityZ * speed;

        }


        if (!backPressed && velocityZ < 0.0f)
        {
            velocityZ += Time.deltaTime * deceleration;
         //   transform.position += Vector3.back  * velocityZ * speed;

        }
        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
          //  transform.position += Vector3.left * velocityZ * speed;

        }


        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
            //transform.position += Vector3.right * velocityZ * speed;

        }
    }
    //handles reset and lockiing of velocity
    void  lockOrResetVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool backPressed, bool runPressed, float currentMaxVelocity)
    {
        if (!forwardPressed && !backPressed && velocityZ != 0.0f && (velocityZ > -0.05f && velocityZ < 0.05f))
        {
            velocityZ = 0.0f;
        }


        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }
        //locking for run
        if (forwardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
            if (velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05f))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }

        //locking left
        if (leftPressed && runPressed & velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - 0.05f))
            {
                velocityX = -currentMaxVelocity;
            }
        }
        else if (leftPressed && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f))
        {
            velocityX = -currentMaxVelocity;
        }

        //locking right
        if (rightPressed && runPressed & velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
            if (velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity + 0.05f))
            {
                velocityX = currentMaxVelocity;
            }
        }
        else if (rightPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity + 0.05f))
        {
            velocityX = -currentMaxVelocity;
        }
    }
    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool backPressed = Input.GetKey(KeyCode.S);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        float currentMaxVelocity = runPressed ? maximumRunVelocity : maximumWalkVelocity;

        ChangeVelocity(forwardPressed, leftPressed, rightPressed, backPressed, runPressed, currentMaxVelocity);
        lockOrResetVelocity(forwardPressed, leftPressed, rightPressed, backPressed, runPressed, currentMaxVelocity);

        animator.SetFloat("Velocity Z", velocityZ);
        animator.SetFloat("Velocity X", velocityX);

        //transform.LookAt(targetLook.transform);
        //horizontalMove = Input.GetAxis("Horizontal");
        //verticalMove = Input.GetAxis("Vertical");
        //Debug.Log(horizontalMove);
        //// Movement
        //if (Input.GetKey(KeyCode.W))
        //{
        //    movement.y = 2f;
        //    states = PLAYERSTATE.FWD;
        //    SetAllParametersByDefault("Forward");
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    movement.y = -1f;
        //    states = PLAYERSTATE.BACK;
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    movement.x = 1f;
        //    states = PLAYERSTATE.RIGHT;
        //    SetAllParametersByDefault("Right");

        //}
        //else if (Input.GetKey(KeyCode.A))
        //{
        //    movement.x = -1f;
        //    states = PLAYERSTATE.LEFT;
        //    SetAllParametersByDefault("Left");

        //}
        //else if(Input.GetKey(KeyCode.Space))
        //{
        //    jump = 1f;
        //    states = PLAYERSTATE.JUMP;
        //    SetAllParametersByDefault("Jump");

        //}
        //else
        //{
        //    states = PLAYERSTATE.IDLE;
        //    SetAllParametersByDefault("idle");

        //}

        //// Punch
        //punch = false;
        //anim.SetBool("punch", false);

        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    if(anim.GetBool("idle"))
        //    {
        //        punch = true;
        //        anim.SetBool("idle", false);
        //        anim.SetBool("punch", true);
        //        cineImpulse.GenerateImpulse(Camera.main.transform.forward);
        //    }

        //}


        //// Resolve Movement
        //transform.position += transform.forward * movement.y * Time.deltaTime * moveSpeed;
        //transform.position += transform.right * movement.x * Time.deltaTime * moveSpeed;

        ////Vector3 delta_up = transform.up *jump* jump_force * Time.deltaTime * moveSpeed;
        ////cc.Move(delta_up);
        //switch(states)
        //{
        //    case PLAYERSTATE.IDLE:
        //        anim.SetBool("idle", true);
        //        break;
        //    case PLAYERSTATE.FWD:
        //        anim.SetBool("Forward", true);
        //        break;
        //    case PLAYERSTATE.LEFT:
        //        anim.SetBool("Left", true);
        //        break;
        //    case PLAYERSTATE.RIGHT:
        //        anim.SetBool("Right", true);
        //        break;
        //    case PLAYERSTATE.JUMP:
        //        anim.SetBool("Jump", true);
        //        break;
        //        //default:
        //        //    anim.SetBool("idle", true);
        //        //    break;
        //}
        ////if (movement.magnitude > 0f)
        ////    anim.SetBool("idle", false);
        ////else
        ////    anim.SetBool("idle", true);


        //movement = Vector2.zero;
        //jump = 0f;
    }
    

    void SetAllParametersByDefault(string saveone)
    {
        AnimatorControllerParameter[] parameters_list = animator.parameters;
        for(int i =0; i < animator.parameterCount; i++)
        {
            if(parameters_list[i].type == AnimatorControllerParameterType.Bool && parameters_list[i].name != saveone)
            {
                animator.SetBool(parameters_list[i].name, false);
            }
            
        }
    }

}
