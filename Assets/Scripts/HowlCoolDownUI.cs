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

    void Awake()
    {
        _playerManger = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        if (_playerManger && _playerManger.players.Count > 0)
        {
            
            var currentWolf = _playerManger.players[_playerManger.currentWolfIndex];
            var howlAbility = currentWolf.GetComponent<HowlAbility>();
            UpdateExpBar(Mathf.Clamp(howlAbility.ElapsedTime, 0, howlAbility.CooldownTime), howlAbility.CooldownTime);
        }
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
