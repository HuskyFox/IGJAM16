using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Detects objects that enter to the trigger collider and activates/de-activates attached interactable objects. 
/// </summary>

[RequireComponent(typeof(TriggerAwareness))]
public class SmartItweenTrigger : MonoBehaviour
{

    private TriggerAwareness _triggerAwareness;

    public TriggerObject[] ObjectsToActivate;
    public bool DeactivateObjectsOnExit = true;

    void OnEnable()
    {
        GetComponent<TriggerAwareness>().OnObjectEnter += OnObjectEnter;
        GetComponent<TriggerAwareness>().OnObjectExit += OnObjectExit;
    }

    void OnDisable()
    {
        GetComponent<TriggerAwareness>().OnObjectEnter -= OnObjectEnter;
        GetComponent<TriggerAwareness>().OnObjectExit -= OnObjectExit;
    }

    void Start()
    {
        _triggerAwareness = GetComponent<TriggerAwareness>();
    }

    void OnObjectEnter(GameObject go)
    {
        if (_triggerAwareness.CollidingUnitsList.Count == 1)
        {
            Debug.Log("Activate Trigger");
            ActivateInteractables();
        }
    }
    void OnObjectExit(GameObject go)
    {
        if (_triggerAwareness.CollidingUnitsList.Count == 0 && DeactivateObjectsOnExit)
        {
            Debug.Log("Deactivate Trigger");
            DeactivateInteractables();
        }
    }

    private void ActivateInteractables()
    {
        foreach (TriggerObject tObject in ObjectsToActivate)
        {
            if (!tObject)
                return;
            if (!tObject.IsActivated)
            {
                tObject.Activate();
            }
        }
    }

    private void DeactivateInteractables()
    {
        foreach (TriggerObject tObject in ObjectsToActivate)
        {
            if (!tObject)
                return;
            if (tObject.IsActivated)
            {
                tObject.Deactivate();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (ObjectsToActivate == null)
            return;

        Gizmos.color = Color.blue;

        foreach (TriggerObject tObject in ObjectsToActivate)
        {
            if (!tObject)
                return;
            Gizmos.DrawLine(this.transform.position, tObject.transform.position);
            if (tObject.GetComponent<MeshFilter>() == null)
                return;
            else
                Gizmos.DrawWireMesh(tObject.GetComponent<MeshFilter>().sharedMesh, -1, tObject.transform.position, tObject.transform.rotation, tObject.transform.lossyScale);
        }
    }
}