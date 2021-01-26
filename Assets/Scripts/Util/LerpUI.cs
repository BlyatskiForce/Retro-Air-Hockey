using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LerpUI : MonoBehaviour
{
    public Vector2 lerpPosition;

    private RectTransform rectTransform;

    public float smoothTime = 0.5f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        rectTransform.DOAnchorPos(lerpPosition, smoothTime);
    }
}
