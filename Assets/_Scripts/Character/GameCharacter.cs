using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Template 와 status로 나누어진다
// Template 는 원본이고 Skill과 Status가 포함되어 있다.
public class GameCharacter 
{
	public BaseObject TargetComponenet = null;

	CharacterTemplateData TemplateData = null;
	CharacterStatusData CharacterStatus = new CharacterStatusData();

	public CharacterTemplateData GetCharacterTemplate
	{ get { return TemplateData; } }
	public CharacterStatusData GetCharacterStatus
	{ get { return CharacterStatus; } }

    public SkillData SELECT_SKILL
    {
        get;
        set;
    }
    List<SkillData> ListSkill = new List<SkillData>();

	double CurrentHP = 0;
	public double CURRENT_HP
	{ get { return CurrentHP; } }


    // 
	public void IncreaseCurrentHP(double valueData)
	{
		CurrentHP += valueData;

		if (CurrentHP < 0)
			CurrentHP = 0;

        // maxHP 자료 불러다가 저장
		double maxHP =
			CharacterStatus.GetStatusData(eStatusData.MAX_HP);
		if (CurrentHP > maxHP)
			CurrentHP = maxHP;

		if (CurrentHP == 0)
			TargetComponenet.ObjectState = 
				eBaseObjectState.STATE_DIE;
	}

	public void SetTemplate(CharacterTemplateData _templateData)
	{
		TemplateData = _templateData;
		CharacterStatus.AddStatusData(
			ConstValue.CharacterStatusDataKey,
			TemplateData.STATUS);
		CurrentHP =
			CharacterStatus.GetStatusData(eStatusData.MAX_HP);

        // Skill Setting
        for(int i = 0; i<TemplateData.LIST_SKILL.Count; i++)
        {
            SkillData data = SkillManager.Instance.GetSkillData(TemplateData.LIST_SKILL[i]);

            if (data == null)
            {
                Debug.LogError(TemplateData.LIST_SKILL[i] + " 스킬 키를 찾을 수 없습니다.");
                return;
            }
            else
            {
                AddSkill(data);
            }
        }
    }

    public bool EquipSkillByIndex(int index)
    {
        if (ListSkill.Count > index)
        {
            SELECT_SKILL = ListSkill[index];
        }
        else
            return false;

        return true;
    }

    public SkillData GetSkillDataByIndex(int index)
    {
        if (ListSkill.Count > index)
        {
            return ListSkill[index];
        }
        else
            return null;
    }

    public void AddSkill(SkillData data)
    {
        ListSkill.Add(data);
    }
}
