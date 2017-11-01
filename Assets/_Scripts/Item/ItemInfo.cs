using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class ItemInfo
{
    string DataKey = string.Empty;
    string ItemName = string.Empty;

    eSlotType SlotTtype = eSlotType.Slot_None;
    StatusData Status = new StatusData();
    string ItemImage = string.Empty;

    public string KEY { get { return DataKey; } }
    public string NAME { get { return ItemName; } }
    public eSlotType SLOT_TYPE { get { return SlotTtype; } }
    public StatusData STATUS { get { return Status; } }
    public string ITEM_IMAGE { get { return ItemImage; } }

    public ItemInfo(string strKey, JSONNode nodedata)
    {
        DataKey = strKey;
        //ItemName = strKey;

        // Datakey 와 중복 // nodedata 안에는 KEY가 없다
        //DataKey = nodedata["KEY"]; 필요한가?

        ItemName = nodedata["NAME"];
        SlotTtype = (eSlotType)int.Parse(nodedata["SLOT_TYPE"]);

        for(int i = 0; i < (int)eStatusData.MAX; i++)
        {
            eStatusData status = (eStatusData)i;
            double valueData = nodedata[status.ToString()].AsDouble;
            if (valueData > 0)
                Status.IncreaseData(status, valueData);
        }

        ItemImage = nodedata["IMAGE"];

    }

    public string GetSlotString()
    {
        string returnStr = string.Empty;
        returnStr = SlotTtype.ToString().Split('_')[1];
        return returnStr;
    }

}
