using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Button))]
public class LoadMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject[] menusToDisable = null;
    [SerializeField]
    private GameObject[] menusToEnable = null;




    private void Awake()
    {
        Button o = GetComponent<Button>();

        foreach(GameObject obj in menusToDisable)
        {
            o.onClick.AddListener(() => obj.SetActive(false));
        }

        foreach (GameObject obj in menusToEnable)
        {
            o.onClick.AddListener(() => obj.SetActive(true));
        }
        
    }
}
