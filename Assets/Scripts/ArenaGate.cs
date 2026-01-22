using UnityEngine;
using UnityEngine.SceneManagement;
public class ArenaGate : MonoBehaviour
{
    public string nextScene;
    public int sceneNum;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextScene);
            AudioManager.instance.SceneMusic(sceneNum);
        }
    }
}
