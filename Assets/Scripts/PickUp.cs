using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUp : MonoBehaviour, IInteractable
{
    [HideInInspector]public DropInteraction dropInteraction;
    public string dropId;

    private void Start()
    {
        dropInteraction = GameObject.FindGameObjectWithTag("GameController").GetComponent<DropInteraction>();
    }
    bool IInteractable.CanInteract()
    {
        return true;
    }

    void IInteractable.Interact()
    {
        //DropInteraction
        if (dropInteraction != null)
        {
            dropInteraction.TriggerDrop(dropId);
        }
        //Acknowledge that player has picked up
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName + " cleared!");
        //Destroy the gameobject
        Destroy(gameObject);
    }
}
