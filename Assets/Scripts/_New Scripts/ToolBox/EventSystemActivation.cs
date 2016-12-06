using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// This script handles the EventSystem assignation (there was some problems when moving it from scene to scene)
public class EventSystemActivation : MonoBehaviour
{
	[SerializeField]private GameObject _buttonToSelect;

	void Start()
	{
		EventSystem.current.SetSelectedGameObject (_buttonToSelect);
	}

	void OnEnable()
	{
		if (EventSystem.current)
			StartCoroutine (ChangeSelectedButton ());
	}

	void OnDisable()
	{
		if (EventSystem.current)
			EventSystem.current.SetSelectedGameObject (null);
	}

	IEnumerator ChangeSelectedButton()
	{
		//setting it to null and waiting for the next frame solves occasional non-assignation problems...
		EventSystem.current.SetSelectedGameObject (null);
		yield return new WaitForEndOfFrame ();
		EventSystem.current.SetSelectedGameObject (_buttonToSelect);
	}
}