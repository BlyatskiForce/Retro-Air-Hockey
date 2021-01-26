using System.Collections;
using UnityEngine;
using Photon.Pun;


public class Puck : MonoBehaviourPunCallbacks
{
    Rigidbody rb;
    CapsuleCollider cp;
    PhotonView pv;

    private bool isHit = false;

    public const float spawnX = -1.05f, spawnY = 5.33f, spawnZ = 18.1f;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cp = GetComponent<CapsuleCollider>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        StartCoroutine(CheckPosition());
    }

    void OnCollisionEnter(Collision c)
    {
        if((c.gameObject.tag == "Player1Hole" || c.gameObject.tag == "Player2Hole"))
        {
            if(PhotonNetwork.IsConnectedAndReady && photonView.IsMine)
            {
                GameManager.UpdateScore(c.gameObject.tag);
            }

            ResetPuck();
        }
    }

    // Resets puck to original position if its out of bounds
    public void ResetPuck()
    {
        cp.isTrigger = true;
        rb.velocity = Vector3.zero;
        rb.ResetCenterOfMass();
        rb.ResetInertiaTensor();
        rb.position = new Vector3(spawnX, spawnY, spawnZ);
        rb.velocity = Vector3.zero;
        cp.isTrigger = false;
    }
    // Every second check if the boundaries are incorrect.
    // Reset position if it is.
    IEnumerator CheckPosition()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);

            if (GameManager.isTesting || PhotonNetwork.IsConnectedAndReady)
            {
                Vector3 pos = rb.position;

                Vector3 newPos = GameManager.checkBoundaries(pos, "Puck");
                if (pos != newPos)
                {
                    ResetPuck();
                }
            }
        }
    }
}
