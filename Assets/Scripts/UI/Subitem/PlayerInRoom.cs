using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInRoom : UI_subitem
{
    private Player thisPlayer;

    public void Init(Player player)
    {
        thisPlayer = player;
        transform.GetChild(0).GetComponent<TMP_Text>().text = player.Name;
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
