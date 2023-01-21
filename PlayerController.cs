using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public enum eMode { idle, idleF, attackUp, attackUpF, attackDown, attackDownF, lost, lostF }

    public eMode mode = eMode.idle;

    public Game game;
    [SerializeField] private int lives;
    public TextMeshProUGUI livesText;
    private Animator anim;
    private SpriteRenderer sRend;    
    private bool hasLost;    
    private bool isSounding;
    private bool isActing;

    public AudioClip[] yells;
    public AudioClip[] swings;
    public AudioClip death;
    private AudioSource playerAudio;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sRend = GetComponent<SpriteRenderer>();
        playerAudio = GetComponent<AudioSource>();
    }    
   
    void FixedUpdate()
    {        
        PlayerPCInput();
        
        SamuraiLose();

        livesText.text = $"{lives}";


    }
    
    void PlayerPCInput()
    {

        if (hasLost) return;
       
            if (Input.GetKey(KeyCode.E))
            {
                if (isActing) return;
                isActing = true;
                mode = eMode.attackUp;
                ProcessSamuraiMode();
             }
            if (Input.GetKey(KeyCode.Q))
            {
                if (isActing) return;
                isActing = true;
                mode = eMode.attackUpF;
            ProcessSamuraiMode();
        }
            if (Input.GetKey(KeyCode.D))
            {
                if (isActing) return;
                isActing = true;
                mode = eMode.attackDown;
            ProcessSamuraiMode();
        }
            if (Input.GetKey(KeyCode.A))
            {
                if (isActing) return;
                isActing = true;
                mode = eMode.attackDownF;
            ProcessSamuraiMode();
        }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (isActing) return;
                isActing = true;
                if (sRend.flipX == false) mode = eMode.attackUp;
                else mode = eMode.attackUpF;
            ProcessSamuraiMode();
        }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (isActing) return;
                isActing = true;
                if (sRend.flipX == false) mode = eMode.attackDown;
                else mode = eMode.attackDownF;
            ProcessSamuraiMode();
        }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (isActing) return;
                isActing = true;
                mode = eMode.idleF;
            ProcessSamuraiMode();
        }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (isActing) return;
                isActing = true;
                mode = eMode.idle;
            ProcessSamuraiMode();
        }
          
 
    }   
    /// <summary>
    /// 4 attack methods for MobileInput, bound to tap buttons
    /// </summary>
    public void MobileAttackRightUp ()
    {
        if (hasLost) return;

        if (isActing) return;
        isActing = true;
        mode = eMode.attackUp;
        ProcessSamuraiMode();
    }

    public void MobileAttackRightDown()
    {
        if (hasLost) return;

        if (isActing) return;
        isActing = true;
        mode = eMode.attackDown;
        ProcessSamuraiMode();
    }

    public void MobileAttackLeftUp()
    {
        if (hasLost) return;

        if (isActing) return;
        isActing = true;
        mode = eMode.attackUpF;
        ProcessSamuraiMode();
    }

    public void MobileAttackLeftDown()
    {
        if (hasLost) return;

        if (isActing) return;
        isActing = true;
        mode = eMode.attackDownF;
        ProcessSamuraiMode();
    }

    void ProcessSamuraiMode()
    {
        switch (mode)
        {
            case eMode.idle:
                anim.CrossFade("anim_Idle", 0);
                sRend.flipX = false;
                isActing = false;
                break;
            case eMode.idleF:
                anim.CrossFade("anim_Idle", 0);
                sRend.flipX = true;
                isActing = false;
                break;
            case eMode.attackUp:
                PlaySounds();
                anim.CrossFade("anim_AttackUp", 0);
                sRend.flipX = false;                
                StartCoroutine(BackToIdle(0));
                break;
            case eMode.attackUpF:
                PlaySounds();
                anim.CrossFade("anim_AttackUp", 0);
                sRend.flipX = true;                
                StartCoroutine(BackToIdle(1));
                break;
            case eMode.attackDown:
                PlaySounds();
                anim.CrossFade("anim_AttackDown", 0);
                sRend.flipX = false;                
                StartCoroutine(BackToIdle(0));
                break;
            case eMode.attackDownF:
                PlaySounds();
                anim.CrossFade("anim_AttackDown", 0);
                sRend.flipX = true;                
                StartCoroutine(BackToIdle(1));
                break;
        }
    }
    IEnumerator BackToIdle(int flip)
    {        
        yield return new WaitForSeconds(0.25f);        
        if (flip == 0) mode = eMode.idle;
        else mode = eMode.idleF;
        isActing = false;
        ProcessSamuraiMode();
    }
    void SamuraiLose()
    {
        if (lives <= 0)
        {
            if (!hasLost) playerAudio.PlayOneShot(death);
            hasLost = true;
            if (sRend.flipX)
            {
                mode = eMode.lost;            
            }
            else mode = eMode.lostF;            
            anim.CrossFade("anim_Lose", 0);
            game.SetGameOver();
        }     
        
    }
    public void SetLives(int value)
    {
        lives += value;
        if (lives < 0) lives = 0;
    }
    void PlaySounds()
    {
        if (isSounding) return;
        StartCoroutine(SoundCountDown());
        PlayRandomSwordSwing();
        PlayRandomYell();

    }
    void PlayRandomSwordSwing()
    {
        playerAudio.PlayOneShot(swings[Random.Range(0, swings.Length)]);
    }
    void PlayRandomYell()
    {
        int randomYell = Random.Range(0, 3);
        if (randomYell == 0) playerAudio.PlayOneShot(yells[Random.Range(0, yells.Length)]);
    }  
    IEnumerator SoundCountDown()
    {
        isSounding = true;
        yield return new WaitForSeconds(0.5f);
        isSounding = false;
    }
}
