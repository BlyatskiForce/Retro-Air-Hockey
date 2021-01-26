using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Manages which room you are connecting to.
/// </summary>
public class LobbyManager : MonoBehaviourPunCallbacks
{
    private const int MAX_PLAYERS = 2;

    private bool isQueuing = false;

    #region UNITY
    private void Start()
    {
        // Makes sure the user is disconnected.
        DeQueue();
        Queue();
    }
    #endregion

    #region PHOTON
    private void ConnectToMaster()
    {
        //Debug.Log("Connecting to Master.");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        Photon.Realtime.RoomOptions roomOptions = new RoomOptions();
        roomOptions.PlayerTtl = 60000; // 60 sec
        roomOptions.EmptyRoomTtl = 1000; // 1 sec
        roomOptions.MaxPlayers = MAX_PLAYERS;

        PhotonNetwork.JoinOrCreateRoom("SPC 99", roomOptions, null);
    }



    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        Debug.Log("Unimplemented OnCreateRoomFailed.");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsMasterClient)
        {
            FindObjectOfType<ProgressSceneLoader>().LoadScene("GameScene");
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public void Queue()
    {
        if (isQueuing)
        {
            DeQueue();
        }

        isQueuing = true;
            
        ConnectToMaster();

        StartCoroutine(BeginCheckForStartGame());
    }

    public void DeQueue()
    {
        isQueuing = false;

        PhotonNetwork.Disconnect();

        StopAllCoroutines();
    }

    public void ReQueue()
    {
        DeQueue();
        Queue();
    }


    /// <summary>
    /// Checks two things for a verified Connection/game for Hoster
    /// 1. When someone has joined your lobby, start the game.
    /// 2. When you dont find another player in 5 seconds, quietly Dequeue then Requeue.
    /// Checks every .5 seconds.
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeginCheckForStartGame()
    {
        bool shouldRequeue = true;

        // Up to 2 second wait delay, before checking.
        // Mostly important for finding a room that was created before yours.
        float timeToWait = Random.Range(0.0f, 5.0f);
        yield return new WaitForSeconds(timeToWait);

        for (int i = 0; i < 10; i++)
        {
            if (isQueuing)
            {
                yield return new WaitForSeconds(0.5f);

                if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount == 2)
                {
                    isQueuing = false;
                    shouldRequeue = false;

                    FindObjectOfType<ProgressSceneLoader>().LoadScene("GameScene");
                    break;
                }
            }
        }

        if(shouldRequeue)
        {
            ReQueue();
        }
    }

    #endregion

   
}
