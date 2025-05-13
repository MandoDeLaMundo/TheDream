using System.Data.SqlTypes;
using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] int speed;
    [SerializeField] int sprintMod;

    [SerializeField] bool shooting;
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    float shootTimer;

    [SerializeField] int jumpMax;
    [SerializeField] int jumpForce;
    [SerializeField] int Gravity;
    int jumpCount;
    Vector3 playerVel;



    Vector3 moveDir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        }
        Movement();
        sprint();
    }

    void Movement()
    {
        shootTimer += Time.deltaTime;

        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);
        
        controller.Move(moveDir * speed * Time.deltaTime);

        jump();

        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= Gravity * Time.deltaTime;
        if (Input.GetButton("Fire1") && shootTimer >= shootRate && shooting)
        {
            shoot();
        }
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax) 
        {
            jumpCount++;
            playerVel.y = jumpForce;
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed += sprintMod;
        }
        if (Input.GetButtonUp("Sprint"))
        {
            speed -= sprintMod;
        }
    }

    void shoot()
    {
        shootTimer = 0;

        RaycastHit hit;

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {

        }
    }
}
