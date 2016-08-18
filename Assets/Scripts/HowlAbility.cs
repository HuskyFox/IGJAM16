using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

[RequireComponent(typeof(ThreatBroadcast))]
public class HowlAbility : MonoBehaviour
{
    public float CooldownTime = 5f;
    private float _elapsedTime = 0;

    private Player _owner;
    private ThreatBroadcast _threatBroadcast;

	// Use this for initialization
	void Awake ()
	{
	    _owner = GetComponent<Player>();
	    _threatBroadcast = GetComponent<ThreatBroadcast>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!_owner.isWolf) return;

		if (_owner.Device!=null && _owner.Device.Action2 && _elapsedTime > CooldownTime)
	    {
			SoundManager.instance.PlayWolfHowl ();
			Invoke ("SheepReactionToHowl", 1.0f);
	        _elapsedTime = 0;
	        _threatBroadcast.BroadcastThreat();
	    }
	    _elapsedTime += Time.deltaTime;
	}

	void SheepReactionToHowl()
	{
		SoundManager.instance.PlaySheepReactionToHowl ();
	}
}
