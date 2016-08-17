using UnityEngine;
using System.Collections;

public class EventRelay : MonoBehaviour {

	public delegate string EventAction(EventMessageType type, MonoBehaviour sender);
	public static event EventAction OnEventAction;

	public enum EventMessageType {
		WolfHowled
	}

	public static string RelayEvent(EventMessageType messageType, MonoBehaviour sender) {
		return OnEventAction(messageType, sender);
	}
}
