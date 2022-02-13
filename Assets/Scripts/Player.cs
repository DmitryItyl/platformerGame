using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    //Player movement stats
    [SerializeField] float speed = 2f;
    [SerializeField] float jumpPower = 1100f;

    //Ground check for jumping utility
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;

    //Respawn & checkpoint
    [SerializeField] Transform respawnPoint;
    [SerializeField] Transform checkpoint;
    [SerializeField] GameObject gameOverScreen;
    private Transform currentSpawnPoint;
    private bool checkPointMade = false;

    //Player objects
    Rigidbody2D body;
    Animator animator;

    //Pre-determined values 
    const float groundCheckRadius = 0.2f;
    float playerScale = 3f;
    float walkSpeedModifier = 0.5f;
    
    //Movement utility value
    float horizontalValue;

    //Flags
    [SerializeField] bool isGrounded;
    bool isWalking = false;
    bool facingRight = true;
    bool isJumping = false;

    // Life counter
    public Image[] lives;
    public int livesRemaining;

    // Score displayer
    public Text scoreText;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpawnPoint = respawnPoint;
    }

    private void Start()
    {
        scoreText.text = "Score: " + ScoreManager.totalScore;
    }

    private void Update()
    {
        // Left shift to walk slower (not avialable on mobile devices)      
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isWalking = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isWalking = false;
        }

        #region Android movement using UI buttons
        horizontalValue = CrossPlatformInputManager.GetAxisRaw("Horizontal");

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            animator.SetBool("Jump", true);
            isJumping = true;
        }
        else if (CrossPlatformInputManager.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
        #endregion


        #region Windows movement using keyboard (for playtesting)
        /*
        horizontalValue = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Jump", true);
            isJumping = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
        */
        #endregion

        //Determining if character should use jumping or falling animation
        animator.SetFloat("yVelocity", body.velocity.y);
    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue, isJumping);
    }

    void GroundCheck()
    {
        //Check if any objects in the "Ground" layer collide with players ground checking collider
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        
        //False by default
        isGrounded = false;
        if (colliders.Length > 0)
            isGrounded = true;

        animator.SetBool("Jump", !isGrounded);
    }

    void Move(float dir, bool jumpFlag)
    {
        #region Horizontal Movement
        float xVel = dir * speed * 100 * Time.fixedDeltaTime;

        if (isWalking)
        {
            xVel *= walkSpeedModifier;
        }

        Vector2 targetVelocity = new Vector2(xVel, body.velocity.y);
        body.velocity = targetVelocity;

        if (facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-playerScale, playerScale, playerScale);
            facingRight = false;
        }
        else if (!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(playerScale, playerScale, playerScale);
            facingRight = true;
        }

        animator.SetFloat("xVelocity", Mathf.Abs(body.velocity.x));
        #endregion

        #region Jumping

        if (isGrounded && jumpFlag)
        {
            body.AddForce(new Vector2(0f, jumpPower));
            isGrounded = false;
            jumpFlag = false;
        }

        #endregion
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            //If collide with coins: add score, update score prompt, remove coin from the playfield
            case "Coins":
                ScoreManager.AddCoinScore();
                scoreText.text = "Score: " + ScoreManager.totalScore;
                collision.gameObject.SetActive(false);
                break;

            //Make checkpoint (one checkpoint per level so only do something if checkpoint isn't made)
            case "Checkpoint":
                if (!checkPointMade)
                {
                    currentSpawnPoint = checkpoint;
                    checkPointMade = true;
                    Animator checkpointAnimator = checkpoint.GetComponent<Animator>();
                    checkpointAnimator.SetBool("Checked", true);
                }
                break;
 
            case "FallZone":
                LoseLife();
                break;

            //Making it to the end of the level, goint to the next level, update score prompt on the new level
            case "Finish":
                int newLevel = SceneManager.GetActiveScene().buildIndex + 1;

                PlayerPrefs.SetInt("Score", ScoreManager.totalScore);
                PlayerPrefs.SetInt("Level", newLevel);

                SceneManager.LoadScene(newLevel);
                checkPointMade = false;
            
                break;

            default:
                break;
        }
    }

    public void LoseLife()
    {
        //Handling UI "lives" elements
        livesRemaining--;
        lives[livesRemaining].enabled = false;

        //Respawn player or end game
        if (livesRemaining == 0)
        {
            GameOver();
        }
        else 
        {
            transform.position = currentSpawnPoint.position;
        }
    }

    public void GameOver()
    {
        ScoreManager.ResetScore();
        PlayerPrefs.SetInt("Level", 1);
        Time.timeScale = 0f;
        Menu.isPaused = true;
        gameOverScreen.SetActive(true);
    }
}
