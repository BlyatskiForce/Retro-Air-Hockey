using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disables gameobject by stated amount of time...
/// </summary>
public class DisableGOBy: MonoBehaviour
{
    public float amountOfSecondsTillDisabled;

    private void OnEnable()
    {
        StartCoroutine(disableGOBy());
    }

    IEnumerator disableGOBy()
    {
        yield return new WaitForSeconds(amountOfSecondsTillDisabled);
        gameObject.SetActive(false);
    }
}
