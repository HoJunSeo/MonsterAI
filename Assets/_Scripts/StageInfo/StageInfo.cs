using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

// 스테이지의 자료를 저장하는 공간
public class StageInfo
{
    string DataKey = string.Empty;
    string Name = string.Empty;
    string MapModel = string.Empty;
    eClearType ClearType = eClearType.CLEAR_KILLCOUNT;

    double ClearFinish = 0.0f;

    public string KEY { get { return DataKey; } }
    public string NAME { get { return Name; } }
    public string MODEL { get { return MapModel; } }
    public eClearType CLEAR_TYPE { get { return ClearType; } }
    public double CLEAR_FINISH { get { return ClearFinish; } }

    // SkillTemplate 이랑 비슷한 유형
    public StageInfo(string strkey, JSONNode nodeData)
    {
        DataKey = strkey;

        Name = nodeData["NAME"];

        MapModel = nodeData["MAP_MODEL"];

        //ClearType = (eClearType)nodeData["CLEAR_TYPE"].AsInt;

        ClearType = (eClearType)int.Parse(nodeData["CLEAR_TYPE"]);

        ClearFinish = nodeData["CLEAR_FINISH"].AsDouble;
    }
}
