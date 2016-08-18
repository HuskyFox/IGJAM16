using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking.NetworkSystem;

public class PlayerScoreKeeper : MonoBehaviour
{

    public int OwnerIndex;
    private int _currentScore = 0;
    private Text _text;

    private const int KillNPSheepPenalty = 20;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void OnEnable()
    {
        Sheep.OnNpSheepWasKilled += NPSheepWasKilled;
    }

    void OnDisable()
    {
        Sheep.OnNpSheepWasKilled -= NPSheepWasKilled;
    }

    void NPSheepWasKilled(Player killer, Sheep killedSheep)
    {
        int playerIndex = -1;
        int.TryParse(killer.playerIndex, out playerIndex);
        Debug.Log("Player " + playerIndex + " killed a npsheep");
        if (playerIndex.Equals(OwnerIndex))
        {
            _currentScore -= KillNPSheepPenalty;
            UpdateText();
        }
    }

    private void UpdateText()
    {
        _text.text = _currentScore.ToString();
    }
}
