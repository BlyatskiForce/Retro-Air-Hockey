using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Displays Loading Screen while Loading Level.
/// </summary>
public class ProgressSceneLoader : MonoBehaviour
{
    private static ProgressSceneLoader instance = null;

    private Canvas m_canvas;

    private void Awake()
    {
        m_canvas = gameObject.GetComponent<Canvas>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void LoadScene(string sceneName)
    {
        m_canvas.enabled = true;


        StartCoroutine(BeginLoad(sceneName));
    }


    private IEnumerator BeginLoad(string sceneName)
    {
        // Waits for a second at least to show the loading screen.

        yield return new WaitForSeconds(1f);
        PhotonNetwork.LoadLevel(sceneName);

        float time = 0;
        while (!(PhotonNetwork.LevelLoadingProgress != 1.0f))
        {
            time += Time.deltaTime;

            if(time > 10f)
            {
                Debug.LogError("Loading too long (Over 10 seconds), timed out.");
                PhotonNetwork.Disconnect();
            }

            yield return null;
        }

        m_canvas.enabled = false;
    }
}
