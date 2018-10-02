using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 2f;
    public float maxSpeed = 5f;
    public bool grounded;
    public float jumpPower = 6.5f;
    public int maxCoinAmmount = 5;


    public GameObject coin;
    

    private Rigidbody2D rb2d;
    private Animator anim;
    private bool jump;
    private float timeSinceLastDrop = 0.0f;
    private float dropInterval = 3.0f;
    private myGlobalVars myVars;

    // Use this for initialization
    void Start () {
        myVars = GetComponentInParent<myGlobalVars>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spawnCoin();
        dropInterval = Random.Range(2.0f, 5.0f);
    }
	
	// Update is called once per frame
	void Update () {
        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetBool("Grounded", grounded);

        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
        {
            jump = true;
        }

        if (timeSinceLastDrop >= dropInterval&&myGlobalVars.coinsNow<maxCoinAmmount)
        {
            spawnCoin();
            myGlobalVars.coinsNow++;
            timeSinceLastDrop = 0;
            dropInterval = Random.Range(2.0f, 5.0f);
        }
        else
            timeSinceLastDrop += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Vector3 fixedVelocity = rb2d.velocity;
        fixedVelocity.x *= 0.75f;

        if (grounded)
        {
            rb2d.velocity = fixedVelocity;
        }

        float h = Input.GetAxis("Horizontal");

        rb2d.AddForce(Vector2.right * speed * h);

        float limitedSpeed = Mathf.Clamp(rb2d.velocity.x,-maxSpeed, maxSpeed);
        rb2d.velocity = new Vector2(limitedSpeed, rb2d.velocity.y);
       
        if(h > 0.1f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if(h < -0.1f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (jump)
        {
            rb2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jump = false;
        }
    }

    private void OnBecameInvisible()
    {
        transform.position = new Vector3(-1, 0, 0);
    }

    private void spawnCoin()
    {
        Vector2 position = new Vector2(Random.Range(-10, -1), -1);
        Instantiate(coin, position, Quaternion.identity);
    }
}
