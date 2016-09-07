using UnityEngine;
using System.Collections;

public class KillManager : UnitySingleton <KillManager>
{
	public delegate void PlayerWasKilled (PlayerController killer, PlayerController victim);
	public static event PlayerWasKilled OnPlayerWasKilled;

	public delegate void NPSheepWasKilled (PlayerController killer, Sheep Victim);
	public static event NPSheepWasKilled OnNPSheepWasKilled;
}
