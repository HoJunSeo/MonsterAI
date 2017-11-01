using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StageIcon : BaseObject {

    StageInfo Info = null;

    public StageInfo INFO
    {
        get { return Info; }
    }

    UILabel StageName = null;

    public void Init(StageInfo info)
    {
        Info = info;
        StageName = this.GetComponentInChildren<UILabel>();
        StageName.text = info.NAME;
    }

    public void OnClick()
    {
        BaseObject bo = UITools.Instance.ShowUI(eUIType.Pf_UI_Popup);
        UI_Popup popup = bo.GetComponent<UI_Popup>();
        
        // 이름 없는 메서드 ( 어딘가의 공간에 저장되어 있다. )
        popup.Set(
            () =>
            {
                // Yes Button
                Debug.Log(INFO.NAME + " 입장 ");
                GameManager.Instance.SelectStage = int.Parse(INFO.KEY);
                Scene_Manager.Instance.LoadScene(eSceneType.Scene_Game);
                UITools.Instance.HideUI(eUIType.Pf_UI_Popup);
            },
            () =>
            {
                // No Button
                UITools.Instance.HideUI(eUIType.Pf_UI_Popup);
            },
            "스테이지 선택", "스테이지" + INFO.NAME + " 을 입장하시겠습니까?");


    }
}
