using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using UnityEngine.Serialization;

/// <summary>
/// Detects the game rules.
/// </summary>
public class GameManager : MonoBehaviourPunCallbacks
{
    #region CONSTANTS
    public const int WINNING_COUNT = 5;
    public static string PlayerOneScoreKey = "PlayerOneScore";
    public static string PlayerTwoScoreKey = "PlayerTwoScore";
    public static string IS_PAUSED = "IsPaused";

    // Boundary
    public const float LeftX = -17.33f, RightX = 15.4f, UpperZ = 62.47f, LowerZ = -25f;
    public const float Player1MiddleZ = 14.89f;
    public const float Player2MiddleZ = 20.94f;
    #endregion

    // GUI
    [FormerlySerializedAs("Menu")] 
    public GameObject menu;

    [FormerlySerializedAs("EndGameManager")] [SerializeField]
    private GameObject endGameManager;

    [SerializeField]
    TextMeshProUGUI playerOneScoreText;
    public static int PlayerOneScore = 0;
    [SerializeField]
    TextMeshProUGUI playerTwoScoreText;
    public static int PlayerTwoScore = 0;


    public static bool IsPaused = true;
    public static bool HasEnded = false;

    public static bool isTesting = false;

    private void Start()
    {
        PlayerOneScore = 0;
        PlayerTwoScore = 0;
        HasEnded = false;
        IsPaused = true;


        if (isTesting)
        {
            IsPaused = false;
            return;
        }
        StartCoroutine(StartChecker());
    }

    private void OnEnable()
    {
        base.OnEnable();

        CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
    }

    public void PauseMyGame(bool shouldPause)
    {
        if (!HasEnded && !CountdownTimer.isTimerRunning)
        {
            IsPaused = shouldPause;
        }
    }

