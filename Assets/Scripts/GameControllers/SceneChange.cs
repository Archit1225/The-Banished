using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : StateMachineBehaviour
{
    public string sceneName;
    //override public void OnStateExit()
    //{
    //    SceneManager.LoadScene(sceneName);
    //}
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

}
