using UnityEngine;
using UnityEngine.Playables;

public class AutoCutsceneState : MonoBehaviour
{
    private PlayableDirector director;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        director.played += OnCutsceneStarted;
        director.stopped += OnCutsceneEnded;
    }
    private void OnDisable()
    {
        director.played -= OnCutsceneStarted;
        director.stopped -= OnCutsceneEnded;
    }

    private void OnCutsceneStarted(PlayableDirector obj)
    {
        if(GameStateManager.instance != null)
        {
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Cutscene);
        }
    }
    private void OnCutsceneEnded(PlayableDirector obj)
    {
        if (GameStateManager.instance != null)
        {
            GameStateManager.instance.ChangeState(GameStateManager.GameState.Gameplay);
        }
    }
   
}
