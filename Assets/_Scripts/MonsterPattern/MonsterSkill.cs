using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

///// <summary>
///// state를 통해 idle / dash 등등을 관리해주고,
///// 현재 state를 종료해주고 싶을때 changestate 를 idle 상태로 변경해주는 방식으로
///// bool값이 필요가 없어진다.
///// 
///// </summary>

public enum MonsterState
{
    MState_Idle,
    MState_Move,
    MState_Attack,
    MState_Damage,
    MState_Dash,
    MState_Smite,
    MState_WeaponWall,
}

//public enum ChangeState
//{
//    CState_Idle,
//    CState_Move,
//    CState_Attack,
//    CState_Damage,
//    CState_Dash,
//    CState_Smite,
//    CState_WeaponWall,
//}

//public enum _MoveState
//{
//    MoveToOrigin,
//    MoveToEnemy,
//}

//public enum _BossSkill
//{
//    None,
//    Smite,
//    Dash,
//    WeaponWall,
//}

public class MonsterSkill : MonoBehaviour
{
    public MonsterState monsterState = MonsterState.MState_Idle;


    //ChangeState changeState = ChangeState.CState_Idle;
    //_MoveState moveState = _MoveState.MoveToOrigin;
    //_BossSkill monsterSkill = _BossSkill.None;

    List<GameObject> SkillList = new List<GameObject>();
    GameObject Prefabs;

    Vector3 OriginPos;

    public TestEnemy TargetEnemy = null;

    TestEnemy[] NearTarget;

    float Timer;

    Vector3 Destpos = Vector3.zero;
    //==================================
    // Skill 관련 불값
    // Dash
    bool DashOver;
    // Smite
    bool SmiteOver;
    // WeaponWall
    bool WeaponWallOver;
    // 공격
    bool IsAttack = true;
    //==================================
    bool IsLoad;

    NavMeshAgent _NavMesh;
    public Transform skillcollider = null;

    // 애니메이션
    Animator Anim = null;

    public Animator ANIMATOR
    {
        get
        {
            if (Anim == null)
            {
                Anim = gameObject.GetComponentInChildren<Animator>();
            }
            return Anim;
        }
    }

    private void Awake()
    {
        OriginPos = transform.position;

        _NavMesh = GetComponent<NavMeshAgent>();

        //====================================
        // 스킬 로드
        if (SkillList.Count == 0)
        {
            SkillPrefabLoad();
        }
        if (IsLoad == true)
            Debug.Log("스킬 프리팹 로드 성공");
        //=====================================
    }

    private void Update()
    {

    }

    //void _MonsterState()
    //{
    //    switch (monsterState)
    //    {
    //        case MonsterState.MState_Idle:
    //            {
    //                if (TargetEnemy == null)
    //                {
    //                    FindNearTarget();
    //                }
    //                else
    //                {
    //                    monsterState = MonsterState.MState_Move;
    //                }
    //            }
    //            break;
    //        case MonsterState.MState_Move:
    //            {
    //                Move();
    //                // 애니메이션 변경
    //            }
    //            break;
    //        case MonsterState.MState_Attack:
    //            {
    //                while(IsAttack)
    //                {
    //                    // Player가 죽으면 return;

    //                    // 애니메이션 변경

    //                    // 보스피가 90과 같거나 작을때
    //                    IsDash = true;
    //                    monsterState = MonsterState.MState_Dash;
    //                    IsAttack = false;

    //                    // 보스피가 80과 같거나 작을때
    //                    monsterState = MonsterState.MState_Smite;
    //                    IsAttack = false;
    //                }       
    //            }
    //            break;
    //        case MonsterState.MState_Dash:
    //            {
    //                Dash();
    //            }
    //            break;
    //        case MonsterState.MState_Smite:
    //            {
    //                Smite();
    //            }
    //            break;
    //        case MonsterState.MState_WeaponWall:
    //            {
    //                SpearSkill();
    //            }
    //            break;
    //    }
    //}

    //-----------------------------------------------------

    void StateMachine()
    {
        switch (monsterState)
        {
            case MonsterState.MState_Idle:
                {
                    ChangeAnimation();
                    if (TargetEnemy == null)
                    {
                        if (FindNearTarget())
                            ChangeMonsterState(MonsterState.MState_Move);
                    }
                }
                break;
            case MonsterState.MState_Move:
                {
                    Move();
                    ChangeAnimation();
                }
                break;
            case MonsterState.MState_Attack:
                {
                    ProcessAttack();
                    ChangeAnimation();
                }
                break;
            case MonsterState.MState_Dash:
                {
                    DashOver = false;
                    Dash();
                    ChangeAnimation();
                }
                break;
            case MonsterState.MState_Smite:
                {
                    Smite();
                    ChangeAnimation();
                }
                break;
            case MonsterState.MState_WeaponWall:
                {
                    SpearSkill();
                    ChangeAnimation();
                }
                break;
        }
    }

    void ChangeMonsterState(MonsterState state)
    {
        // 반복을 제한한다.
        if (monsterState == state)
            return;

        monsterState = state;
        SetupAtChangeState(monsterState);
    }

