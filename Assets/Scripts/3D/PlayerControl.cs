using UnityEngine;
using System.Collections;

// The GameObject is made to bounce using the space key.
// Also the GameObject can be moved forward/backward and left/right.
// Add a Quad to the scene so this GameObject can collider with a floor.
public class PlayerControl : MonoBehaviour
{
    public float speed = 4.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    public Vector3 moveDirection = Vector3.zero;
    private CharacterController player;

    //everything needed for animations
    private Animator anim;
    private float InputH;
    private float InputV;

    private bool crouch = false;
    private bool Done;

    private AudioSource footsteps;

    void Start()
    {
        footsteps = GetComponent<AudioSource>();
        player = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //speed for sprinting
        if (Input.GetButton("Run"))
        {
            speed = 6f;
        }
        else
        {
            footsteps.Stop();
            speed = 2f;
        }

        //crouch anim
        anim.SetBool("Crouch", Input.GetButton("Crouch"));
       

        //ActionRoll
        anim.SetBool("ActionRoll", Input.GetButton("ActionRoll"));


        if (player.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

            moveDirection = moveDirection * speed;

            //different inputs only used for animations
            InputH = moveDirection.x;
            InputV = moveDirection.z;

            moveDirection = transform.TransformDirection(moveDirection);

            //jump anim
            anim.SetBool("Jump", Input.GetButton("Jump"));
            
            //walk animations
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                anim.SetBool("Idle", false);
            }
            else { anim.SetBool("Idle", true); }
        }

        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        if(!crouch)
        // Move the player
        player.Move(moveDirection * Time.deltaTime);

        //no turning the player if Static button is held
        if (!Input.GetButton("Static"))
        {
            transform.Rotate(0.0f, Input.GetAxis("Mouse X") * 50f * Time.deltaTime, 0.0f);
        }

        //walking animations triggers
        anim.SetFloat("InputH", InputH);
        anim.SetFloat("InputV", InputV);

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "End")
        {
            LevelManager.MainMenu();
           // GameManager.EndGame();
        }
        
        //write code to activate cellgame
        if(other.tag == "Starter")
        {
            GameManager.StartGame();
            GameManager.start = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Starter")
        {
            GameManager.start = false;
        }
    }


    public void Jump()
    {
        moveDirection.y = jumpSpeed;
    }
    public void Crouch()
    {
        crouch = !crouch;
    }
    public void Run()
    {
        footsteps.Play();
    }
}