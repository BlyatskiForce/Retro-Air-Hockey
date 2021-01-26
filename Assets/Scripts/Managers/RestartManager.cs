using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;


/// <summary>
/// Adds Restart functionality for buttons with photon.
/// </summary>
public class RestartManager : MonoBehaviourPunCallbacks
{
    public const string PlayerOneWantsToRestart = "PlayerOneWantsToRestart";
    public const string PlayerTwoWantsToRestart = "PlayerTwoWantsToRestart";

    public bool wantToRestart = false;

    [SerializeField]
    TextMeshProUGUI text;

    // Sets button and button text Color to specified color.
    [SerializeField]
    Color32 color;

    public void QueueRestart()
    {
        if(!wantToRestart && !GameManager.isTesting)
        {
            wantToRestart = true;

            object InfoFromProps;

            if (PhotonNetwork.IsMasterClient)
            {
                Hashtable props = new Hashtable
                    {
                        {PlayerOneWantsToRestart, true}
                    };

                PhotonNetwork.CurrentRoom.SetCustomProperties(props);

                // Checks if Player two wants to restart as well.
                if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(PlayerTwoWantsToRestart, out InfoFromProps))
                {
                    bool otherPlayerWantstoRestartAsWell = (bool)InfoFromProps;
                    if (otherPlayerWantstoRestartAsWell)
                    {
                        FindObjectOfType<ProgressSceneLoader>().LoadScene("GameScene");
                    }
                }
            }
            else
            {
                Hashtable props = new Hashtable
                    {
                        {PlayerTwoWantsToRestart, true}
                    };

                PhotonNetwork.CurrentRoom.SetCustomProperties(props);

                // Checks if Player one wants to restart as well.
                if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(PlayerOneWantsToRestart, out InfoFromProps))
                {
                    bool otherPlayerWantstoRestartAsWell = (bool)InfoFromProps;
                    if (otherPlayerWantstoRestartAsWell)
                    {
                        FindObjectOfType<ProgressSceneLoader>().LoadScene("GameScene");
                    }
                }
            }

            text.color = color;
            text.text = "Waiting for Other Player...";
        }

    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        object InfoFromProps;

        if (wantToRestart)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // Checks if Player one wants to restart as well.
                if (propertiesThatChanged.TryGetValue(PlayerTwoWantsToRestart, out InfoFromProps))
                {
                    bool otherPlayerWantstoRestartAsWell = (bool)InfoFromProps;
                    if (otherPlayerWantstoRestartAsWell)
                    {
                        FindObjectOfType<ProgressSceneLoader>().LoadScene("GameScene");
                    }
                }
            }
            else
            {
                // Checks if Player two wants to restart as well.
                if (propertiesThatChanged.TryGetValue(PlayerOneWantsToRestart, out InfoFromProps))
                {
                    bool otherPlayerWantstoRestartAsWell = (bool)InfoFromProps;
                    if (otherPlayerWantstoRestartAsWell)
                    {
                        FindObjectOfType<ProgressSceneLoader>().LoadScene("GameScene");
                    }
                }
            }
        }



    }


}
