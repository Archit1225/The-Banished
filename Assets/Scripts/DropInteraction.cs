using System.Collections.Generic;
using UnityEngine;

public class DropInteraction : MonoBehaviour
{
    [System.Serializable]
    public class DropAction
    {
        public string dropID;

        public List<GameObject> objectsToActivate;
        public List<GameObject> objectsToDeactivate;

        public Animator animator;
        public List<string> animatorBoolsToEnable;
        public List<string> animatorBoolsToDisable;

        public List<Collider2D> collidersToTRIGGER;
        public List<Collider2D> collidersToUNTRIGGER;
    }

    public List<DropAction> dropActions = new List<DropAction>();

    public void TriggerDrop(string dropID)
    {
        DropAction action = dropActions.Find(x => x.dropID == dropID);

        if (action == null)
        {
            Debug.LogWarning("Drop ID not found: " + dropID);
            return;
        }

        foreach (GameObject obj in action.objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        foreach (GameObject obj in action.objectsToDeactivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        foreach (Collider2D coll in action.collidersToTRIGGER)
        {
            if (coll != null)
                coll.isTrigger = true;
        }

        foreach (Collider2D coll in action.collidersToUNTRIGGER)
        {
            if (coll != null)
                coll.isTrigger = false;
        }

        if (action.animator != null)
        {
            foreach (string boolName in action.animatorBoolsToEnable)
                action.animator.SetBool(boolName, true);

            foreach (string boolName in action.animatorBoolsToDisable)
                action.animator.SetBool(boolName, false);
        }
    }
}