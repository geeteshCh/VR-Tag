using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI roomName;
 
    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void OnRoomJoinOrCreate()
    {
        NetworkManager.Instance.StartSharedSession(roomName.text);
    }

    public void RefreshSessionList()
    {
        NetworkManager.Instance.RefereshSessionListUI();
    }
}
