using UnityEngine;
using TMPro;
public class Stopwatch : MonoBehaviour
{   
    [Header("UI")]
    public TextMeshProUGUI timerText;

    [Header("Behavior")]
    [SerializeField] private bool autoStart = true;
    [SerializeField] private PlayerMovement player; // opcional: arraste o Player aqui
    [SerializeField] private string playerTag = "Player"; // alternativa: buscar por tag
    public TextMeshProUGUI timeEnd;

    private float timeElapsed;
    private bool isRunning;
    private float findPlayerRetry;

    void Start()
    {
        // Cacheia PlayerMovement se não foi atribuído no Inspector
        if (player == null)
            player = GetComponent<PlayerMovement>();
        if (player == null)
            TryFindPlayerByTag();

        // Inicializa UI se existir, mas não bloqueia o timer se estiver nula
        if (timerText != null)
            timerText.text = GetFormattedTime();

        if (autoStart)
            StartTimer();
    }

    void Update()
    {   
        // Tenta re-obter o player por tag, caso ainda não esteja setado (ex.: spawn tardio)
        if (player == null)
        {
            findPlayerRetry -= Time.deltaTime;
            if (findPlayerRetry <= 0f)
            {
                TryFindPlayerByTag();
                findPlayerRetry = 0.5f;
            }
        }

        // Para automaticamente se tiver player e ele morrer
        if (player != null && !player.IsAlive())
        {
            StopTimer();
            // Atualiza a UI de fim de jogo
            if (timeEnd != null)
                timeEnd.text = GetFormattedTime();
        }

        if (isRunning)
        {
            timeElapsed += Time.deltaTime;
            if (timerText != null)
                timerText.text = GetFormattedTime();
        }
    }

    public void StartTimer() => isRunning = true;
    public void StopTimer() => isRunning = false;

    public void ResetTimer()
    {
        isRunning = false;
        timeElapsed = 0f;
        if (timerText != null)
            timerText.text = GetFormattedTime();
    }

    private string GetFormattedTime()
    {
        float totalSeconds = Mathf.Floor(timeElapsed);
        int minutes = Mathf.FloorToInt(totalSeconds / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void TryFindPlayerByTag()
    {
        if (string.IsNullOrEmpty(playerTag)) return;
        var go = GameObject.FindGameObjectWithTag(playerTag);
        if (go != null)
            player = go.GetComponent<PlayerMovement>();
    }
}
