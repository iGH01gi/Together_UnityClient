using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInRoom : UI_subitem
{
    private RoomPlayer thisPlayer;

    public void Init(RoomPlayer player)
    {
        thisPlayer = player;
        transform.GetChild(0).GetComponent<TMP_Text>().text = player.Name;
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
