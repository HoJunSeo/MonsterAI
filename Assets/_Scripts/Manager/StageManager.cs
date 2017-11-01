using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

// 스테이지 프리팹을 동적할당으로 자동 생성
public class StageManager : MonoSingleton<StageManager>
{
    Dictionary<int, StageInfo> DicStageInfo =
        new Dictionary<int, StageInfo>();

    public Dictionary<int, StageInfo> DIC_STAGEINFO
    { get { return DicStageInfo; } }

    public void StageInit()
    {
        TextAsset StageInfo = Resources.Load(ConstValue.StageDataPath) as TextAsset;

        JSONNode rootNode = JSON.Parse(StageInfo.text);

        foreach(KeyValuePair<string, JSONNode> pair in rootNode[ConstValue.StageDataKey] as JSONObject)
        {
            StageInfo info = new StageInfo(pair.Key, pair.Value);
            DicStageInfo.Add(int.Parse(info.KEY), info);
        }
    }

    public StageInfo LoadStage(int selectStage)
    {
        StageInfo info = null;
        DicStageInfo.TryGetValue(selectStage, out info);

        if(info == null)
        {
            Debug.LogError("#1 JSON 정상 로드 확인" + " #2 JSON Key 값 확인");

            return null;
        }

        // info.MODEL 을 통해 스테이지를 골라준다.
        GameObject go = Resources.Load("Prefabs/Stages/" + info.MODEL) as GameObject;
        Debug.Assert(go != null, "스테이지 리소스 로드 실패");

        Instantiate(go, Vector3.zero, Quaternion.identity);
        return info;
    }
}
