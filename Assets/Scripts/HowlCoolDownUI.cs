using UnityEngine;
using System.Collections;

public class HowlCoolDownUI : MonoBehaviour
{
    [Range(0, 100)]
    public float Value;

    public float TopFillPadding;
    public float BottomStartPadding;
    public GameObject barFillArea;
    public GameObject barFillImage;

    private PlayerManager _playerManger;
    private WolfManager _wolfManager;

    private Player[] _players;

    private int wolfIndex;
    private GameObject currentWolf;

    void Awake()
    {
        _playerManger = FindObjectOfType<PlayerManager>();
        _wolfManager = FindObjectOfType<WolfManager>();
        
    }
    void Start()
    {
        _players = FindObjectsOfType<Player>();
        wolfIndex = _wolfManager.currentWolfIndex;
        currentWolf = GameObject.Find("Player_" + wolfIndex);
    }

    private Player GetPlayerWithIndex(int index)
    {
        foreach (var player in _players)
        {
            Debug.Log("Player index " + player.playerIndex + " wolf index " + index);
            if (player.playerIndex.Equals(index))
                return player;
        }
        return null;
    }

    void Update()
    {
        //      Player currentWolf = GetPlayerWithIndex(_wolfManager.currentWolfIndex);
        if(wolfIndex!= _wolfManager.currentWolfIndex)
        {
            currentWolf = GameObject.Find("Player_" + _wolfManager.currentWolfIndex);
            wolfIndex = _wolfManager.currentWolfIndex;
        }
        if (!currentWolf) return;
        Debug.Log("current wolf is not null");
        var howlAbility = currentWolf.GetComponent<HowlAbility>();
        UpdateExpBar(Mathf.Clamp(howlAbility.ElapsedTime, 0, howlAbility.CooldownTime), howlAbility.CooldownTime);
    }

    public void UpdateExpBar(float currentVal, float maxVal)
    {
        var percentFilled = (currentVal / maxVal) * 100;
        var barHeight = barFillArea.GetComponent<RectTransform>().rect.height + TopFillPadding;
        RectTransform fillRect = barFillImage.GetComponent<RectTransform>();
        Vector2 tempPos = fillRect.anchoredPosition;
        tempPos.y = BottomStartPadding + ((barHeight / 100) * percentFilled);
        print("Y" + percentFilled);

        fillRect.anchoredPosition = tempPos;
    }
}

