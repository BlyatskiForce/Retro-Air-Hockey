using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DisconnectManager : MonoBehaviourPunCallbacks
{
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        PhotonNetwork.Disconnect();

        FindObjectOfType<ProgressSceneLoader>().LoadScene("MenuScreen");
    }
}
