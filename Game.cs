using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{
    public GameObject[] flyingMobs;
    public GameObject[] groundMobs;
    public GameObject[] spawnPoints;
    public AudioClip[] mobGrunts;    

    public TextMeshProUGUI scoreText;
    private int score = 0;

    private SettingsManager settings;

    private bool isGameOver;
    private float speed = 1f;
    private float spawnRate = 1f;
    private AudioSource gameAudio;

    public GameObject btnPause;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnRandomMob", 5.8f);
        settings = GetComponent<SettingsManager>();
        gameAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"{score}";
        speed = 1f + 0.01f * score;
        spawnRate = 1f - 0.002f * score;
        if (spawnRate < 0.3f) spawnRate = 0.3f;
    }
    void SpawnRandomMob()
    {
        if (!isGameOver)
        {
            int randomInt = Random.Range(0, 4);
            if (randomInt <= 1) Instantiate(flyingMobs[Random.Range(0, flyingMobs.Length)], spawnPoints[randomInt].transform.position, transform.rotation);
            else Instantiate(groundMobs[Random.Range(0, groundMobs.Length)], spawnPoints[randomInt].transform.position, transform.rotation);
            int randomSoundPlay = Random.Range(0, 5);
            if (randomSoundPlay == 0) gameAudio.PlayOneShot(mobGrunts[Random.Range(0, mobGrunts.Length)]);
            Invoke("SpawnRandomMob", spawnRate);        
        }

    }
    public void SetScore(int value)
    {
        score += value;
    }
    public int GetScore()
    {
        return score;
    }
    public void SetGameOver()
    {
        isGameOver = true;
        btnPause.gameObject.SetActive(false);
        settings.SetRestartMenu();

    }
    public bool GetGameOver()
    {
        return isGameOver;
    }
    public float GetSpeed()
    {
        return speed;
    }
}
