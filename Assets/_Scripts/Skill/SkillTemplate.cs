﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

// SkillData 묶음 안에 들어있는게 SkillTemplate 이다.
// RANGE_DATA_1 / RANGE_DATA_2 / etc 가 들어있다.
// Sound 나 Effect를 넣어줄 수도 있다.
public class SkillTemplate
{
    string DataKey = string.Empty;

    eSkillTemplateType SkillType = eSkillTemplateType.TARGET_ATTACK;
    eSkillAttackRangeType RangeType = eSkillAttackRangeType.RANGE_BOX;

    //eSkillAttackRangeType Box x : RangetData_1 y : RangeData_2
    //Range Sphere radius : RangeData_1

    float RangeData_1 = 0;
    float RangeData_2 = 0;

    StatusData SkillStatus = new StatusData();

    public eSkillTemplateType SKILL_TYPE { get { return SkillType; } }
    public eSkillAttackRangeType RANGE_TYPE { get { return RangeType; } }
    public float RANGE_DATA_1 { get { return RangeData_1; } }
    public float RANGE_DATA_2 { get { return RangeData_2; } }
    public StatusData STATUS_DATA { get { return SkillStatus; } }

    public SkillTemplate(string strKey, JSONNode nodeData)
    {
        DataKey = strKey;

        SkillType = (eSkillTemplateType)nodeData["SKILL_TYPE"].AsInt;
        RangeType = (eSkillAttackRangeType)nodeData["RANGE_TYPE"].AsInt;
        //System.Enum.Parse(typeof(eSkillTemplateType), nodeData["TARGET_ATTACK"]);

        RangeData_1 = nodeData["RANGE_DATA_1"].AsFloat;
        RangeData_2 = nodeData["RANGE_DATA_2"].AsFloat;

        for(int i = 0; i< (int)eStatusData.MAX; i++)
        {
            eStatusData statusData = (eStatusData)i;
            double valueData = nodeData[statusData.ToString()].AsDouble;
            if (valueData > 0)
                SkillStatus.IncreaseData(statusData, valueData);
        }
    }
}
