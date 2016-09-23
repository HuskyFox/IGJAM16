using UnityEngine;
using System.Collections;
using NodeCanvas.StateMachines;

public class Sheep : MonoBehaviour
{
    public GameObject Particles;

    public delegate void NPSheepWasKilled(Player killer, Sheep npSheep);
    public static NPSheepWasKilled OnNpSheepWasKilled;


    void TakeDamage(object damageInflicter) {
		//GetComponent<NavMeshAgent> ().Stop ();
	    var particles = Instantiate(Particles);
	    if (particles) particles.transform.position = transform.position;

		gameObject.SetActive (false);
       // if (SoundManager.Instance)
		   // SoundManager.Instance.PlayWolfBiteFail ();
	    var player = (Player) damageInflicter;
	    if (!player) return;
        if(OnNpSheepWasKilled != null)
            OnNpSheepWasKilled.Invoke(player, this);
        
	    //	Destroy ( this.gameObject );
	}
}
