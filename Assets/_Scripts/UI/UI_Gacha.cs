using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Gacha : BaseObject {

    UILabel Label = null;
    UITexture Texture = null;

    ItemInstance ItemInst;
    public ItemInstance ITEM_INSTANCE
    {
        get { return ItemInst; }
        set { ItemInst = value; }
    }

    public void Init(ItemInstance instance)
    {

        ItemInst = instance;

        Label = GetChild("Contents").GetComponent<UILabel>();
        Texture = GetChild("Texture").GetComponent<UITexture>();

        Label.text = ItemInst.ITEM_INFO.GetSlotString()
            + " : " + ItemInst.ITEM_INFO.NAME
            + "\n" + ItemInst.ITEM_INFO.STATUS.StatusString();

        Texture.mainTexture = Resources.Load("Item_Textures/" + ItemInst.ITEM_INFO.ITEM_IMAGE) as Texture;

        // Effect 초기화
        TweenAlpha alpha = GetChild("Effect").GetComponent<TweenAlpha>();
        alpha.ResetToBeginning();
        alpha.enabled = true;

        
    }

    public override void ThrowEvent(string keyData, params object[] datas)
    {
        if(keyData.Equals("GACHA") == true)
        {
            ItemInstance inst = datas[0] as ItemInstance;
            Init(inst);
        }
    }

    public void YesClick()
    {
        UITools.Instance.HideUI(eUIType.Pf_UI_Gacha);
    }

}
