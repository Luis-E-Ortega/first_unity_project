using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel; // Added panel for game win
    public Button restartButton;
    public static GameManager Instance {get; private set;}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }   
        restartButton.onClick.AddListener(RestartGame);
    }
    void Start()
    {
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false); 
        Time.timeScale = 1; // To ensure game starts unpaused
    }
    void Update()
    {
        if ((gameOverPanel.activeSelf || gameWinPanel.activeSelf) && Input.GetKeyDown(KeyCode.Y)) // Changed this to add or statement
        {
            RestartGame();
        }
    }

    public void ShowGameOver()
    {
        PauseGame();
        Debug.Log("ShowGameOver called");
        gameOverPanel.SetActive(true);

        SetUpRestartButton();
    }
    public void ShowGameWin()
    {
        PauseGame();
        Debug.Log("ShowGameWin called");
        gameWinPanel.SetActive(true);
        
        SetUpRestartButton();
    }
    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void SetUpRestartButton()
    {
        restartButton.enabled = false;
        restartButton.enabled = true;
        restartButton.interactable = true;
        EventSystem.current.SetSelectedGameObject(restartButton.gameObject);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame called");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
