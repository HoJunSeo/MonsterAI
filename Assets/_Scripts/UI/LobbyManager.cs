using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoSingleton<LobbyManager>
{
    public void LoadLobby()
    {
        UITools.Instance.ShowUI(eUIType.Pf_UI_Lobby);
    }

    public void DisableLobby()
    {
        UITools.Instance.HideUI(eUIType.Pf_UI_Lobby);
    }
}

