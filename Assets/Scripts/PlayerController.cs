using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    float playerSpeed = 5f;
    float jumpForce = 0.5f;

    int healthPoint = 50;
    int coinCount = 0;

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

        if (Input.GetKey(KeyCode.Space))
        {
            vVelocity = jumpForce;
            playerAni.SetTrigger("Jumped");
        }

        hVelocity = Mathf.Clamp(RB.velocity.x + hVelocity, -5, 5); //To limit to given no

        RB.velocity = new Vector2(hVelocity, RB.velocity.y + vVelocity); //Jump things
    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mace"))
        {
            healthPoint -= 10;
            healthText.GetComponent<Text>().text = "Health: " + healthPoint;
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coinCount++;
            coinText.GetComponent<Text>().text = "Coin: " + coinCount;
            audioSource.PlayOneShot(audioClip[0]);
        }
    }
}
