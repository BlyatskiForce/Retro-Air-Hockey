using UnityEngine;
using Photon.Pun;

public class PUN2_RigidBodySync : MonoBehaviourPun//, IPunObservable
{

    void OnCollisionEnter(Collision contact)
    {
        string tag = contact.transform.tag;

        if(PhotonNetwork.IsConnected && tag == "Player" || tag == "Player2")
        {
            if(contact.gameObject.GetComponent<PhotonView>().IsMine 
                && !photonView.IsMine)
            {
                Debug.Log("Took control of Puck PhotonView.");

                photonView.RequestOwnership();
            }
        }


    }
}
