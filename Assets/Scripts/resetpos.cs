using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resetpos : MonoBehaviour
{
    [SerializeField]
    Rigidbody ob;
    
    Vector3 origPosition;

    private void Start()
    {
        origPosition = ob.transform.position;

        gameObject.GetComponent<Button>().onClick.AddListener(() => posNVelReset());
    }

    private void posNVelReset()
    {
        ob.velocity = Vector3.zero;
        ob.position = origPosition;
    }
}
