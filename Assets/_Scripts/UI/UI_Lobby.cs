using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Lobby : BaseObject {

    UIButton StageButton = null;
    UIButton GachaButton = null;
    UIButton InvenButton = null;

    // UI_Logo 참고 로그 프로젝트에는 람다 형식으로 작성 되었다.
    private void Awake()
    {
        Transform temp = GetChild("StageButton");
        if (temp == null)
        {
            Debug.Log("StageButton을 찾을 수 없습니다.");
            return;
        }

        StageButton = temp.GetComponent<UIButton>();

        EventDelegate.Add(StageButton.onClick, new EventDelegate(this, "ShowStage"));

        temp = GetChild("GachaButton");
        if (temp == null)
        {
            Debug.LogError("GachaButton 을 찾을 수 업습니다.");
            return;
        }
        GachaButton = temp.GetComponent<UIButton>();
        EventDelegate.Add(GachaButton.onClick, new EventDelegate(
            () =>
            {
                ItemManager.Instance.Gacha();
            }
            )
            );

        // Inventory
        temp = GetChild("InvenButton");
        if (temp == null)
        {
            Debug.LogError("InvenButton 을 찾을 수 업습니다.");
            return;
        }
        InvenButton = temp.GetComponent<UIButton>();
        EventDelegate.Add(InvenButton.onClick, new EventDelegate(this, "ShowInventory"));

    }

    void ShowStage()
    {
        UITools.Instance.ShowUI(eUIType.Pf_UI_Stage);
    }

    void ShowInventory()
    {
        BaseObject inven = UITools.Instance.ShowUI(eUIType.Pf_UI_Inventory) as BaseObject;
        inven.ThrowEvent("Inven_Init");
    }
}
