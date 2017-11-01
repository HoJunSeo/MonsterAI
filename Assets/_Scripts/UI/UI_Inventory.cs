using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : BaseObject {

    bool IsInit = false;
    GameObject ItemPrefab;

    UIGrid Grid;
    UIButton CloseButton;
    UILabel WeaponLabel;
    UILabel ArmorLabel;
    UILabel ShieldLabel;
    UILabel AccLabel;


    private void Awake()
    {
        CloseButton = GetChild("CloseButton").GetComponent<UIButton>();
        // Lamda식
        CloseButton.onClick.Add(new EventDelegate(() => { UITools.Instance.HideUI(eUIType.Pf_UI_Inventory); }));
    }

    public override void ThrowEvent(string keyData, params object[] datas)
    {
        if(keyData.Equals("Inven_Init") == true)
        {
            Init();
            Reset();
        }
    }

    public void Init()
    {
        if (IsInit == true)
            return;
        IsInit = true;

        ItemPrefab = Resources.Load("Prefabs/UI/Pf_UI_Item") as GameObject;

        Grid = GetComponentInChildren<UIGrid>();

        WeaponLabel = GetChild("Weapon").GetComponent<UILabel>();
        ArmorLabel = GetChild("Armor").GetComponent<UILabel>();
        ShieldLabel = GetChild("Shield").GetComponent<UILabel>();
        AccLabel = GetChild("Acc").GetComponent<UILabel>();


        EquipItemReset();
        ItemManager.Instance.EquipE = EquipItemReset;
    }

    public void Reset()
    {
        for(int i = 0; i<Grid.transform.childCount; i++)
        {
            Destroy(Grid.transform.GetChild(i).gameObject);
        }
        AddItem();
    }

    void AddItem()
    {
        List<ItemInstance> list = ItemManager.Instance.LIST_ITEM;
        for(int i = 0; i<list.Count; i++)
        {
            // Grid가 정렬을 해주기 때문에 임의로 포지션을 바꿀 필요가 없다
            GameObject go = Instantiate(ItemPrefab, Grid.transform);
            go.transform.localScale = Vector3.one;
            go.GetComponent<UI_Item>().Init(list[i]);
        }

        Grid.repositionNow = true;
    }

    public void EquipItemReset()
    {
        Dictionary<eSlotType, ItemInstance> dic = ItemManager.Instance.DIC_EQUIP;

        foreach(KeyValuePair<eSlotType, ItemInstance> Pair in dic)
        {

            switch (Pair.Key)
            {
                case eSlotType.Slot_Weapon:
                    //WeaponLabel = GetChild("Wapon").GetComponent<UILabel>();
                    WeaponLabel.text = Pair.Value.ITEM_INFO.GetSlotString()
                        + "   " + Pair.Value.ITEM_INFO.NAME;
                    break;
                case eSlotType.Slot_Armor:
                    //ArmorLabel = GetChild("Armor").GetComponent<UILabel>();
                    ArmorLabel.text = Pair.Value.ITEM_INFO.GetSlotString()
                        + "   " + Pair.Value.ITEM_INFO.NAME;
                    break;
                case eSlotType.Slot_Shield:
                    //ShieldLabel = GetChild("Shield").GetComponent<UILabel>();
                    ShieldLabel.text = Pair.Value.ITEM_INFO.GetSlotString()
                        + "   " + Pair.Value.ITEM_INFO.NAME;
                    break;
                case eSlotType.Slot_Acc:
                    //AccLabel = GetChild("Acc").GetComponent<UILabel>();
                    AccLabel.text = Pair.Value.ITEM_INFO.GetSlotString()
                        + "   " + Pair.Value.ITEM_INFO.NAME;
                    break;
            }

        }
    }
}
