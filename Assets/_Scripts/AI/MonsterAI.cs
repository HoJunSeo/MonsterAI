using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// state를 통해 idle / dash 등등을 관리해주고,
/// 현재 state를 종료해주고 싶을때 changestate 를 idle 상태로 변경해주는 방식으로
/// bool값이 필요가 없어진다.
/// 
/// </summary>

public class MonsterAI : BaseAI
{
    protected override IEnumerator Idle()
    {





        return base.Idle();
    }

}
