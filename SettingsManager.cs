using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public GameObject background;
    public GameObject mist;
    public GameObject btnDayNight;
    public GameObject btnDayNight2;
    public GameObject sunImage;
    public GameObject moonImage;
    public GameObject sunImage2;
    public GameObject moonImage2;

    public GameObject pauseMenu;
    public GameObject restartMenu;
    public GameObject creditsMenu;
    //mobile only
    private GameObject mobileInputMenu;
    

    public GameObject mainTrack;
    public GameObject defeatTrack;

    private Game game;
    [SerializeField] private bool isNightTheme;
    [SerializeField] private bool isGamePaused = true;
    [SerializeField] private bool isSoundOn = true;
    public GameObject soundOn1;
    public GameObject soundOn2;
    public GameObject soundOff1;
    public GameObject soundOff2;

    public GameObject recordPanel;

    public TextMeshProUGUI bestScoreTextPause;
    public TextMeshProUGUI bestScoreTextDefeat;
    private int bestScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        game = GetComponent<Game>();
        if (PlayerPrefs.HasKey("best"))
        {
            bestScore = PlayerPrefs.GetInt("best");
            bestScoreTextPause.text = PlayerPrefs.GetInt("best").ToString();
            bestScoreTextDefeat.text = PlayerPrefs.GetInt("best").ToString();
        }
        if (PlayerPrefs.HasKey("night"))
        {
            if (PlayerPrefs.GetInt("night") == 1) isNightTheme = false;
            else isNightTheme = true;
            SetDayNightTheme();
        }
        if (PlayerPrefs.HasKey("sound"))
        {
            if (PlayerPrefs.GetInt("sound") == 1) isSoundOn = false;
            else isSoundOn = true;
            SoundOnOff();
        }

        mobileInputMenu = GameObject.Find("Taps");
        mobileInputMenu.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !game.GetGameOver()) PauseOnOff();
    }
    public void SetDayNightTheme()
    {
        isNightTheme = !isNightTheme;

        if (isNightTheme)
        {
            background.GetComponent<SpriteRenderer>().color = Color.black;
            mist.gameObject.SetActive(false);            
            sunImage.gameObject.SetActive(true);
            moonImage.gameObject.SetActive(false);
            btnDayNight.GetComponent<Image>().color = Color.white;
            sunImage2.gameObject.SetActive(true);
            moonImage2.gameObject.SetActive(false);
            btnDayNight2.GetComponent<Image>().color = Color.white;

        }
        else
        {
            background.GetComponent<SpriteRenderer>().color = new Color(255f/255f, 170f/255f, 170f/255f);
            mist.gameObject.SetActive(true);
            sunImage.gameObject.SetActive(false);
            moonImage.gameObject.SetActive(true);
            btnDayNight.GetComponent<Image>().color = Color.black;
            sunImage2.gameObject.SetActive(false);
            moonImage2.gameObject.SetActive(true);
            btnDayNight2.GetComponent<Image>().color = Color.black;
        }

        RememberDayNightChoice();

    }
    public void PauseOnOff()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0f;
            if (mobileInputMenu != null)
            {
                mobileInputMenu.gameObject.SetActive(false);
            }
        }
        else
        {
            pauseMenu.gameObject.SetActive(false);
            creditsMenu.gameObject.SetActive(false);
            defeatTrack.gameObject.SetActive(false);
            mainTrack.gameObject.SetActive(true);
            Time.timeScale = 1f;
            
            if (mobileInputMenu != null)
            {
                mobileInputMenu.gameObject.SetActive(true);
            }
        }
    }
    public void RestartGame()
    {        
        SceneManager.LoadScene(0);
    }
    public void SetRestartMenu()
    {
        defeatTrack.gameObject.SetActive(true);
        mainTrack.gameObject.SetActive(false);        
        StartCoroutine(WaitForRestart());
    }
    IEnumerator WaitForRestart()
    {
        yield return new WaitForSeconds(2);
        restartMenu.gameObject.SetActive(true);
        SetBestScore();
    }
    public void SoundOnOff()
    {
        isSoundOn = !isSoundOn;

        if (isSoundOn)
        {
            AudioListener.volume = 1f;
            soundOn1.gameObject.SetActive(true);
            soundOn2.gameObject.SetActive(true);
            soundOff1.gameObject.SetActive(false);
            soundOff2.gameObject.SetActive(false);
        }            
        else
        {
            AudioListener.volume = 0f;
            soundOn1.gameObject.SetActive(false);
            soundOn2.gameObject.SetActive(false);
            soundOff1.gameObject.SetActive(true);
            soundOff2.gameObject.SetActive(true);
        }

        RememberSoundChoice();
    }

    void SetBestScore()
    {
        int currentScore = game.GetScore();
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            if (game.GetGameOver()) StartCoroutine(FlashRecord());
            bestScoreTextPause.text = bestScore.ToString();
            bestScoreTextDefeat.text = bestScore.ToString();
            PlayerPrefs.SetInt("best", bestScore);
        }
        
    }
    IEnumerator FlashRecord()
    {
        WaitForSeconds wait = new WaitForSeconds(0.3f);
        yield return wait;
        recordPanel.GetComponent<Image>().color = Color.green;
        yield return wait;
        recordPanel.GetComponent<Image>().color = Color.white;
        yield return wait;
        recordPanel.GetComponent<Image>().color = Color.green;
        yield return wait;
        recordPanel.GetComponent<Image>().color = Color.white;
        yield return wait;
        recordPanel.GetComponent<Image>().color = Color.green;
        yield return wait;
        recordPanel.GetComponent<Image>().color = Color.white;        
    }
    public void FromMainToCredits()
    {
        pauseMenu.gameObject.SetActive(false);
        restartMenu.gameObject.SetActive(false);
        creditsMenu.gameObject.SetActive(true);
    }
    public void FromCreditsBack()
    {        
        creditsMenu.gameObject.SetActive(false);
        if (game.GetGameOver()) restartMenu.gameObject.SetActive(true);
        else pauseMenu.gameObject.SetActive(true);        
    }  
    void RememberDayNightChoice()
    {
        if (isNightTheme)
        {
            PlayerPrefs.SetInt("night", 1);
        }
        else PlayerPrefs.SetInt("night", 0);
    }
    void RememberSoundChoice()
    {
        if (isSoundOn)
        {
            PlayerPrefs.SetInt("sound", 1);
        }
        else PlayerPrefs.SetInt("sound", 0);
    }
    public void QuitGame()
    {        
        SetBestScore();        
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

}
