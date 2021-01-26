using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{

    private RectTransform rectComponent;
    private Image imageComp;
    public float speed = 0.0f;


    private void Awake()
    {
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = 0.0f;
    }

    private void OnEnable()
    {
        imageComp.fillAmount = 0.0f;
    }

    void Update()
    {
        if (imageComp.fillAmount != 1f)
        {
            imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;

        }
        else
        {
            imageComp.fillAmount = 0.0f;
        }
    }
}
