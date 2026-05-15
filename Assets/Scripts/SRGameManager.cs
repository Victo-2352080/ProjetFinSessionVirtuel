using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class SRGameManager : MonoBehaviour
{
    public static SRGameManager Instance { get; private set; }

    [Header("Timer")]
    [SerializeField] private float gameDuration = 120f;
    private float timer;

    [Header("Score")]
    [SerializeField] private int score = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bestScoresText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private RawImage countdownBackground;
    [SerializeField] private GameObject menuTitle;
    [SerializeField] private TMP_Text gameOverText;

    [Header("Sound")]
    [SerializeField] private AudioClip backgroundMusic;
    private AudioSource audioSource;

    private List<int> bestScores = new List<int>();

    private bool gameStarted = false;
    public bool IsGameStarted => gameStarted;

    public UnityAction OnGameStart;
    public UnityAction OnGameEnd;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.clip = backgroundMusic;
            audioSource.pitch = 1f;
            audioSource.playOnAwake = false;
        }
        timer = gameDuration;

        UpdateUI();
        UpdateMenuTitleVisibility(true);
        UpdateGameOverText(false);
    }

    void Update()
    {
        if (!gameStarted) return;
        UpdateTimer();
        UpdateUI();
    }

    private void UpdateTimer()
    {
        if (timer <= 0f) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 0f;
            EndGame();
        }
    }

    private void UpdateUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);

            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }

        if (bestScoresText != null)
        {
            bestScoresText.text = GetBestScoresFormatted();
        }
    }

    private void UpdateMenuTitleVisibility(bool visible)
    {
        if (menuTitle != null)
            menuTitle.SetActive(visible);
    }

    private void UpdateGameOverText(bool visible)
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(visible);
        }
    }

    public void AddScore(int value)
    {
        score += value;
    }

    public void EndGame()
    {
        OnGameEnd?.Invoke();
        AddToBestScores(score);
        timer = gameDuration;
        gameStarted = false;
        UpdateUI();
        UpdateMenuTitleVisibility(true);
        UpdateGameOverText(true);
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();
        UpdateMenuTitleVisibility(false);
        UpdateGameOverText(false);
        StartCoroutine(StartCountdown());
    }

    private void AddToBestScores(int newScore)
    {
        bestScores.Add(newScore);
        bestScores.Sort((a, b) => b.CompareTo(a));

        if (bestScores.Count > 3)
        {
            bestScores.RemoveAt(bestScores.Count - 1);
        }
    }

    private string GetBestScoresFormatted()
    {
        string result = "";

        for (int i = 0; i < bestScores.Count; i++)
        {
            result += bestScores[i];

            if (i < bestScores.Count - 1)
                result += "\n";
        }

        return result;
    }

    private System.Collections.IEnumerator StartCountdown()
    {
        score = 0;
        timer = gameDuration;

        int count = 3;

        if (countdownText != null)
        {
            countdownBackground.gameObject.SetActive(true);
            countdownText.gameObject.SetActive(true);
        }


        while (count > 0)
        {
            if (countdownText != null)
                countdownText.text = count.ToString();

            yield return new WaitForSeconds(1f);
            count--;
        }

        if (countdownText != null)
            countdownText.text = "GO!";

        yield return new WaitForSeconds(0.5f);

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
            countdownBackground.gameObject.SetActive(false);
        }


        if (audioSource != null && backgroundMusic != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        gameStarted = true;
    }
}