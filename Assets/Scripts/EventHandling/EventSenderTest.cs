using UnityEngine;
using System.Collections;

public class EventSenderTest : MonoBehaviour {

	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.K))
	    {
	        EventRelay.RelayEvent(EventRelay.EventMessageType.WolfHowled, this);
	    }
	}

}
