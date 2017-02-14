using UnityEngine;
using System.Collections.Generic;
using NodeCanvas.StateMachines;

public class SheepEventListener : MonoBehaviour {

	public List<EventRelay.EventMessageType> EventsHandled =
		new List<EventRelay.EventMessageType>();

    private FSMOwner _sheepFSM;

    void Awake()
    {
        _sheepFSM = GetComponent<FSMOwner>();
    }

	void OnEnable() {
		EventRelay.OnEventAction += HandleEvent;
	}

	void OnDisable() {
		EventRelay.OnEventAction -= HandleEvent;
	}

	//This method matches the signature of:
	//public delegate string EventAction(EventMessageType type, MonoBehaviour sender);
	//This means we can add it to the OnEventAction

	string HandleEvent(EventRelay.EventMessageType messageType, MonoBehaviour sender) {
		if(EventsHandled.Contains(messageType)) {
		    switch (messageType)
		    {
		            case EventRelay.EventMessageType.Threat:
		                var threat = (ThreatBroadcast) sender;
		                if (threat)
		                {
		                    var distanceToHowlSource = Vector3.Distance(transform.position, threat.transform.position);
		                    if (distanceToHowlSource < threat.ThreatReach)
		                        _sheepFSM.blackboard.SetValue("Threat", threat.gameObject.transform);
		                }
                            
		            break;
		    }
		return this.ToString();
		} else {
			//ignore event
			return this.ToString();
		}
	}
}
