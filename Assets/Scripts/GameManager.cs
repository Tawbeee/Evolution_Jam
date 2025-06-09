using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform spawnPosition;

    private bool isPaused = true; // Le jeu commence en pause à cause du startPanel
    private bool isEnded = false;

    void Awake()
    {
        // Singleton pattern simple
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Initialisation
        startPanel.SetActive(true);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (isEnded) return; // Bloque toute interaction si le jeu est fini

        if (startPanel.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                StartGame();
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                QuitGame();
            }
        }
    }

    void StartGame()
    {
        startPanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPaused = false;

        player.transform.position = spawnPosition.position;
        PlayerState.Instance.playerState = KillingObjectType.None;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Fonction publique à appeler pour finir le jeu
    public void End()
    {
        isEnded = true;
        isPaused = true;

        Time.timeScale = 0f;

        endPanel.SetActive(true);
        pausePanel.SetActive(false);
        startPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
