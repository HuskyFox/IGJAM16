using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TriggerAwareness))]
public class GenericTriggerVolume : MonoBehaviour
{
    public UnityEvent TriggerEnter;
    public UnityEvent TriggerExit;
    private TriggerAwareness _triggerAwareness;

	// Use this for initialization
	void Awake ()
	{
	    _triggerAwareness = GetComponent<TriggerAwareness>();
	}

    void OnEnable()
    {
        _triggerAwareness.OnObjectEnter += ObjectEnter;
        _triggerAwareness.OnObjectExit += ObjectExit;

    }

    void OnDisable()
    {
        _triggerAwareness.OnObjectEnter -= ObjectEnter;
        _triggerAwareness.OnObjectExit -= ObjectExit;
    }

    private void ObjectEnter(GameObject go)
    {
        if (TriggerEnter != null)
            TriggerEnter.Invoke();
    }

    private void ObjectExit(GameObject go)
    {
        if (TriggerExit != null)
            TriggerExit.Invoke();
    }
}
