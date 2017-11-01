using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//public enum MonsterState
//{
//    MState_None,
//    MState_Idle,
//    MState_Fastmove,
//    MState_Slowmove,
//    MState_Attack,
//}

//public enum _MiddleMonster
//{
//    MoveToOrigin,
//    MoveToNearTarget,
//}

//public enum _MonsterSkill
//{
//    None,
//    Smite,
//    Spear,
//    Dash,
//}
//[RequireComponent(typeof(NavMeshAgent))]
public class Monster_1 : MonoBehaviour
{
//    // 에너미 저장
//    List<GameObject> Enemy_N = new List<GameObject>();
//    // 스킬프리팹 저장
//    List<GameObject> SkillList = new List<GameObject>();

//    // 스킬 enum 초깃값
//    public _MonsterSkill monsterSkill = _MonsterSkill.None;
//    // Monster 애니메이션 초깃값 지정
//    MonsterState monsterState = MonsterState.MState_None;
//    // 스킬 프리팹 저장할 변수
//    GameObject Prefabs;

//    NavMeshAgent _NavMesh;

//    //==============
//    // 거리 비교 변수
//    float Dist;
//    float Radius = 10f;
//    //속도 변수
//    float MoveSpeed;
//    //==============
//    Vector3 OriginPos;

//    // Player를 타겟으로 하는 검사 
//    GameObject Target_A = null;

//    // =============================
//    // 스마이트 스킬 필요 변수
//    float Timer;
//    float WaitingTime = 2;
//    // =============================
//    // 애니메이션
//    public Animator Anim;
//    //===============================
//    bool IsMeet;
//    // 스킬 로드 관련 debug 확인 변수
//    bool IsLoad;
//    bool IsReady;
//    bool IsSkillOver;

//    public GameObject[] TargetEnemy;

//    private void Awake()
//    {
//        // 초기 위치 기억
//        OriginPos = transform.position;
//        //====================================
//        // 스킬 로드
//        if (SkillList.Count == 0)
//        {
//            SkillPrefabLoad();
//        }
//        if (IsLoad == true)
//            Debug.Log("스킬 프리팹 로드 성공");
//        //=====================================
//        // NavMesh
//        _NavMesh = GetComponent<NavMeshAgent>();
//    }

//    private void Start()
//    {
//        foreach (GameObject item in TargetEnemy)
//        {
//            Enemy_N.Add(item);
//        }
//    }

//    private void Update()
//    {
//        if (Target_A == null)
//        {
//            FindNearTarget();
//            return;
//        }
//        MoveFromOurTeam();
//        //MonsterSkill();
//    }

//    // 보스 스킬 관리
//    void MonsterSkill()
//    {
//        // 제한 추가

//        switch (monsterSkill)
//        {
//            case _MonsterSkill.Smite:
//                {
//                    GroundSmiteSkill();
//                }
//                break;
//            case _MonsterSkill.Spear:
//                {
//                    SpearSkill();
//                }
//                break;
//            case _MonsterSkill.Dash:
//                {
//                    Dash();
//                }
//                break;
//        }
//    }

//    void SkillPrefabLoad()
//    {
//        Prefabs = Resources.Load("Test/Weapon") as GameObject;

//        if (Prefabs == null)
//        {
//            Debug.Log(Prefabs.name + " 로드 실패");
//        }

//        SkillList.Add(Prefabs);

//        IsLoad = true;
//    }

//    void MoveFromOurTeam()
//    {
//        if (Target_A != null)
//        {
//            Dist = Vector3.Distance(transform.position, Target_A.transform.position);
//            // 이동
//            if (Dist > 1.5f)
//            {
//                NavMeshMove (Target_A.transform.position, 4);
//                //transform.LookAt(Target_A.transform);
//                //MoveSpeed = 4f;
//                //transform.position = Vector3.MoveTowards(transform.position, Target_A.transform.position, MoveSpeed * Time.deltaTime);
//                FastMove();
//            }
//            else
//            {
//                NavMeshMove(OriginPos, 2);
//                //transform.LookAt(OriginPos);
//                //MoveSpeed = 2f;
//                //transform.position = Vector3.MoveTowards(transform.position, OriginPos, MoveSpeed * Time.deltaTime);
//                Target_A.transform.position = Vector3.MoveTowards(Target_A.transform.position, transform.position, MoveSpeed * Time.deltaTime);
//                SlowMove();
//            }
//        }
//    }

//    void FindNearTarget()
//    {
//        foreach (GameObject NearTarget in Enemy_N)
//        {
//            Dist = Vector3.Distance(transform.position, NearTarget.transform.position);

//            if (Dist <= Radius)
//            {
//                Target_A = NearTarget;
//                Debug.Log(Target_A.name + "찾았음!");
//                break;
//            }
//        }
//    }

//    void GroundSmiteSkill()
//    {
//        //  && 스킬이 실행되었을때를 추가 해준다.
//        // 시간이나 hp로
//        if (Target_A != null)
//        {
//            transform.LookAt(Target_A.transform);
//            Debug.Log("머리 돌림!");

//            // 시작 시 공격 상태라면 IsReady를 true로 만들어주고 바로 밑에 
//            //  if (IsReady == true) 로 진입한다.
//            IsReady = true;

//            // 다 종료가 됐을때
//            //if (IsSkillOver == true)
//            //{
//            //    GameObject.Find("Weapon").GetComponent<BoxCollider>().size = new Vector3(0.055f, 0.681f, 0.054f);
//            //    Debug.Log("스킬 종료!");
//            //}

//            if (IsReady == true)
//            {
//                Timer += Time.deltaTime;
//                // 애니메이션 실행
//                Attack();
//                // 시간체크를 통해 스킬실행이 3초라고 가정하고

//                // 2초후에 콜라이더 사이즈를 증가
//                if (Timer > WaitingTime)
//                {
//                    GameObject.Find("Weapon").GetComponent<BoxCollider>().size = new Vector3(2, 0.5f, 2);
//                    Debug.Log("콜라이더 크기 증가 성공!");
//                    Timer = 0;

//                    IsSkillOver = true;
//                }
//                // 스킬이 종료되면 콜라이더 사이즈를 원래대로 변환
//            }

//        }
//    }

//    // 타겟과의 거리가 20이상 멀어지면 타겟의 방향으로 뛴다.
//    // 단, 타겟과의 벌어진 거리 만큼이 아니라 해당 방향으로 30의 거리를 뛰는 식으로 한다.
//    // 뛸때 애니메이션의 속도를 증가 시켜주고, 콜라이더의 크기를 x축으로 3배 만큼 늘려
//    // 피하지 못하고 충돌시 데미지를 입게 만들어준다.
//    void Dash()
//    {
//        if (Target_A != null)
//        {
//            Dist = Vector3.Distance(transform.position, Target_A.transform.position);

//            if (Dist > 20)
//            {
//                NavMeshMove(Vector3.forward, 5, 10);

//                //transform.LookAt(Target_A.transform);
//                //_NavMesh.speed = 30;
//                //_NavMesh.acceleration = 10;
//                //_NavMesh.SetDestination(Vector3.forward * MoveSpeed);

//                FastMove();
//                gameObject.GetComponent<BoxCollider>().size = new Vector3(2.5f, 1, 2.5f);
//            }
//        }
//    }

//    void SpearSkill()
//    {
//        for (int i = 0; i < 5; i++)
//        {
//            Vector3 skillPosition = new Vector3(Random.Range(-3, 3), -2, Random.Range(-3, 3));

//            Prefabs = Instantiate(SkillList[0], skillPosition, Quaternion.identity);
//        }
//    }


//    void NavMeshMove(Vector3 destPos, float speed = 30f, float accel = 10f)
//    {
//        _NavMesh.speed = speed;
//        _NavMesh.acceleration = accel;
//        _NavMesh.SetDestination(destPos);
//    }


//    //========================================
//    // 애니메이션

//    void SetAnimation(int animState) // -> Enum
//    {
//        Anim.SetInteger("STATE", animState);
//    }

//    void Idle()
//    {
//        Anim.SetInteger("STATE", 1);
//    }

//    void FastMove()
//    {
//        Anim.SetInteger("STATE", 2);
//    }

//    void SlowMove()
//    {
//        Anim.SetInteger("STATE", 3);
//    }

//    void Attack()
//    {
//        Anim.SetInteger("STATE", 4);
//    }
//    // =========================================


}
