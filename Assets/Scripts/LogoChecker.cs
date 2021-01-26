using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LogoChecker : MonoBehaviourPunCallbacks
{
    Image image;

    public Sprite player1;

    public Sprite player2;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            image.sprite = player1;
        }else
        {
            image.sprite = player2;
        }
    }
}
