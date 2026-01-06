using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange;
    private bool interButtonPressed;

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
            //Image visible
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Show that it can interact by eneabling the image
        if(collision.TryGetComponent(out IInteractable interactable) && interactable==interactableInRange)
        {
            interactableInRange = null;
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
