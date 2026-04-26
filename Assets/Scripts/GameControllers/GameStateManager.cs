using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    public event Action<GameState> OnStateChanged;
    public enum GameState { Gameplay, Cutscene, Dialogue}
    public GameState currentState {  get; private set; } 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        OnStateChanged?.Invoke(currentState);
    }

}
