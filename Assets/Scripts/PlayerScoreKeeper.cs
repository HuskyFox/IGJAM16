﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking.NetworkSystem;

public class PlayerScoreKeeper : MonoBehaviour
{

    public int OwnerIndex;
    private int _currentScore = 0;
    private Text _text;
    private int oldScore;
    public int CurrentScore { get{ return _currentScore; } }
    private const int KillNPSheepPenalty = 20;
    private const int KillPlayerPoints = 20;
    private const int KilledByPlayerPenalty = 10;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void OnEnable()
    {
        Sheep.OnNpSheepWasKilled += NPSheepWasKilled;
        Player.OnPlayerKilled += PlayerWasKilled;
    }

    void OnDisable()
    {
        Sheep.OnNpSheepWasKilled -= NPSheepWasKilled;
        Player.OnPlayerKilled -= PlayerWasKilled;
    }

    void PlayerWasKilled(Player killer, Player victim)
    {
        int killerIndex = -1;
        int.TryParse(killer.playerIndex, out killerIndex);
        int victimIndex = -1;
        int.TryParse(killer.playerIndex, out victimIndex);

        if (killerIndex.Equals(OwnerIndex))
        {
            _currentScore += KillPlayerPoints;
        }else if (victimIndex.Equals(OwnerIndex))
        {
            _currentScore -= KilledByPlayerPenalty;
        }
        UpdateText();
    }

    void NPSheepWasKilled(Player killer, Sheep killedSheep)
    {
        int playerIndex = -1;
        int.TryParse(killer.playerIndex, out playerIndex);

        if (playerIndex.Equals(OwnerIndex))
        {
            _currentScore -= KillNPSheepPenalty;
            UpdateText();
        }
    }

    private void UpdateText()
    {
        oldScore = int.Parse(_text.text);
        _text.text = _currentScore.ToString();

        if (oldScore > _currentScore)
        {
            _text.color = Color.red;
            Invoke("RevertTextEffects", 3f);
            _text.transform.parent.gameObject.GetComponent<Animation>().Play();
        }

        if (oldScore < _currentScore)
        {
            _text.color = Color.green;
            Invoke("RevertTextEffects", 3f);
            _text.transform.parent.gameObject.GetComponent<Animation>().Play();
        }
    }

    private void RevertTextEffects()
    {
        _text.color = Color.white;
    }
}
