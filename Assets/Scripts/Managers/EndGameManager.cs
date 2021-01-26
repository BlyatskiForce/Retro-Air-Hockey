using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    [RequireComponent(typeof(GameObject))]
    [RequireComponent(typeof(GameObject))]
    public class EndGameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject endResults;

        [SerializeField] private TextMeshProUGUI resultsText;

        [FormerlySerializedAs("Menu")] [SerializeField]
        private GameObject menu;

        public Color32 winningColor;
    
        public Color32 losingColor;


        public override void OnEnable()
        {
            base.OnEnable();
            GameObject.Find("CountdownTimer").SetActive(false);

            GameManager.HasEnded = true;


            SetEndResultsText();

            menu.SetActive(false);

            endResults.SetActive(true);
            GameManager.IsPaused = true;

            // TO IMPLEMENT...
            // Send code to android app.
        }

        private void SetEndResultsText()
        {
            if(PhotonNetwork.IsMasterClient)
            {
                if(GameManager.PlayerOneScore >= 5)
                {
                    resultsText.color = winningColor;
                    resultsText.text = "You Won!";

                    // Throw firework for Player One winning, at Player Two's goal.
                    GameObject player1 = GameObject.FindGameObjectWithTag("Player");
                    if (player1.GetComponent<PhotonView>().IsMine)
                    {
                        player1.transform.parent.Find("Rockets").gameObject.SetActive(true);
                    }
                }
                else
                {
                    resultsText.color = losingColor;
                    resultsText.text = "You Lost.";
                }
            }else
            {
                if (GameManager.PlayerTwoScore >= 5)
                {
                    resultsText.color = winningColor;
                    resultsText.text = "You Won!";

                    // Throw firework for Player Two winning, at Player One's goal.
                    GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
                    if (player2.GetComponent<PhotonView>().IsMine)
                    {

                        player2.transform.parent.Find("Rockets").gameObject.SetActive(true);
                    }
                }
                else
                {
                    resultsText.color = losingColor;
                    resultsText.text = "You Lost.";
                }
            }
        
        }
    }
}
