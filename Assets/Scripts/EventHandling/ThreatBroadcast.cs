using UnityEngine;
using System.Collections;

public class ThreatBroadcast : MonoBehaviour
{

    public float ThreatReach = 5f;  //The distance which the threat reaches

    public void BroadcastThreat()
    {
        EventRelay.RelayEvent(EventRelay.EventMessageType.Threat, this);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ThreatReach);
    }
}
