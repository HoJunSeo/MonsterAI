using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Logo : BaseObject
{
    UIButton StartButton = null;
	// Use this for initialization
	void Start ()
    {
        Transform temp = this.GetChild("StartButton");
        if(temp == null)
        {
            Debug.Log("Logo 에 StartButton 이 없습니다.");
            return;
        }

        StartButton = temp.GetComponent<UIButton>();

        // 버튼으로 진입하는 방법 EventDelegate 사용
        //StartButton.onClick.Add(new EventDelegate(this, "GoLobby"));
        //EventDelegate.Add(StartButton.onClick, new EventDelegate(this, "GoLobby"));

        EventDelegate.Add(StartButton.onClick,
            // Lamda ( 이름 없는 메서드 )
            () =>
            {
                // 신 전환 ( 동기 방식으로 호출할 수 있게 만들어 준다. )
                Scene_Manager.Instance.LoadScene(eSceneType.Scene_Lobby);
            }
            );
	}

    //void GoLobby()
    //{
    //    // 신 전환
    //}
}
