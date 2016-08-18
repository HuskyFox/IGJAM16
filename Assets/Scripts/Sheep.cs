using UnityEngine;
using System.Collections;
using NodeCanvas.StateMachines;

public class Sheep : MonoBehaviour
{
    public GameObject Particles;

    public delegate void NPSheepWasKilled(Player killer, Sheep npSheep);
    public static NPSheepWasKilled OnNpSheepWasKilled;

    //void OnEnable()
    //{
    //    Sheep.OnNpSheepWasKilled += PauseBehaviour;
    //}

    //void OnDisable()
    //{
    //    Sheep.OnNpSheepWasKilled -= PauseBehaviour;
    //}

    private void PauseBehaviour(Player killer, Sheep npSheep)
    {
        var fsm = GetComponent<FSMOwner>();
        if (!fsm) return;
        fsm.PauseBehaviour();
    }



	void TakeDamage(object damageInflicter) {
		//GetComponent<NavMeshAgent> ().Stop ();
	    var particles = Instantiate(Particles);
	    if (particles) particles.transform.position = transform.position;

		gameObject.SetActive (false);
        if (SoundManager.instance)
		    SoundManager.instance.PlayWolfBiteFail ();
	    var player = (Player) damageInflicter;
	    if (!player) return;
        if(OnNpSheepWasKilled != null)
            OnNpSheepWasKilled.Invoke(player, this);
        
	    //	Destroy ( this.gameObject );
	}
}
