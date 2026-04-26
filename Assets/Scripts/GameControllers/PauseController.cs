using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public static PauseController instance;
    public PlayerInput playerInput;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        // Ensure every time a scene starts, the game is unpaused
        UnPause();
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        playerInput.SwitchCurrentActionMap("UI");
        Debug.Log("UI action map");
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        playerInput.SwitchCurrentActionMap("Player");
        Debug.Log("Player action map");
    }
}
