using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

// Template 
// String Skill Key로 저장
// Skill Data에 밀리 / 레인지 / etc 저장
public class SkillData
{
    // 오류 발생
    string DataKey = string.Empty;
    float Range = 0;
    List<string> SkillList = new List<string>();

    public float RANGE { get { return Range; } }
    public List<string> SKILL_LIST { get { return SkillList; } }

    public SkillData(string strKey, JSONNode nodeData)
    {
        DataKey = strKey;
        Range = nodeData["RANGE"].AsFloat;

        JSONArray arrSkill = nodeData["SKILL"].AsArray;
        if(arrSkill != null)
        {
            for(int i = 0; i<arrSkill.Count; i++)
            {
                SkillList.Add(arrSkill[i]);

            }
        }
    }
}

