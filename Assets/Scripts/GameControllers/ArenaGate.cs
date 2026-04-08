using Unity.VisualScripting;
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
            AudioManager.instance.PlaySceneMusic(sceneNum);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Some mysterious force is not allowing to pass through....");
        }
    }
}