    /// <summary>
    /// Checks if the countdown timer may start
    /// Adds properties to the network.
    /// </summary>
    private IEnumerator StartChecker()
    {
        float startCheckingTime = Time.time;

        float waitTime = 10.0f;

        if(PhotonNetwork.IsConnected)
        {
            while (GameObject.Find("Player1") != null && GameObject.Find("Player2") != null)
            {
                yield return new WaitForSeconds(0.5f);

                if(startCheckingTime - Time.time == waitTime)
                {
                    // Try to reload game if for whatever reason you have been waiting for player for 10 seconds.
                    Debug.Log("Reloading game");
                    FindObjectOfType<ProgressSceneLoader>().LoadScene("GameScene");
                    break;
                }
            }
        }else if(isTesting)
        {
            // Actually just do nothing...
        }else
        {
            Debug.LogError("You are not connected and not testing yet still playing...");
            FindObjectOfType<ProgressSceneLoader>().LoadScene("MenuScreen");
        }

        if(PhotonNetwork.IsMasterClient)
        {
            // Ensures no one wants to restart.
            Hashtable props = new Hashtable
                    {
                        {RestartManager.PlayerOneWantsToRestart, false},
                        {RestartManager.PlayerTwoWantsToRestart, false}
                    };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);

            // Ensures everyone is paused.
            Hashtable prop2 = new Hashtable
                {
                    {IS_PAUSED, true}
                };
            PhotonNetwork.CurrentRoom.SetCustomProperties(prop2);

            // resets scores to 0
            Hashtable props3 = new Hashtable
                {
                    {PlayerOneScoreKey, 0},
                    {PlayerTwoScoreKey, 0}
                };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props3);
        }

        // Sets Player text to be tapable.
        GameObject.Find("StatusText").GetComponent<TextMeshProUGUI>().text = "Tap to Play";

        StartCoroutine(tapToStart());
    }

    /// <summary>
    /// Checks if someone has tapped to start the game.
    /// </summary>
    /// <returns></returns>
    private IEnumerator tapToStart()
    {
        GameObject startGameStuff = GameObject.Find("StartGameStuff");

        bool touched = false;

        while(!touched && !CountdownTimer.isTimerRunning && IsPaused)
        {
            yield return null;

            // Mobile Movement
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (Input.touchCount > 0)
                {
                    touched = true;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                touched = true;
            }

            if(touched)
            {
                // Starts countdown Timer.
                Hashtable props4 = new Hashtable
                {
                    {CountdownTimer.CountdownStartTime, (float) PhotonNetwork.Time}
                };
                PhotonNetwork.CurrentRoom.SetCustomProperties(props4);
            }

         
        }

        Destroy(startGameStuff);
    }

    public static void ResetPuck()
    {
        // Code for respawning players if needed.
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    GameObject player1 = GameObject.FindGameObjectWithTag("Player");
        //    player1.GetComponent<PlayerGoalie>().Respawn();
        //}
        //else
        //{
        //    GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
        //    player2.GetComponent<PlayerGoalie>().Respawn();
        //}

        FindObjectOfType<Puck>().ResetPuck();
    }

    /// <summary>
    /// Checks if there is a winner
    /// Then ends game.
    /// </summary>
    /// <returns></returns>
    public static void UpdateScore(string tag)
    {
        // Updates score from whichever goal.
        if(!isTesting)
        {
            object playerScoreFromProps;

            if (tag == "Player1Hole")
            {
                // Update PlayerTwo Value
                if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(PlayerTwoScoreKey, out playerScoreFromProps))
                {
                    int newScore = (int)playerScoreFromProps + 1;

                    Hashtable props = new Hashtable
                        {
                            {PlayerTwoScoreKey, (int)playerScoreFromProps + 1}
                        };

                    PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                }
                else
                {
                    Debug.LogError("Failed to get player2 score.");
                }

            }
            else if (tag == "Player2Hole")
            {

                if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(PlayerOneScoreKey, out playerScoreFromProps))
                {
                    int newScore = (int)playerScoreFromProps + 1;

                    Hashtable props = new Hashtable
                {
                    {PlayerOneScoreKey, newScore}
                };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                }
                else
                {
                    Debug.LogError("Failed to get player1 score.");

                }
            }
        }

    }

    private void Update()
    {
        // Mobile key inputs
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // Check if Back was pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (menu.activeSelf || !HasEnded)
                {
                    menu.SetActive(!menu.activeSelf);
                }
            }
        }
        else
        {
            // Check if Back was pressed 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(menu.activeSelf || !HasEnded)
                {
                    menu.SetActive(!menu.activeSelf);
                }
            }
        }
    }

    private void CheckIfWinner(int score)
    {
        if(score >= WINNING_COUNT)
        {
            endGameManager.SetActive(true);
        }
    }

    #region PUN CALLBACKS

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        object InfoFromProps;

        if (propertiesThatChanged.TryGetValue(PlayerOneScoreKey, out InfoFromProps))
        {
            playerOneScoreText.text = ((int)InfoFromProps).ToString();
            PlayerOneScore = ((int)InfoFromProps);
            CheckIfWinner(PlayerOneScore);
        }
        
        if(propertiesThatChanged.TryGetValue(PlayerTwoScoreKey, out InfoFromProps))
        {
            playerTwoScoreText.text = ((int)InfoFromProps).ToString();
            PlayerTwoScore = ((int)InfoFromProps);

            CheckIfWinner(PlayerTwoScore);
        }
        
        if(propertiesThatChanged.TryGetValue(IS_PAUSED, out InfoFromProps))
        {
            IsPaused = (bool)InfoFromProps;
        }
    }

    #endregion


    /// <summary>
    /// Set ispaused to false for both players.
    /// </summary>
    private void StartGame()
    {
        Hashtable prop = new Hashtable
                {
                    {IS_PAUSED, false}
                };
        PhotonNetwork.CurrentRoom.SetCustomProperties(prop);

        Debug.Log("You have unpaused game.");
    }

    public static Vector3 checkBoundaries(Vector3 vec, string tag)
    {
        // Puck needs extra wiggle room for score.
        float UpperZ = GameManager.UpperZ;
        float LowerZ = GameManager.LowerZ;
        float RightX = GameManager.RightX;
        float LeftX = GameManager.LeftX;

        if(tag == "Puck")
        {
            UpperZ += 10;
            LowerZ -= 10;
            RightX += 10;
            LeftX -= 10;
        }


        if (vec.z > UpperZ)
        {
            vec.z = UpperZ;
        }
        else if (vec.z < LowerZ)
        {
            vec.z = LowerZ;   
        }
        if (vec.x > RightX)
        {
            vec.x = RightX;
        }
        else if (vec.x < LeftX)
        {
            vec.x = LeftX;
        }

        if(tag == "Player" && vec.z > Player1MiddleZ)
        {
            vec.z = Player1MiddleZ;
        }
        else if(tag == "Player2" && vec.z < Player2MiddleZ)
        {
            vec.z = Player2MiddleZ;
        }
        
        return vec;
    }

    private void OnCountdownTimerIsExpired()
    {
        StartGame();
    }

}
