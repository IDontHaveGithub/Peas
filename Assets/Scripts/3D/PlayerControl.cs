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

    private AudioSource footsteps;

    void Start()
    {
        footsteps = GetComponent<AudioSource>();
        player = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        Cursor.visible = false;
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
        if (Input.GetButton("Crouch"))
        {
            anim.SetBool("Crouch", true);
        }
        else
        {
            anim.SetBool("Crouch", false);
        }

        //ActionRoll
        if (Input.GetButton("ActionRoll"))
        {
            anim.SetBool("ActionRoll", true);
        }
        else
        {
            anim.SetBool("ActionRoll", false);
        }


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
            if (Input.GetButton("Jump"))
            {
                anim.SetBool("Jump", true);
            }
            else
            {
                anim.SetBool("Jump", false);
            }

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

        //no turning the player is Static button is held
        if (!Input.GetButton("Static"))
        {
            transform.Rotate(0.0f, Input.GetAxis("Mouse X") * 50f * Time.deltaTime, 0.0f);
        }

        anim.SetFloat("InputH", InputH);
        anim.SetFloat("InputV", InputV);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit2");
        if (other.transform.tag == "End")
        {
            Debug.Log("Hit End");
            GameManager.EndGame();
        }
        
        //write code to activate cellgame
        if(other.tag == "Starter")
        {
            Debug.Log("Start Game");
            GameManager.StartGame();
        }
        
    }

    public void Jump()
    {
        Debug.Log("Jump");
        moveDirection.y = jumpSpeed;
    }

    public void Crouch()
    {
        crouch = true;
    }

    public void NotCrouch()
    {
        crouch = false;
    }
    public void Run()
    {
        footsteps.Play();
    }
}