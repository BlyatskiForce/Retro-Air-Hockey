using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    Camera camera;

    private const float baseAngleX = 20;
    private const float rotAngleX = 10;
    private bool isPositiveX = false;

    private const float baseAngleY = 90;
    private const float rotAngleY = 10;
    private bool isPositiveY = false;

    [SerializeField]
    private float speed = 1.0f;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPositiveY)
        {
            Vector3 euler = camera.transform.rotation.eulerAngles;

            euler.y = Mathf.Lerp(euler.y, euler.y + rotAngleY, speed * Time.deltaTime);
            transform.localEulerAngles = euler;

            if(transform.localEulerAngles.y > baseAngleY + rotAngleY)
            {
                isPositiveY = false;
            }
        }
        else
        {
            Vector3 euler = transform.localEulerAngles;

            euler.y = Mathf.Lerp(euler.y, euler.y - rotAngleY, speed * Time.deltaTime);
            transform.localEulerAngles = euler;

            if (transform.localEulerAngles.y < baseAngleY - rotAngleY)
            {
                isPositiveY = true;
            }
        }
    }
}
