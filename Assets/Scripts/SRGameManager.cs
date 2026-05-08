using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SRGameManager : MonoBehaviour
{
    public static SRGameManager Instance { get; private set; }

    [Header("Timer")]
    [SerializeField] private float gameDuration = 120f; // secondes
    private float timer;

    [Header("Score")]
    [SerializeField] private int score = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bestScoresText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private RawImage countdownBackground;

    private List<int> bestScores = new List<int>();

    private bool gameStarted = false;
    public bool IsGameStarted => gameStarted;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        timer = gameDuration;

        UpdateUI();
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

    public void AddScore(int value)
    {
        score += value;
    }

    public void EndGame()
    {
        AddToBestScores(score);
        timer = gameDuration;
        gameStarted = false;
        UpdateUI();
    }

    public void StartGame()
    {
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
            

        gameStarted = true;
    }
}