using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerGoalie : MonoBehaviourPunCallbacks
{
    private Rigidbody _rb;
    private PhotonView _pv;
    private GameManager _mnger;

    float _spawnCoordX;
    float _spawnCoordZ;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _pv = GetComponent<PhotonView>();
        _mnger = GameObject.Find("Managers").GetComponentInChildren<GameManager>();

        var position = _rb.position;
        _spawnCoordX = position.x;
        _spawnCoordZ = position.z;
    }

    private void Start()
    {
        
        if (!_pv.IsMine && !GameManager.isTesting)
        {
            Destroy(transform.parent.Find("Camera").gameObject);
        }
        
        if(GameObject.Find("Main Camera") != null)
        {
            Destroy(GameObject.Find("Main Camera").gameObject);
        }
    }

    public void Respawn()
    {
        if(!GameManager.isTesting && _rb.GetComponent<PhotonView>().IsMine)
        {
            _rb.position = new Vector3(_spawnCoordX, _rb.position.y, _spawnCoordZ);
        }
    }

    private void FixedUpdate()
    {
        if(!GameManager.IsPaused && !GameManager.HasEnded && _pv.IsMine || GameManager.isTesting )
        {
            // Mobile Movement
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Moved)
                    {
                        MovePlayer(touch.position);
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                MovePlayer(Input.mousePosition);
            }
        }

    }

    private void MovePlayer(Vector3 mousePos)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);


        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 vec = new Vector3(
                hit.point.x,
                _rb.transform.position.y,
                hit.point.z);   

            vec = GameManager.checkBoundaries(vec, tag);
                    
            _rb.MovePosition(vec);
                    
        }
    }
}

