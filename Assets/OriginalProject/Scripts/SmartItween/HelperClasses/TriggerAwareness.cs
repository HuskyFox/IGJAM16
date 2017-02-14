using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of units/objects inside of a trigger. If one of the objects is deactivated while inside the trigger, it is removed from the list.
/// </summary>
public class TriggerAwareness : MonoBehaviour {

    //Public Variables
    //----------------------------------------------------------------------------------------------
    [System.NonSerialized]
    public List<GameObject> CollidingUnitsList;         //A list containing all the objects detected inside of the trigger (after tag filter)

    public LayerMask LayersDetected;

    public delegate void ObjectEnterTrigger(GameObject go);
    public event ObjectEnterTrigger OnObjectEnter;
    public delegate void ObjectExitTrigger(GameObject go);
    public event ObjectExitTrigger OnObjectExit;

    //EventCalls
    //----------------------------------------------------------------------------------------------
    void Awake()
    {

        CollidingUnitsList = new List<GameObject>();
    }

    void Update()
    {
        RemoveDeactivatedObjects();
    }

    void OnEnable()
    {
        CollidingUnitsList.Clear();
    }

    //Keep track of the objects entering the trigger
    void OnTriggerEnter(Collider other)
    {
        FilterIntoList(other.gameObject);
    }

    //Remove the objects who leave the trigger from the list of colliding objects
    void OnTriggerExit(Collider other)
    {
        FilterOutOfList(other.gameObject);
    }

    //----------------------------------------------------------------------------------------------

    private void FilterIntoList(GameObject go)
    {
        if ((LayersDetected.value & 1 << go.layer) != 0)
        {
            if (!CollidingUnitsList.Contains(go.gameObject))
            {
                CollidingUnitsList.Add(go.gameObject);
                if (OnObjectEnter != null)
                    OnObjectEnter(go);
            }
        }
    }

    private void FilterOutOfList(GameObject go)
    {
        if (CollidingUnitsList.Contains(go))
        {
            CollidingUnitsList.Remove(go);

            if (OnObjectExit != null)
                OnObjectExit(go);
        }
    }


    private void RemoveDeactivatedObjects()
    {
        List<GameObject> tempColObjects = CollidingUnitsList;
        foreach (GameObject colObject in tempColObjects)
        {
            if (!colObject || !colObject.activeInHierarchy)
            {
                tempColObjects.Remove(colObject);

                if (OnObjectExit != null)
                    OnObjectExit(colObject);

                break;
            }
        }
    }
}
