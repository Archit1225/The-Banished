using Unity.Cinemachine;
using UnityEngine;

public class ActivateArena : MonoBehaviour
{
    public Arena arenaDetails;
    public CinemachineCamera cineCam;
    public Animator animDoor;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Check if the collider belongs to the Player
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            collision.transform.position += gameObject.GetComponent<Collider2D>().transform.up;
            animDoor.SetBool("Entered", true);
            cineCam.Lens.OrthographicSize = 6;
            hasTriggered = true;

            // 2. Find the Spawner in the scene
            Wave_Spawner spawner = Object.FindFirstObjectByType<Wave_Spawner>();

            if (spawner != null)
            {
                // 3. Hand over the data and start the battle
                spawner.InitializeArena(arenaDetails);
                StartCoroutine(spawner.ExecuteArena());
            }
            else
            {
                Debug.LogError("No Wave_Spawner found in the scene!");
            }

            // 4. Optional: Disable the trigger so it can't be hit twice
            // Or use this space to close a 'Fog Wall' or Door
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }
}

