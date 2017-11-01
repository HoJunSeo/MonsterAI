using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AnimationTest : MonoBehaviour
{

    List<GameObject> TestList = new List<GameObject>();

    public Animator Anim;
    //public MonsterState MState = MonsterState.MState_None;
    float _MoveSpeed = 2f;

    GameObject Target;
    public GameObject TestPrefab;
    public int _MaxObjectNum;

    Vector3 TransPosition;

    NavMeshAgent _Navmesh;

    private void Awake()
    {
        _Navmesh = GetComponent<NavMeshAgent>();

        if(TestPrefab == null)
        {
            TestLoad();
        }
    }

    private void Start()
    {
        MonsterLoad();
    }

    private void Update()
    {
        //_Navmesh.speed = 50;
        //_Navmesh.acceleration = 10f;
        //_Navmesh.SetDestination(new Vector3(5,0,0));
        //if (Anim != null)
        //{
        //    Anim.SetInteger("STATE", (int)MState);
        //}
        MoveTogether();
    }

    void TestLoad()
    {
        for(int i = 0; i < 4; i++)
        {
            TestPrefab = Resources.Load("Prefabs/_Actor/" + "Enemy_1") as GameObject;

            TestList.Add(TestPrefab);
        }
    }

    void MonsterLoad()
    {
        TransPosition = new Vector3(0, 0.5f, 0);

        for (int i = 0; i < _MaxObjectNum; i++)
        {
            Instantiate(TestPrefab, TransPosition + RandomPos(), Quaternion.identity);
        }
    }

    Vector3 RandomPos()
    {
        Vector3 pos =  new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        return pos.normalized * Random.Range(1, 10);
    }

    void MoveTogether()
    {
        transform.position = Vector3.forward;
    }
    

}
