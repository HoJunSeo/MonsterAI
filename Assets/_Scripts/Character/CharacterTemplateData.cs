using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

// 자료형은 list에 string 키값을 저장하고 있다.
public class CharacterTemplateData
{
	string DataKey = string.Empty;

	StatusData Status = new StatusData();
	List<string> listSkill = new List<string>();

	public string KEY { get { return DataKey; } }
	public StatusData STATUS { get { return Status; } }
	public List<string> LIST_SKILL { get { return listSkill; } }
	
	public CharacterTemplateData(string strKey, JSONNode nodeData )
	{
		DataKey = strKey;

		for(int i = 0; i < (int)eStatusData.MAX; i++)
		{
			eStatusData statusData = (eStatusData)i;

			double valueData =
				nodeData[statusData.ToString()].AsDouble;
			Status.IncreaseData(statusData, valueData);
		}


		JSONArray arrSkill = nodeData["SKILL"].AsArray;
		if(arrSkill != null && arrSkill.Count > 0)
		{
			for(int i = 0; i < arrSkill.Count; i++)
			{
				listSkill.Add(arrSkill[i]);
			}
		}


	}


}
