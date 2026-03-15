using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossArenaCollider1 : MonoBehaviour
{
    public GameObject boss;
    public UnityEngine.Playables.PlayableDirector preBossCutscene;
    public CinemachineCamera cineCam;
    public Collider2D mapBoundary2;
    public Light2D globalLight;
    public GameObject aoESpawner;
    public Vector3 finalVector = new Vector3(-100, 69, 0);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position += Vector3.up;
            gameObject.GetComponent<Collider2D>().isTrigger = false;
            boss.SetActive(true);
            cineCam.GetComponent<CinemachineConfiner2D>().BoundingShape2D = mapBoundary2;
            cineCam.GetComponent<CinemachineConfiner2D>().InvalidateLensCache();
            globalLight.intensity = 0.8f;
            //Camera on boss
            //Pause the game
            StartCoroutine(PlayCutscene());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.GetComponent<Collider2D>().isTrigger = false;
        }
    }
    IEnumerator PlayCutscene()
    {
        preBossCutscene.Play();
        Debug.Log("Is this Started??");
        yield return new WaitUntil(CheckIfCutsceneIsOver);
        Debug.Log("Is this Over??");
        cineCam.Lens.OrthographicSize = 8;
        boss.GetComponent<Enemy_Controller>().enabled = true;
        aoESpawner.SetActive(true);
    }

    bool CheckIfCutsceneIsOver()
    {
        return preBossCutscene.state != UnityEngine.Playables.PlayState.Playing;
    }
}
