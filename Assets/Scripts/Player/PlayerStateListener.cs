using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateListener : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerMovement playerMovement;
    //public Rigidbody2D rb;

    private void Start()
    {
        if (GameStateManager.instance != null)
        {
            HandleStateChange(GameStateManager.instance.currentState);
        }
    }
    private void OnEnable()
    {
        if (GameStateManager.instance!=null)
        {
            GameStateManager.instance.OnStateChanged += HandleStateChange;
        }
    }
    private void OnDisable()
    {
        if (GameStateManager.instance!=null)
        {
            GameStateManager.instance.OnStateChanged -= HandleStateChange;
        }
    }

    private void HandleStateChange(GameStateManager.GameState newState)
    {
        if (newState == GameStateManager.GameState.Gameplay)
        {
            playerInput.SwitchCurrentActionMap("Player");
            playerMovement.enabled = true; 
            Debug.Log("State is Dialogue. Current Map is now: " + playerInput.currentActionMap.name);
        }
        else if (newState == GameStateManager.GameState.Cutscene || newState == GameStateManager.GameState.Dialogue)
        {
            playerInput.SwitchCurrentActionMap("UI");
            playerMovement.ClearInputs();
            
            playerMovement.enabled = false;
            Debug.Log("State is Dialogue. Current Map is now: " + playerInput.currentActionMap.name);
        }
    }
}
