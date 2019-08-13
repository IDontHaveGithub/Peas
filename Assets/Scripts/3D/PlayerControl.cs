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

    private float InputH;
    private float InputV;

    private Vector3 moveDirection = Vector3.zero;

    private bool crouch = false;

    private CharacterController player;
    private Animator anim;
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

        //Action roll
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

            //actual movement
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

        // Move the player
        if (!crouch)
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
        //HACK: searching for tags is a nice way, but a bit rudimental, can you change this
        if (other.transform.tag == "End")
        {
            LevelManager.MainMenu();
           // GameManager.EndGame(); //this is for now a debug and a application.quit
           // TODO: think of a better ending to the game
        }
        
        //write code to activate cellgame
        if(other.tag == "Starter")
        {
            GameManager.StartGame(other.transform.parent.GetSiblingIndex(), other.transform.parent.gameObject); //this is for now a debug
            GameManager.start = true; //BUG: this bool is not being used
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