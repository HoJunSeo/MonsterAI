using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TestPlayerState
{
    OriginToEnemy,
    EnemyToOrigin,
}
public class TestPlayer : MonoBehaviour
{

    public TestEnemy testEnemy = null;

    // NextState 가 실행되면 초기 설정된 값에 먼저 들어와서 실행을 해주고, 
    // 다시 들어올때마다 그 아래의 case를 검색해서 돌려준다. 
    public TestPlayerState State = TestPlayerState.EnemyToOrigin;
    TestEnemy[] EnemyArray;

    Vector3 OriginPos;
    Vector3 DestPos= Vector3.zero;

    float Speed = 1f;

    // Use this for initialization
    void Start()
    {
        OriginPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (testEnemy == null)
        {
            Search();
            return;
        }

        Move();
    }

    void Search()
    {
        // 스크립트가 달려있는 type을 전부 찾아온다
        EnemyArray = GameObject.FindObjectsOfType<TestEnemy>();
        if (EnemyArray == null)
            return;
        
        // 기준점을 잡아준다
        Transform NearEenemy = EnemyArray[0].transform;
        // 기준점과의 거리 비교
        float NearDist = Vector3.Distance(transform.position, NearEenemy.position);

        // 가장 가까운 적을 찾기위해 FOR 사용
        for (int i = 1; i < EnemyArray.Length; i++)
        {
            Transform Enemy = EnemyArray[i].transform;
            float dist = Vector3.Distance(transform.position, Enemy.position);
            if(NearDist > dist)
            {
                // 가장 가까운 적과 거리를 최신화
                NearEenemy = Enemy;
                NearDist = dist;
            }
        }

        testEnemy = NearEenemy.GetComponent<TestEnemy>();

        DestPos = NearEenemy.position;
        // search가 끝나고 이 상태를 State의 OriginToEnemy의 상태로 바꿔준다.
        // State = TestPlayerState.OriginToEnemy;
    }

    void Move()
    {
        // transform.position = Vector3.MoveTowards(transform.position, DestPos, Speed * Time.deltaTime);

        // 목적지까지의 방향과 크기(거리)
        Vector3 dir = DestPos - transform.position;

        // 이번 프레임의 이동량
        Vector3 Movedir = dir.normalized * Speed * Time.deltaTime;

        if(Movedir.magnitude < dir.magnitude)
            transform.position += Movedir;
        else
        {
            // 목적지 도달
            transform.position = DestPos;
            // 다음 목적지 설정
            NextState();
        }

        // if(dir.magnitude < 1.5f)

        //if(dir.sqrMagnitude < (1.5f * 1.5f))
        //{
        //    Debug.Log("목적지 도달");
        //    DestPos = OriginPos;
        //}
    }

    void NextState()
    {
        switch (State)
        {
            case TestPlayerState.OriginToEnemy:
                {
                    // 오리진에서 에너미에 도달
                    // 들어와서 Dest에 OriginPos를 넣어주기 때문에
                    // Move() 로직의 Dest가 최신화 되서 복귀가 실행된다.
                    DestPos = OriginPos;    // 오리진 복귀
                    // 들어와서 실행해주고 나갈때 State를 EnemyToOrigin로 최신화
                    // 다음번 NextState가 실행될때 EnemyToOrigin으로 들어가 실행
                    State = TestPlayerState.EnemyToOrigin;
                }
                break;
            case TestPlayerState.EnemyToOrigin:
                {
                    Search();
                    State = TestPlayerState.OriginToEnemy;
                }
                break;
        }
    }
}
