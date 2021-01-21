using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    float playerSpeed = 5f;
    float jumpForce = 7f;

    int healthPoint = 50;
    int coinCount = 0;

    bool isOnGround = true;

    Animator playerAni;
    Rigidbody2D RB;
    AudioSource audioSource;

    public AudioClip[] audioClip;
    public Text healthText;
    public Text coinText;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        playerAni = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        float hVelocity = 0;
        float vVelocity = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            hVelocity = -playerSpeed; //Negative 
            transform.localScale = new Vector3(-1, 1, 1);
            playerAni.SetFloat("xVelocity", Mathf.Abs(hVelocity));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            hVelocity = playerSpeed; //Positive 
            transform.localScale = new Vector3(1, 1, 1); //localScale to turn left/right 
            playerAni.SetFloat("xVelocity", Mathf.Abs(hVelocity));
        }
        else
        {
            playerAni.SetFloat("xVelocity", 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround == true)
        {
            isOnGround = false;
            vVelocity = jumpForce;
            playerAni.SetTrigger("Jumped");
        }

        hVelocity = Mathf.Clamp(RB.velocity.x + hVelocity, -5, 5); //To limit to given no

        RB.velocity = new Vector2(hVelocity, RB.velocity.y + vVelocity); //Jump things

    }
     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
        if (collision.gameObject.CompareTag("Mace"))
        {
            healthPoint -= 10;
            healthText.GetComponent<Text>().text = "Health: " + healthPoint;
            int rand = Random.Range(1,3);
            audioSource.PlayOneShot(audioClip[rand]);

            if (healthPoint <= 0)
            {
                SceneManager.LoadScene("LoseScene");
            }
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(collision.gameObject);
            coinText.GetComponent<Text>().text = "Coin: " + coinCount;
            audioSource.PlayOneShot(audioClip[0]);
        }

        if (collision.gameObject.CompareTag("Goal"))
        {
            SceneManager.LoadScene("WinScene");
        }
    }
}
