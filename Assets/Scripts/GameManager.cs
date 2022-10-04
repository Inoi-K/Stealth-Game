using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameLoseUI;
    public GameObject gameWinUI;
    bool gameIsOver;

    private void Start() {
        Guard.OnGuardHasSpottedPlayer += ShowGameLoseUI;
        FindObjectOfType<Player>().OnReachedEndOfLevel += ShowGameWinUI;
    }

    private void Update() {
        if (gameIsOver)
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene(0);
    }

    void ShowGameWinUI() {
        OnGameOver(gameWinUI);
    }

    void ShowGameLoseUI() {
        OnGameOver(gameLoseUI);
    }

    void OnGameOver(GameObject gameOverUI) {
        gameOverUI.SetActive(true);
        gameIsOver = true;
        Guard.OnGuardHasSpottedPlayer -= ShowGameLoseUI;
        FindObjectOfType<Player>().OnReachedEndOfLevel -= ShowGameWinUI;
    }
}
