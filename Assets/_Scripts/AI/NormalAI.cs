using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAI : BaseAI
{
	protected override IEnumerator Idle()
	{
        // 탐지 범위
        float distance = 0f;
        BaseObject targetObject =
            ActorManager.Instance.GetSearchEnemy(
                TargetComponent, out distance);

        if (targetObject != null)
        {

            // 공격 범위
            float attackRange = 1f;

            // 사용할 스킬을 0, 1 로 바꿔줄 수 있다.
            SkillData skillData = TargetComponent
                .GetData(ConstValue.ActorData_SkillData, 0) as SkillData;

            if (skillData != null)
            {
                attackRange = skillData.RANGE;
            }

            if (distance < attackRange)
            {
                Stop();
                AddNextAI(eAIStateType.AI_STATE_ATTACK,
                    targetObject);
            }
            else
            {
                AddNextAI(eAIStateType.AI_STATE_MOVE);
            }

        }

        //base.Idle() 는 코루틴실행 변수이기 때문에 yield return 을 써줘야 한다.
        yield return StartCoroutine(base.Idle());
	}

	protected override IEnumerator Move()
	{
        // 탐지 범위
        float distance = 0f;
        BaseObject targetObject =
            ActorManager.Instance.GetSearchEnemy(
                TargetComponent, out distance);

        if (targetObject != null)
        {
            // 공격 범위
            float attackRange = 1f;

            SkillData skillData = TargetComponent.GetData(ConstValue.ActorData_SkillData, 0) as SkillData;

            if (skillData != null)
            {
                attackRange = skillData.RANGE;
            }

            if (distance < attackRange)
            {
                Stop();
                AddNextAI(eAIStateType.AI_STATE_ATTACK,
                    targetObject);
            }
            else
            {
                SetMove(targetObject.SelfTransform.position);
            }

        }
        yield return StartCoroutine(base.Move());
	}

	protected override IEnumerator Attack()
	{
        yield return new WaitForEndOfFrame();

        while (IsAttack)
        {
            if (ObjectState == eBaseObjectState.STATE_DIE)
                break;

            yield return new WaitForEndOfFrame();
        }

        AddNextAI(eAIStateType.AI_STATE_IDLE);

        yield return StartCoroutine(base.Attack());
	}


	protected override IEnumerator Die()
	{
        END = true;
        yield return StartCoroutine(base.Die());
	}

}
