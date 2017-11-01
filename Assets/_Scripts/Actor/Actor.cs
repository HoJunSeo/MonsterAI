using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Actor 에는 AI 와 Character
// AI는 구동체와 override(실제행동) 가 있다.
public class Actor : BaseObject
{
	bool _IsPlayer = false;
	public bool IsPlayer
	{
		get { return _IsPlayer; }
		set { _IsPlayer = value; }
	}

	[SerializeField]
	eTeamType _TeamType;
	public eTeamType TeamType
	{
		get { return _TeamType; }
	}

	[SerializeField]
	string TemplateKey = string.Empty;  // string TemplateKey 초기화

	GameCharacter _SelfCharacter = null;
	public GameCharacter SelfCharacter
	{ get { return _SelfCharacter; } }

	// AI
	[SerializeField]
	eAIType _AIType;
	public eAIType AIType
	{
		get { return _AIType; }
	}

	BaseAI _AI = null;
	public BaseAI AI
	{ get { return _AI; }	}

    [SerializeField]
    bool bEnableBoard = true;

	BaseObject HitTarget;	// 공격대상

	private void Awake()
	{
		switch (AIType)
		{
			case eAIType.NormalAI:
				{
					GameObject go =
						new GameObject(
							AIType.ToString(),
							typeof(NormalAI)
						);

					go.transform.SetParent(SelfTransform);
					_AI = go.GetComponent<NormalAI>();
                }
				break;
		}

		AI.TargetComponent = this;

		GameCharacter character =
			CharacterManager.Instance.AddCharacter(TemplateKey);
		character.TargetComponenet = this;
		_SelfCharacter = character;

        if(bEnableBoard)
        {
            BaseBoard board = BoardManager.Instance.AddBoard(this, eBoardType.BOARD_HP);

            board.SetData(ConstValue.SetData_HP, GetStatusData(eStatusData.MAX_HP), SelfCharacter.CURRENT_HP);
        }
        // 엑터 추가
		ActorManager.Instance.AddActor(this);
	}

	protected virtual void Update()
	{
		AI.UpdateAI();
		if (AI.END)
			Destroy(SelfObject);
	}

	public double GetStatusData(eStatusData statusData) // enum 타입의 statusData 가져오기
	{
		return SelfCharacter.
			GetCharacterStatus.GetStatusData(statusData);
	}

	public override object GetData(string keyData, params object[] datas)
	{
		switch (keyData)    // 필요한 KeyData 값 찾아오기
		{
			case ConstValue.ActorData_Team:
				{
					return TeamType;
				}
			case ConstValue.ActorData_Character:
				{
					return SelfCharacter;
				}
			case ConstValue.ActorData_GetTarget:
				{
					return HitTarget;
				}
            case ConstValue.ActorData_SkillData:
                {
                    // RANGE 스킬 사용하는 곳
                    // 조금 더 먼거리에서 레인지 스킬을 사용
                    int index = (int)datas[0];
                    return SelfCharacter.GetSkillDataByIndex(index);
                }

			default:
				return base.GetData(keyData, datas);
		}
	}

