using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boxer : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject targetLook;

    [SerializeField]
    [Range(1f,3f)]
    float moveSpeed = 1.5f;

    Vector2 movement;

    CharacterController cc;

    Animator anim;

    bool punch;

    bool dodgeR, dodgeL;

    // camera shake
    CinemachineImpulseSource cineImpulse;

    void Start()
    {
        movement = new Vector2();
        movement = Vector2.zero;
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        cineImpulse = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(targetLook.transform);

        // Movement
        if (Input.GetKey(KeyCode.W))
            movement.y = 1f;

        if (Input.GetKey(KeyCode.S))
            movement.y = -1f;

        if (Input.GetKey(KeyCode.D))
            movement.x = 1f;

        if (Input.GetKey(KeyCode.A))
            movement.x = -1f;

        // Punch
        punch = false;
        anim.SetBool("punch", false);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(anim.GetBool("idle"))
            {
                punch = true;
                anim.SetBool("idle", false);
                anim.SetBool("punch", true);
                cineImpulse.GenerateImpulse(Camera.main.transform.forward);
            }
     
        }

        // Dodge

        dodgeL = false;
        anim.SetBool("dodge left", false);

        dodgeR = false;
        anim.SetBool("dodge right", false);


        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (anim.GetBool("idle"))
            {
                punch = true;
                anim.SetBool("idle", false);
                anim.SetBool("dodge left", true);
            }

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (anim.GetBool("idle"))
            {
                punch = true;
                anim.SetBool("idle", false);
                anim.SetBool("dodge right", true);
            }

        }

        // Resolve Movement
        Vector3 delta_fwd = transform.forward * movement.y * Time.deltaTime * moveSpeed;
        cc.Move(delta_fwd);

        Vector3 delta_side = transform.right * movement.x * Time.deltaTime * moveSpeed;
        cc.Move(delta_side);

        if (movement.magnitude > 0f)
            anim.SetBool("idle", false);
        else
            anim.SetBool("idle", true);


        movement = Vector2.zero;
    }



}
