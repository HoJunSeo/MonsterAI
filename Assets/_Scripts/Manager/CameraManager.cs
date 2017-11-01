using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
///   # 고정 좌표
///  정해진 시간안에 이동처리를 완료하고 싶을때 Lerp 를 사용
///  Lerp(StartPos , EndPos, t )  t-> 백분율 ( 0% ~ 100% ) 0f ~ 1f
///  Sp + Dist * t        // 누적을 통해 목표위치에 도달 
///  ( 10% )              // 백분위를 누적시키기 위해 반복문 실행
/// 
///   # 이동 좌표 
///   Lerp(CurrPoint , EndPoint, t ( time.deltatime ) )  ex) t = 0.1
///   이동을 한후 이동한 위치를 CurrPoint로 재지정 
///   재지정 지점에서 다시 EndPoint로 0.1 이동을 하게 되면 초기보다
///   이동거리가 줄어 들게 된다 1.0 -> 0.9 -> 0.81 -> ...
///   
/// </summary>



public class CameraManager : MonoSingleton<CameraManager>
{
	public GameObject CameraRoot;
	public Transform Target;

    // 메인카메라 추가
    public Camera MainCamera;

	public float Distance = 10.0f;
	public float Height = 5.0f;

	public float HeightDamping = 2.0f; // 축소값 
	public float WidthDamping = 3.0f;

	public void CameraInit(Actor Player)
	{
        MainCamera = Camera.main;

        CameraRoot = GameObject.Find("CameraRoot");
		Target = Player.SelfTransform;
	}

	private void LateUpdate()
	{
		if (Target == null)
			return;

		float wantedHeight = Target.position.y + Height;
		float currentHeight = CameraRoot.transform.position.y;

		float wantedWidth = Target.position.x;
		float currentWidht = CameraRoot.transform.position.x;

        // Lerp를 사용한 선형보간 카메라가 이동할때 부드럽게 이동
        currentHeight = Mathf.Lerp(
			currentHeight, wantedHeight, 
			HeightDamping * Time.deltaTime);

		currentWidht = Mathf.Lerp(
			currentWidht, wantedWidth,
			WidthDamping * Time.deltaTime);

		Vector3 pos = Target.position;
		pos -= CameraRoot.transform.forward * Distance;

		CameraRoot.transform.position
			= new Vector3(currentWidht, currentHeight, pos.z);

		CameraRoot.transform.LookAt(Target);
	}

    // 코루틴 함수 추가
    public void Shake()
    {
        StartCoroutine(CameraShakeProcess(0.1f, 0.2f));
    }
    
    // 카메라 진동
    IEnumerator CameraShakeProcess(float shakeTime, float shakeSense)
    {
        float deltaTime = 0.0f;

        while(deltaTime < shakeTime)
        {
            deltaTime += Time.deltaTime;

            Vector3 pos = Vector3.zero;
            pos.x = Random.Range(-shakeSense, shakeSense);
            pos.y = Random.Range(-shakeSense, shakeSense);
            pos.z = Random.Range(-shakeSense, shakeSense);

            MainCamera.transform.localPosition = pos;


            yield return new WaitForEndOfFrame();
        }

        MainCamera.transform.localPosition = Vector3.zero;
    }

}
