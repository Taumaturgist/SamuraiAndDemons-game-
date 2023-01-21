using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isMortal;
    public PlayerController playerController;
    public float speed = 1;
    public int points = 1;
    public GameObject[] explosions;
    private GameObject hitBox;  
    private Game game;    

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();  
        game = GameObject.Find("Main Camera").GetComponent<Game>();        
        if (transform.position.x < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        speed = game.GetSpeed();
    }
    private void Update()
    {
        if (isMortal) SeekDeath();
        KillOnGameOver();
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitBox")
        {
            isMortal = true;
            hitBox = collision.gameObject;
            Debug.Log(hitBox.name);
        }
            
            

        if (collision.gameObject.tag == "SamuraiHitBox")
        {
            playerController.SetLives(-1);
            Instantiate(explosions[1], transform.position, transform.rotation);            
            Destroy(gameObject);
        }
            
            
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitBox")
        {
            isMortal = false;
            hitBox = null;
        }
            
    }  

    void SeekDeath()
    {
        if (hitBox.name == "RUhitbox" && playerController.mode == PlayerController.eMode.attackUp) StartCoroutine(KillWithDelay());
        if (hitBox.name == "LUhitbox" && playerController.mode == PlayerController.eMode.attackUpF) StartCoroutine(KillWithDelay());
        if (hitBox.name == "RDhitbox" && playerController.mode == PlayerController.eMode.attackDown) StartCoroutine(KillWithDelay());
        if (hitBox.name == "LDhitbox" && playerController.mode == PlayerController.eMode.attackDownF) StartCoroutine(KillWithDelay());
       
    }

    IEnumerator KillWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        game.SetScore(points);
        Instantiate(explosions[0], transform.position, transform.rotation);        
        Destroy(gameObject);
    }

    private void Move()
    {   
        transform.Translate(new Vector2(-1, 0) * speed * Time.deltaTime);
    }
    void KillOnGameOver()
    {
        if (game.GetGameOver())
        {
            Instantiate(explosions[1], transform.position, transform.rotation);            
            Destroy(gameObject);
        }
    }

}
