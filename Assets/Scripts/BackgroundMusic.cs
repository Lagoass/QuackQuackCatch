using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance { get; private set; }

    public AudioSource backgroundMusic;
    // audio source para botao de start game no main menu
    public AudioSource startGameSound;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (backgroundMusic == null)
        {
            backgroundMusic = GetComponent<AudioSource>();
        }

        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true; // garante que reinicia automaticamente
            if (!backgroundMusic.isPlaying)
            {
                backgroundMusic.Play();
            }
        }
        else
        {
            Debug.LogWarning("BackgroundMusic: Nenhum AudioSource atribuído ou encontrado no GameObject.");
        }
    }

    // funcao publica para tocar o som do botao de start game
    public void PlayStartGameSound()
    {
        if (startGameSound != null)
        {
            startGameSound.PlayOneShot(startGameSound.clip);
        }
    }
}
