using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleHelp : MonoBehaviour
{
    [SerializeField]
    GameObject helpRoot;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            helpRoot.SetActive(!helpRoot.activeSelf);
        }
    }
}
