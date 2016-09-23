using System;
using UnityEngine;
using System.Collections;
using NodeCanvas.BehaviourTrees;
using ObjectPooling;
//using XInputDotNetPure;

public class ShapeshiftAbility : MonoBehaviour
{
    public GameObject Particles;
    public float TimeSpentAsAWolf = 1f;

    private GameObject _wolf;
    private GameObject _sheep;
    private Player _owner;
    public enum State
    {
        Wolf,
        Sheep,
    }

    private State _currentState;
    public State CurrentState {
        get { return _currentState; }
    }

    private bool runningCoorutine = false;
    void OnEnable()
    {
        Sheep.OnNpSheepWasKilled += PlayerKilledSheep;
        Player.OnPlayerKilled += PlayerKilledPlayer;
    }

    void OnDisable()
    {
        Sheep.OnNpSheepWasKilled -= PlayerKilledSheep;
        Player.OnPlayerKilled -= PlayerKilledPlayer;
    }

    void PlayerKilledSheep(Player killer, Sheep npSheep)
    {
        if (_owner.playerIndex.Equals(killer.playerIndex))
        {
            SetState(State.Wolf);
            if (!runningCoorutine)
                StartCoroutine(ResetToSheep());
        }
    }

    void PlayerKilledPlayer(Player killer, Player victim)
    {
        if (_owner.playerIndex.Equals(killer.playerIndex))
        {
            SetState(State.Wolf);
            if (!runningCoorutine)
                StartCoroutine(ResetToSheep());
        }
    }

    // Use this for initialization
    void Awake ()
	{
        var wolfChildObject = transform.FindChild("Wolf");
	    var sheepChildObject = transform.FindChild("Sheep");
	    _owner = GetComponent<Player>();
	    if (wolfChildObject && sheepChildObject)
	    {
	        _wolf = wolfChildObject.gameObject;
	        _sheep = sheepChildObject.gameObject;
            
	    }

        SetState(State.Sheep);
    }

    IEnumerator ResetToSheep()
    {
        runningCoorutine = true;
        yield return new WaitForSeconds(TimeSpentAsAWolf);
        SetState(State.Sheep);
        runningCoorutine = false;
    }

    public void SetState(State newState)
    {
        
        if (!_wolf || !_sheep) return;
        _currentState = newState;
        switch (newState)
        {
            case State.Wolf:
                _wolf.SetActive(true);
                _sheep.SetActive(false);
                break;
            case State.Sheep:
                _sheep.SetActive(true);
                _wolf.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException("newState", newState, null);
        }
        var particles = Instantiate(Particles);
        particles.transform.position = transform.position;
        particles.SetActive(true);
    }
}
