using UnityEngine;

[RequireComponent(typeof(ThreatBroadcast))]
public class HowlAbility : MonoBehaviour
{
    public float CooldownTime = 5f;
    private float _elapsedTime = 0;

    public GameObject Particles;
    public float ParticleYOffset = 0;

    public float ElapsedTime {
        get { return _elapsedTime; }
    }

    private Player _owner;
    private ThreatBroadcast _threatBroadcast;

	// Use this for initialization
	void Awake ()
	{
	    _elapsedTime = CooldownTime;
	    _owner = GetComponent<Player>();
	    _threatBroadcast = GetComponent<ThreatBroadcast>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!_owner.isWolf) return;

		if (_owner.Device!=null && _owner.Device.Action2 && _elapsedTime > CooldownTime)
	    {
				//SoundManager.Instance.PlayWolfHowl ();
				//SoundManager.Instance.PlaySheepReactionToHowl ();

	        if (Particles)
	        {
	            var pos = transform.position;
	            pos.y += ParticleYOffset;
	            Instantiate(Particles).transform.position = transform.position;
	        }
	        _elapsedTime = 0;
	        _threatBroadcast.BroadcastThreat();
	    }
	    _elapsedTime += Time.deltaTime;
	}
}
