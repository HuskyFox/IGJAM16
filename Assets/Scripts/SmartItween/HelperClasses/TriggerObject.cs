using UnityEngine;

public class TriggerObject : MonoBehaviour {

    [HideInInspector]
    public bool IsActivated;

	public virtual void Activate()
    {
        IsActivated = true;
    }

	public virtual void Deactivate()
    {
        IsActivated = false;
    }
}
