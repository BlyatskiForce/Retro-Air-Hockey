using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ColorLerp : MonoBehaviour
{
    TextMeshProUGUI textMeshPro;

    [SerializeField]
    [Range(0f, 1f)]
    float lerpTime;

    [SerializeField]
    Color[] colors;

    int colorIndex = 0;

    float t = 0f;

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.color = Color.Lerp(textMeshPro.color, colors[colorIndex], lerpTime * Time.deltaTime);

        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
        if(t>.9f)
        {
            t = 0f;
            colorIndex++;
            colorIndex = (colorIndex >= colors.Length) ? 0 : colorIndex;

        }
    }
}
