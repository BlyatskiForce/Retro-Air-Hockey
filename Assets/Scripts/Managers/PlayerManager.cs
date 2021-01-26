using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviourPunCallbacks
{
	PhotonView _pv;

    #region UNITY
    void Awake()
	{
		_pv = GetComponent<PhotonView>();

		if (_pv.IsMine)
		{
			CreateController();
		}
	}

    #endregion

    #region PHOTON
    void CreateController()
	{
		Debug.Log("Creating a controller");
		
		if (PhotonNetwork.IsMasterClient && GameObject.Find("Player1(Clone)") == null)
		{
			PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player1"), Vector3.zero, Quaternion.identity);
		}
		else if(GameObject.Find("Player2(Clone)") == null)
		{
			StartCoroutine(CreatePlayer2());
		}
        else
        {
			Debug.LogError("Player1 and Player2 is filled. You have been Disconnected.");
			//PhotonNetwork.Disconnect();
			//PhotonNetwork.LoadLevel("MenuScreen");
        }
	}

	// Ensures that player2 is made after host creates player.
	private IEnumerator CreatePlayer2()
    {
		while(GameObject.Find("Player1(Clone)") == null)
        {
			yield return 0.5f;
		}

		PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player2"), Vector3.zero, Quaternion.identity);
	}

    #endregion

}