    // 상태 돌입 직전 전처리
    void SetupAtChangeState(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.MState_Idle:
                break;
            case MonsterState.MState_Move:
                break;
            case MonsterState.MState_Attack:
                {
                    IsAttack = true;
                }
                break;
            case MonsterState.MState_Dash:
                {
                    IsAttack = false;
                    if (DashOver == true)
                        return;
                }
                break;
            case MonsterState.MState_Smite:
                {
                    IsAttack = false;
                    if (SmiteOver == true)
                        return;
                }
                break;
            case MonsterState.MState_WeaponWall:
                {
                    IsAttack = false;
                    if (WeaponWallOver == true)
                        return;
                }
                break;
        }
    }

    void ProcessAttack()
    {
        // 보스 체력이 90보다 작거나 같다
        ChangeMonsterState(MonsterState.MState_Dash);
        // 보스 체력이 80보다 작거나 같다
        ChangeMonsterState(MonsterState.MState_Smite);
        // 보스 체력이 70보다 작거나 같다
        ChangeMonsterState(MonsterState.MState_WeaponWall);
        // 보스 체력이 60보다 작거나 같다
        ChangeMonsterState(MonsterState.MState_Smite);
        // 보스 체력이 50보다 작거나 같다
        ChangeMonsterState(MonsterState.MState_WeaponWall);
        // 등등등
    }
    //-----------------------------------------------------


    //private void FixedUpdate()
    //{
    //    Dash();
    //    Smite();
    //    SpearSkill();
    //}

    bool FindNearTarget()
    {
        NearTarget = GameObject.FindObjectsOfType<TestEnemy>();
        if (NearTarget == null)
            return false;

        Transform NearEnemy = NearTarget[0].transform;

        float Neardist = Vector3.Distance(transform.position, NearEnemy.transform.position);

        for (int i = 1; i < NearTarget.Length; i++)
        {
            Transform Enemy = NearTarget[i].transform;
            float dist = Vector3.Distance(transform.position, Enemy.position);

            if (Neardist > dist)
            {
                NearEnemy = Enemy;
                Neardist = dist;
            }
        }

        TargetEnemy = NearEnemy.GetComponent<TestEnemy>();
        Destpos = NearEnemy.position;

        return true;
    }

    private void Move()
    {
        float dist = Vector3.Distance(TargetEnemy.transform.position, transform.position);

        if (dist > 1.5f)
        {
            NavMove(TargetEnemy.transform.position);
            //ChangeAnimation();
        }
        else
            ChangeMonsterState(MonsterState.MState_Attack);
        
    }

    //void MoveToOurTeam()
    //{
    //    //Vector3 dir = Destpos - transform.position;
    //    //Vector3 Movedir = dir.normalized * Speed * Time.deltaTime;

    //    //if (dir.magnitude > Movedir.magnitude)
    //    //{
    //    //    transform.position += Movedir;
    //    //}
    //    //else
    //    //{
    //    //    Destpos = OriginPos;

    //    //    NextState();
    //    //}

    //    float dist = Vector3.Distance(TargetEnemy.transform.position, transform.position);

    //    if (dist > 1.5f)
    //    {
    //        NavMove(TargetEnemy.transform.position);
    //    }
    //    else
    //    {

    //    }

    //}

    //void NextState()
    //{
    //    switch (moveState)
    //    {
    //        case _MoveState.MoveToOrigin:
    //            {
    //                Destpos = OriginPos;
    //                moveState = _MoveState.MoveToEnemy;
    //            }
    //            break;
    //        case _MoveState.MoveToEnemy:
    //            {
    //                FindNearTarget();
    //                moveState = _MoveState.MoveToOrigin;
    //            }
    //            break;
    //    }
    //}

    //====================================================================
    // 스킬 프리팹 로드
    void SkillPrefabLoad()
    {
        Prefabs = Resources.Load("Test/Weapon") as GameObject;

        if (Prefabs == null)
        {
            Debug.Log(Prefabs.name + " 로드 실패");
        }

        SkillList.Add(Prefabs);

        IsLoad = true;
    }
    //====================================================================
    // SpearSkill
    void SpearSkill()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 skillPosition = new Vector3(Random.Range(-3, 3), -2, Random.Range(-3, 3));

            Prefabs = Instantiate(SkillList[0], skillPosition, Quaternion.identity);
        }
        WeaponWallOver = true;
    }
    //====================================================================
    // Smite스킬
    void Smite()
    {
        transform.LookAt(TargetEnemy.transform);

        float Timer = 0;
        float WaitingTime = 3;
        Timer += Time.deltaTime;

        if (Timer > WaitingTime)
        {
            GameObject.Find("Weapon").GetComponent<BoxCollider>().size = new Vector3(2.5f, 0.5f, 2.5f);
        }
    }
    //====================================================================
    // Dash스킬
    void Dash()
    {
        float WaitingTime = 10;
        Timer += Time.deltaTime;

        float LimitDist = Vector3.Distance(TargetEnemy.transform.position, transform.position);

        if (LimitDist > 20)
        {
            transform.LookAt(TargetEnemy.transform);

            // 애니메이션 실행

            NavMove(TargetEnemy.transform.position, 30, 10);

            this.GetComponent<BoxCollider>().size = new Vector3(3.5f, 0.5f, 2);
        }

        if(WaitingTime < Timer)
        {
            DashOver = true;
            ChangeMonsterState(MonsterState.MState_Attack);
        }

        // 대쉬가 실행되고 
    }
    //====================================================================

    void NavMove(Vector3 position, float speed = 5f, float accel = 5f)
    {
        _NavMesh.SetDestination(position);
        _NavMesh.speed = speed;
        _NavMesh.acceleration = accel;
    }

    // ===================================================================
    // 애니메이션
    void ChangeAnimation()
    {
        ANIMATOR.SetInteger("STATE", (int)monsterState);
    }

    //====================================================================

    //void SkillOver()
    //{
    //    if (IsDash == true || IsSmite == true || IsWeaponWall == true)
    //        return;

    //    if(DashOver == true)
    //    {
    //        this.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
    //        return;
    //    }

    //    if(SmiteOver == true)
    //    {
    //        this.GetComponent<BoxCollider>().size = new Vector3(0.55f, 0.65f, 0.25f);
    //        return;
    //    }

    //    if(WeaponWallOver == true)
    //    {

    //    }
    //}
}