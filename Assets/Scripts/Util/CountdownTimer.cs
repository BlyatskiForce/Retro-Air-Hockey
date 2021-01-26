using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CountdownTimer : MonoBehaviourPunCallbacks
{
    public const string CountdownStartTime = "StartTime";

    /// <summary>
    /// OnCountdownTimerHasExpired delegate.
    /// </summary>
    public delegate void CountdownTimerHasExpired();

    /// <summary>
    /// Called when the timer has expired.
    /// </summary>
    public static event CountdownTimerHasExpired OnCountdownTimerHasExpired;


    public static bool isTimerRunning = false;

    private float startTime;

    [Header("Reference to a Text component for visualizing the countdown")]
    public TextMeshProUGUI Text;

    [Header("Countdown time in seconds")]
    public float Countdown = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (Text == null)
        {
            Debug.LogError("Reference to 'Text' is not set. Please set a valid reference.", this);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTimerRunning)
        {
            return;
        }

        float timer = (float)PhotonNetwork.Time - startTime;
        float countdown = Countdown - timer;

        Text.text = string.Format("Game starts in {0} seconds", countdown.ToString("n2"));

        if (countdown > 0.0f)
        {
            return;
        }

        isTimerRunning = false;

        Text.text = string.Empty;

        if (OnCountdownTimerHasExpired != null)
        {
            OnCountdownTimerHasExpired();
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        object startTimeFromProps;

        if (propertiesThatChanged.TryGetValue(CountdownStartTime, out startTimeFromProps))
        {
            isTimerRunning = true;
            startTime = (float)startTimeFromProps;
        }
    }

    public bool getIsTimerRunning()
    {
        return isTimerRunning;
    }
}
