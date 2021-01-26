using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
	public Color32 player1Color;
	public Color32 player2Color;

	public override void OnEnable()
	{
		base.OnEnable();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if (scene.buildIndex == 1) // Game Scene
		{
			PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
		}

		// Sets player text name and its color.
		TextMeshProUGUI playerText = GameObject.Find("PlayerName").GetComponent<TextMeshProUGUI>();
		if (PhotonNetwork.IsMasterClient)
        {
			playerText.text = "Player 1";
			playerText.color = player1Color;
		}
		else
        {
			playerText.text = "Player 2";
			playerText.color = player2Color;
		}
		
	}

}
