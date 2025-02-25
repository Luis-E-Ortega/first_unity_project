using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
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
    }
    void Update()
    {
        if (gameOverPanel.activeSelf && Input.GetKeyDown(KeyCode.Y))
        {
            RestartGame();
        }
    }

    public void ShowGameOver()
    {
        //Time.timeScale = 0;
        Debug.Log("ShowGameOver called");
        gameOverPanel.SetActive(true);

        restartButton.enabled = false;
        restartButton.enabled = true;
        restartButton.interactable = true;
        EventSystem.current.SetSelectedGameObject(restartButton.gameObject);
        EventSystem.current.SetSelectedGameObject(null);
        
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame called");
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
