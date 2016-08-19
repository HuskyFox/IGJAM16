using UnityEngine;
using UnityEngine.Events;

public class InputCheck : MonoBehaviour
{

    public KeyCode KeyToDetect;

    public UnityEvent OnKeyWasPressed;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyToDetect) && OnKeyWasPressed != null)
            OnKeyWasPressed.Invoke();
    }
}
