using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    //character movement and flipping
    public float moveSpeed = 1f;
    public float jumpForce = 1f;
    private Rigidbody2D _rigidbody;
    private Vector3 characterScale;
    private float characterScaleX;
    public Transform bendPoint;
    private float bendPointY;

    //character jumping and air jumping
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatisGround;
    private int airJumps;
    public int airJumpValue;

    //camera values
    public float cameraZoomIn;
    public float cameraZoomOut;
    public Camera cameraToZoom;

    bool collisionDetectionActive;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        airJumps = airJumpValue;
        characterScale = transform.localScale;
        characterScaleX = characterScale.x;
        _rigidbody = GetComponent<Rigidbody2D>();
        collisionDetectionActive = false;
        //Stops the sprite forn rotating due to collisions or walking off ledges
        GetComponent<Rigidbody2D>().freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CManager.Instance.Running)
        {
            var movement = Input.GetAxis("Horizontal");
            transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * moveSpeed;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //Debug.Log("Shift Down");
                collisionDetectionActive = !collisionDetectionActive;
                PlatformerUI.Instance.ToggleAirScooter(collisionDetectionActive);
            }
            //Debug.Log(collisionDetectionActive);
            Physics2D.IgnoreLayerCollision(9, 10, collisionDetectionActive);
            Physics2D.IgnoreLayerCollision(9, 11, collisionDetectionActive);
            Physics2D.IgnoreLayerCollision(9, 13, collisionDetectionActive);
            FlipCharacterOld();

            //jumping and airjumping
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatisGround);

            if (isGrounded == true)
            {
                airJumps = airJumpValue;

                if (collisionDetectionActive)
                    animator.SetInteger("state", 3);
                else if (movement > float.Epsilon || movement < -float.Epsilon)
                    animator.SetInteger("state", 1);
                else if (animator.GetInteger("state") != 0)
                    animator.SetInteger("state", 0);

                cameraToZoom.fieldOfView = Mathf.Lerp(cameraToZoom.fieldOfView, cameraZoomIn, Time.deltaTime);
                PlatformerUI.Instance.ToggleAirJump(false);
            }
            else
            {
                if (animator.GetInteger("state") != 2)
                    animator.SetInteger("state", 2);
                cameraToZoom.fieldOfView = Mathf.Lerp(cameraToZoom.fieldOfView, cameraZoomOut, Time.deltaTime);
            }

            if (Input.GetButtonDown("Jump") && airJumps > 0)
            {
                _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                airJumps--;
                animator.SetInteger("state", 2);
                PlatformerUI.Instance.ToggleAirJump(true);
            }
            else if (Input.GetButtonDown("Jump") && airJumps == 0 && isGrounded == true)
            {
                _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                animator.SetInteger("state", 2);
            }
            if (Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
            {
                _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                animator.SetInteger("state", 2);
            }
        }

    }
    private void FlipCharacterOld()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            characterScale.x = -characterScaleX;
            bendPointY = 180;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            characterScale.x = characterScaleX;
            bendPointY = 0;
        }
        transform.localScale = characterScale;
        bendPoint.localRotation = Quaternion.Euler(0, bendPointY, 0);
    }
    
}
