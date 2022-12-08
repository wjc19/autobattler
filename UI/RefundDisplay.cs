using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RefundDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI souls;
    Player player;

    private void Start() {
        player = FindObjectOfType<Player>();
        player.onSoulsChanged += UpdateSoulsText;
        UpdateSoulsText(player.souls);
    }

    private void OnDisable() {
        player.onSoulsChanged -= UpdateSoulsText;
    }

    public void UpdateSoulsText(int soulsCount)
    {
        souls.text = string.Format("{0}", soulsCount);
    }
}
