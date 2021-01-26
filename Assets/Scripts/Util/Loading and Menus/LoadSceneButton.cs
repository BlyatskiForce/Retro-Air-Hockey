using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private string m_SceneToLoad = null;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(LoadScene);
    }



    public void LoadScene()
    {
        gameObject.SetActive(false);
        FindObjectOfType<ProgressSceneLoader>().LoadScene(m_SceneToLoad);
    }



}
