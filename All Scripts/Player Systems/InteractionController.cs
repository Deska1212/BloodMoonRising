using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for casting a ray out and dealing with selection logic
/// </summary>
public class InteractionController : MonoBehaviour
{
    [SerializeField] private IInteractable selected;
    [SerializeField] private float selectDistance;

    public static Action OnSelect;
    public static Action OnDeselect;


    // Update is called once per frame
    void Update()
    {
        //  Deselect whatever is currently in selected slot
        if (selected != null)
        {
            selected.Deselect();
            OnDeselect?.Invoke();
            selected = null;
        }
        
        // Cast a ray if it hits an object with selectable tag, select it
        RaycastHit hit;
        // TODO: Cache main camera
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, selectDistance))
        {
            var selectedTransform = hit.transform;
            if (selectedTransform.CompareTag("Selectable"))
            {
                /*
                 * I was going to use dot product here to make selection a bit easier
                 * However this was deemed slightly out of scope and unnecessary:
                 * We wont have many selectable object, and they are large objects with big colliders
                 * so they won't really benefit from such a system.
                 */
                selected = selectedTransform.GetComponent<IInteractable>(); // Expensive call
                OnSelect?.Invoke();
            }
        }

        if (selected != null)
        {
            selected.Select();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteract();
        }

    }

    public void OnInteract()
    {
        // We have hit 'E', check if we have a selected object and interact with it
        if (selected != null)
        {
            selected.Interact();
            selected = null;
            OnDeselect?.Invoke();
        }
    }
}
