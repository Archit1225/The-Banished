using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    public GameObject iconPrefab;
    private IInteractable interactableInRange;
    private bool interButtonPressed;
    private bool IconSpawned;
    private GameObject iconPrefabInstance;
    private float posOffset = 0.05f;

    private void Start()
    {
        //disableGameObject
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Show that it can interact by eneabling the image
        if(collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            Debug.Log("Interaction Possible 1");
            interactableInRange = interactable;
            if (collision.CompareTag("NPC") && !IconSpawned)
            {
                SpriteRenderer NPCSprite = collision.GetComponent<SpriteRenderer>();
                Vector3 spriteSize = NPCSprite.size;
                Vector3 iconPos = new Vector3(NPCSprite.transform.position.x, NPCSprite.transform.position.y + (spriteSize.y / 2) + posOffset, 0);
                iconPrefabInstance = Instantiate(iconPrefab, iconPos, Quaternion.identity);
                IconSpawned = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Show that it can interact by eneabling the image
        if(collision.TryGetComponent(out IInteractable interactable) && interactable==interactableInRange)
        {
            interactableInRange = null;
            IconSpawned=false;
            if (iconPrefabInstance != null) { Destroy(iconPrefabInstance); }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Interaction Possible 3");
            interButtonPressed = true;
        }
    }

    private void Update()
    {
        if (interButtonPressed)
        {
            interactableInRange?.Interact();
            interButtonPressed = false;
        }
    }
}   
