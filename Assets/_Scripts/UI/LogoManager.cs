using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoManager : MonoBehaviour {

    private void Start()
    {
        UITools.Instance.ShowUI(eUIType.Pf_UI_Logo);
    }
}
