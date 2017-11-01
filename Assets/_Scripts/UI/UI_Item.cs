using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Item : BaseObject {

    ItemInstance itemInstance;
    public ItemInstance ITEM_INSTANCE
    {
        // itemInstance에 자료값을 넣어줘서 반환 null이면 반환해주는 자료가 없다.
        get { return itemInstance; }
        set { itemInstance = value; }
    }

    UILabel Label;
    UITexture Texture;

    public void Init(ItemInstance inst)
    {
        ITEM_INSTANCE = inst;

        Label = GetComponentInChildren<UILabel>();
        Texture = GetComponentInChildren<UITexture>();

        Label.text = inst.ITEM_INFO.NAME;
        Texture.mainTexture = Resources.Load("Item_Textures/" + inst.ITEM_INFO.ITEM_IMAGE) as Texture; 

    }

    // UICamera 안에 들어있다.
    void OnClick()
    {
        BaseObject bo = UITools.Instance.ShowUI(eUIType.Pf_UI_Popup);
        UI_Popup Popup = bo.GetComponent<UI_Popup>();

        Popup.Set(ItemYes, ItemNo, "아이템 장착", "이 장비를 장착 하시겠습니까?");
    }

    void ItemYes()
    {
        // UI_Inventory가 가지고 있는 아이템 자료를 매니저에서 실행 후 장착을 실행 해준다.
        ItemManager.Instance.EquipItem(ITEM_INSTANCE);
        UITools.Instance.HideUI(eUIType.Pf_UI_Popup);
    }

    void ItemNo()
    {
        UITools.Instance.HideUI(eUIType.Pf_UI_Popup);
    }
}
