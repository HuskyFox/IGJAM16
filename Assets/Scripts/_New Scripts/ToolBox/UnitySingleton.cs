using UnityEngine;
using System.Collections;


//A script that defines a class for Singletons
//When you want to create a Manager Singleton, pass in Manager as the T Component

public class UnitySingleton <T> : MonoBehaviour
	where T : Component
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<T> ();
				if (instance == null)
				{
					GameObject obj = new GameObject ();
					obj.hideFlags = HideFlags.HideAndDontSave;
					instance = obj.AddComponent<T> ();
				}
			}
			return instance;
		}
	}

	public virtual void Awake ()
	{
		DontDestroyOnLoad (this.gameObject);
		if (instance == null)
			instance = this as T;
		else
			Destroy (gameObject);
	}
}