	public override void ThrowEvent(string keyData, params object[] datas)
	{
		switch (keyData)
		{
			case ConstValue.ActorData_SetTarget:
				{
					HitTarget = datas[0] as BaseObject; // HitTarget 은 BaseObject의 0번째 datas를 실행
				}
				break;
                // 콘스트밸류의 이벤트키_힛 찾아오기
			case ConstValue.EventKey_Hit:
				{
                    if (ObjectState == eBaseObjectState.STATE_DIE)
                        return; 

                    GameCharacter casterCharacter = datas[0] as GameCharacter;  // casterCharacter를 게임캐릭터의 datas[0]번째로 설정
                    SkillTemplate skillTemplate = datas[1] as SkillTemplate;    // skillTemplate 를 스킬템플릿의 datas[1]번째로 설정

                    // caseterCharacter 에 데이터 추가 : SKILL이란 키값을 STATUS_DATA에 추가.
                    casterCharacter.GetCharacterStatus.AddStatusData("SKILL",skillTemplate.STATUS_DATA);

                    // attackDamage 의 값에 enum 타입 eStatusData의 어택값을 저장
                    double attackDamage = casterCharacter.GetCharacterStatus.GetStatusData(eStatusData.ATTACK);

                    casterCharacter.GetCharacterStatus.RemoveStatusData("SKILL");   // SKILL 키값 삭제

                    SelfCharacter.IncreaseCurrentHP(-attackDamage); // HP를 어택데미지만큼 빼준다

                    //Debug.Log(SelfObject.name + " 가 데미지 " + attackDamage + " 피해를 입었습니다.");

                    // DamageBoard
                    BaseBoard board = BoardManager.Instance.AddBoard(this, eBoardType.BOARD_DAMAGE);
                    if (board != null)
                        board.SetData(ConstValue.SetData_Damage, attackDamage);

                    board = BoardManager.Instance.GetBoardData(this, eBoardType.BOARD_HP);
                    if(board != null)
                    {
                        board.SetData(ConstValue.SetData_HP, GetStatusData(eStatusData.MAX_HP), SelfCharacter.CURRENT_HP);
                    }

                    // 카메라 진동 코루틴 실행체크
                    if (IsPlayer == true)
                    {
                        CameraManager.Instance.Shake();
                    }

                    // 죽었나 안죽었나 검사
                    if(ObjectState == eBaseObjectState.STATE_DIE)
                    {
                        Debug.Log(gameObject.name + " 죽음!");
                        GameManager.Instance.KillCheck(this);
                    }

                    AI.ANIMATOR.SetInteger("Hit", 1); // 애니메이션을 Hit키값의 1번으로 변경
				}
				break;
            case ConstValue.EventKey_SelectSkill:   // 스킬 선택
                {
                    int index = (int)datas[0];
                    if(SelfCharacter.EquipSkillByIndex(index) == false)
                    {
                        Debug.LogError(this.gameObject + " 의 " + " Skill Index : " + index + " 스킬 구동 실패");
                    }
                }
                break;

			default:
				base.ThrowEvent(keyData, datas);
			break;
		}
	}

	public void RunSkill()
	{
        SkillData selectSkill = SelfCharacter.SELECT_SKILL; // 스킬 설정

        if (selectSkill == null)
            return;

        // 스킬리스트의 카운트만큼 스킬 선택 포문 실행
        for(int i = 0; i < selectSkill.SKILL_LIST.Count; i++)
        {
            SkillManager.Instance.RunSkill(this, selectSkill.SKILL_LIST[i]);
        }

        SelfCharacter.SELECT_SKILL = null;  // 선택된 스킬 다시 초기화
	}


	public virtual void OnDestroy()
	{
        if (BoardManager.Instance != null)
            BoardManager.Instance.ClearBoard(this);

		// ActorManager RemoveActor
		if(ActorManager.Instance != null)   
			ActorManager.Instance.RemoveActor(this);
	}

    public virtual void OnDisable()
    {
        if (BoardManager.Instance != null)
            if(GameManager.Instance.GAME_OVER == false)
                 BoardManager.Instance.ShowBoard(this, false);
    }

    public virtual void OnEnable()
    {
        if (BoardManager.Instance != null)
            BoardManager.Instance.ShowBoard(this, true);
    }
    //Animator Anim;
    //protected eAIStateType CurrentState;

    //void Awake ()
    //{
    //	Anim = this.GetComponentInChildren<Animator>();
    //	if(Anim == null)
    //	{
    //		Debug.Log("Animator is null");
    //		return;
    //	}

    //	ChangeState(eAIStateType.AI_STATE_IDLE);
    //}

    //void SetAnimtion(eAIStateType eAIState)
    //{
    //	Anim.SetInteger("State", (int)eAIState);
    //}

    //public void ChangeState(eAIStateType state)
    //{
    //	if (CurrentState == state)
    //		return;

    //	CurrentState = state;

    //	switch (CurrentState)
    //	{
    //		case eAIStateType.AI_STATE_IDLE:
    //			break;
    //		case eAIStateType.AI_STATE_ATTACK:
    //			{

    //			}
    //			break;
    //		case eAIStateType.AI_STATE_MOVE:
    //			break;
    //		case eAIStateType.AI_STATE_DIE:
    //			break;
    //	}


    //	SetAnimtion(CurrentState);
    //}

    //protected virtual void Update()
    //{

    //}

    //public override object GetData(string keyData, params object[] datas)
    //{
    //	return base.GetData(keyData, datas);
    //}

    //public override void ThrowEvent(string keyData, params object[] datas)
    //{
    //	switch(keyData)
    //	{
    //		case ConstValue.EventKey_Hit:
    //			{
    //				// Die
    //				Destroy(SelfObject);
    //			}
    //			break;

    //		default:
    //			{
    //				base.ThrowEvent(keyData, datas);
    //			}
    //			break;
    //	}

    //}
}