using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EventSystemActivation : MonoBehaviour
{
	[SerializeField]GameObject buttonToSelect;

	void Start()
	{
		EventSystem.current.SetSelectedGameObject (buttonToSelect);
	}

	void OnEnable()
	{
		if (EventSystem.current)
			StartCoroutine (ChangeSelectedButton ());
			//EventSystem.current.SetSelectedGameObject (buttonToSelect);
	}

	void OnDisable()
	{
			EventSystem.current.SetSelectedGameObject (null);
	}

	IEnumerator ChangeSelectedButton()
	{
		EventSystem.current.SetSelectedGameObject (null);
		yield return new WaitForEndOfFrame ();
		EventSystem.current.SetSelectedGameObject (buttonToSelect);
	}
}