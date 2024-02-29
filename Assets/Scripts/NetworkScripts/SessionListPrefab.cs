using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionListPrefab : MonoBehaviour
{

    public TextMeshProUGUI sessionName;
    public TextMeshProUGUI playerCount;
    public Button joinButton;
  
    private void Start()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    public void JoinSession()
    {
        NetworkManager.Instance.StartSharedSession(sessionName.text);
    }


}
