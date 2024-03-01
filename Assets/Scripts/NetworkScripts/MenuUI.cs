using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI roomName;
    
    
    public void OnUserNameEntered(string val)
    {
        PlayerPrefs.SetString("username", val);
    }
    public void OnRoomJoinOrCreate()
    {
        if (roomName.text == "")
        {
            roomName.text = "dummy";
        }
        NetworkManager.Instance.StartSharedSession(roomName.text);
    }

    public void RefreshSessionList()
    {
        NetworkManager.Instance.RefereshSessionListUI();
    }
}
