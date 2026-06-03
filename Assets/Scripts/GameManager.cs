using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TextMeshProUGUI messageText;

    private bool gameEnded = false;
    public bool HasArtifact { get; private set; } = false;

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
    }

    private void Start()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    public void CollectArtifact()
    {
        HasArtifact = true;
        Debug.Log("Artifact collected!");
    }

    public void PlayerCaught()
    {
        if (gameEnded) return;

        gameEnded = true;
        ShowMessage("You were caught!");
        Invoke(nameof(RestartScene), 2f);
    }

    public void PlayerWon()
    {
        if (gameEnded) return;

        gameEnded = true;
        ShowMessage("You escaped!");
        Invoke(nameof(RestartScene), 2f);
    }

    private void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}