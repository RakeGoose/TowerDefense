using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;
    public GameObject Menu;
    public Text MoneyTxt;
    public Text LivesTxt;
    public int points, livesCount;
    public bool canSpawn = false;
    private bool isPaused = false;
    public int waveCount = 0;
    public int maxWaves = 25;

    public GameObject WinPanel;
    public GameObject GameOverPanel;
    public GameObject PausePanel;

    public AudioClip LoseSound;
    public AudioClip EnemyDieSound;
    public AudioClip winSound;
    public AudioClip gameMusic;
    public AudioClip placeTowerSound;
    private AudioSource musicSource;
    

    public Image DamageOverlay;
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.2f;

    public TextMeshProUGUI WaveTxt;
    public TextMeshProUGUI WaveStartTxt;
    public CanvasGroup WaveStartGroup;

    Coroutine shakeRoutine;
    Coroutine flashRoutine;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        musicSource = gameObject.GetComponent<AudioSource>();
        musicSource.clip = gameMusic;
        musicSource.loop = true;
        musicSource.volume = 0.3f;
        musicSource.Play();
    }


    void Update()
    {
        MoneyTxt.text = points.ToString();
        LivesTxt.text = "LIVES: " + livesCount.ToString();
        WaveTxt.text = $"WAVES: {waveCount}/{maxWaves}";

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void OnWaveComplete()
    {
        

        if(waveCount >= maxWaves)
        {
            WinGame();
        }
    }

    public void PlayBtn()
    {
        FindObjectOfType<LevelManager>().CreateLevel();
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        spawner.enemiesPerWave = 3;
        spawner.StopAllCoroutines();
        Menu.SetActive(false);
        WinPanel.SetActive(false);
        canSpawn = true;
        waveCount = 0;
        Time.timeScale = 1f;

        FindObjectOfType<EnemySpawner>().StartSpawning();
    }

    public void RetryLevel()
    {
        FindObjectOfType<EnemySpawner>().StopAllCoroutines();
        foreach (EnemyLogic es in FindObjectsOfType<EnemyLogic>())
        {
            es.StopAllCoroutines();
            Destroy(es.gameObject);
        }
        foreach (TowerFireScript ts in FindObjectsOfType<TowerFireScript>())
            Destroy(ts.gameObject);
        foreach (cellScript cs in FindObjectsOfType<cellScript>())
            Destroy(cs.gameObject);
        GameOverPanel.SetActive(false);
        FindObjectOfType<LevelManager>().CreateLevel();
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        spawner.enemiesPerWave = 3;
        WinPanel.SetActive(false);
        canSpawn = true;
        waveCount = 0;
        Time.timeScale = 1f;

        FindObjectOfType<EnemySpawner>().StartSpawning();
    }

    public void GoToMenuBtn()
    {
        ToMenu();
        GameOverPanel.SetActive(false);
    }

    public void ExitBtn()
    {
        Application.Quit();
    }

    public void OnContinueButton()
    {
        ResumeGame();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        PausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
    }

    void WinGame()
    {
        musicSource.Stop();
        GetComponent<AudioSource>().PlayOneShot(winSound);
        canSpawn = false;
        FindObjectOfType<EnemySpawner>().StopAllCoroutines();
        foreach (EnemyLogic es in FindObjectsOfType<EnemyLogic>())
        {
            es.StopAllCoroutines();
            Destroy(es.gameObject);
        }
        foreach (TowerFireScript ts in FindObjectsOfType<TowerFireScript>())
            Destroy(ts.gameObject);
        foreach (cellScript cs in FindObjectsOfType<cellScript>())
            Destroy(cs.gameObject);

        if (WinPanel != null)
        {
            WinPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        musicSource.Stop();
        PlayLoseSound();
        canSpawn = false;
        Time.timeScale = 0f;

        if (shakeRoutine != null) StopCoroutine(shakeRoutine);
        if (flashRoutine != null) StopCoroutine(flashRoutine);

        DamageOverlay.color = new Color(1, 0, 0, 0);

        FindObjectOfType<EnemySpawner>().StopAllCoroutines();
        foreach (EnemyLogic es in FindObjectsOfType<EnemyLogic>())
        {
            es.StopAllCoroutines();
            Destroy(es.gameObject);
        }
        foreach (TowerFireScript ts in FindObjectsOfType<TowerFireScript>())
            Destroy(ts.gameObject);
        foreach (cellScript cs in FindObjectsOfType<cellScript>())
            Destroy(cs.gameObject);
        GameOverPanel.SetActive(true);
        if (FindObjectsOfType<ShopLogic>().Length > 0)
            Destroy(FindObjectOfType<ShopLogic>().gameObject);
    }

    void ToMenu()
    {
        FindObjectOfType<EnemySpawner>().StopAllCoroutines();
        foreach (EnemyLogic es in FindObjectsOfType<EnemyLogic>())
        {
            es.StopAllCoroutines();
            Destroy(es.gameObject);
        }
        foreach (TowerFireScript ts in FindObjectsOfType<TowerFireScript>())
            Destroy(ts.gameObject);
        foreach (cellScript cs in FindObjectsOfType<cellScript>())
            Destroy(cs.gameObject);

        points = 30;
        livesCount = 20;

        Menu.SetActive(true);
        canSpawn = false;

        if(FindObjectsOfType<ShopLogic>().Length > 0)
            Destroy(FindObjectOfType<ShopLogic>().gameObject);
    }

    public void TakePlayerDamage()
    {
        TriggerDamageEffect();

        if(livesCount > 1)
        {
            livesCount--;
        }
        else
        {
            livesCount = 20;
            points = 30;
            GameOver();
        }
    }

    public void PlayLoseSound()
    {
        GetComponent<AudioSource>().PlayOneShot(LoseSound);
    }

    public void PlayPlaceTowerSound()
    {
        GetComponent<AudioSource>().volume = 3f;
        GetComponent<AudioSource>().PlayOneShot(placeTowerSound);
    }

    public void PlayEnemyDieSound()
    {
        GetComponent<AudioSource>().PlayOneShot(EnemyDieSound);
    }

    public void TriggerDamageEffect()
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        shakeRoutine = StartCoroutine(ScreenShake());
        flashRoutine = StartCoroutine(RedFlash());
    }

    public void ShowWaveStart(int waveNumber)
    {
        StartCoroutine(ShowWaveStartRoutine(waveNumber));
    }

    IEnumerator ShowWaveStartRoutine(int waveNumber)
    {
        WaveStartTxt.text = $"WAVE {waveNumber}";
        WaveStartGroup.gameObject.SetActive(true);
        WaveStartGroup.alpha = 0f;

        float duration = 0.5f;
        float elapsed = 0f;
        while(elapsed < duration)
        {
            WaveStartGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        WaveStartGroup.alpha = 1f;

        yield return new WaitForSeconds(1.5f);

        elapsed = 0f;
        while(elapsed < duration)
        {
            WaveStartGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        WaveStartGroup.alpha = 0f;
        WaveStartGroup.gameObject.SetActive(false);
    }

    IEnumerator ScreenShake()
    {
        Vector3 originalPos = Camera.main.transform.position;
        float elapsed = 0f;

        while(elapsed < shakeDuration)
        {
            if (!canSpawn)
                break;
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            Camera.main.transform.position = new Vector3(originalPos.x + offsetX, originalPos.y + offsetY, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = originalPos;
    }

    IEnumerator RedFlash()
    {
        float duration = 2f;
        float elapsed = 0f;
        Color baseColor = new Color(1, 0, 0, 0.3f);

        DamageOverlay.color = baseColor;

        while(elapsed < duration)
        {
            float alpha = Mathf.Lerp(0.5f, 0f, elapsed / duration);
            DamageOverlay.color = new Color(1, 0, 0, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        DamageOverlay.color = new Color(1, 0, 0, 0);
    }
}
