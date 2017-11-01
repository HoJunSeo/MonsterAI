using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoSingleton<Scene_Manager>
{
    // 비동기 방식
    bool IsAsyc = true;
    AsyncOperation Operation = null;

    eSceneType CurrentState = eSceneType.Scene_Logo;
    eSceneType NextState = eSceneType.Scene_None;
    float StackTime = 0.0f;
    public eSceneType CURRENT_SCENE
    {
        get { return CurrentState; }
    }

    public void LoadScene(eSceneType type, bool isAsyc = true)
    {
        if (CurrentState == type)
            return;

        NextState = type;
        IsAsyc = isAsyc;
    }

    private void Update()
    {
        if(Operation != null)
        {
            // 눈으로 보기 위한 TestCode
            StackTime += Time.deltaTime;

            //UITools.Instance.ShowLoadingUI(Operation.progress);
            //if (Operation.isDone == true)

            UITools.Instance.SubRootCreate();
            UITools.Instance.ShowLoadingUI(StackTime / 2f);
            if (Operation.isDone == true && StackTime>= 2.0f)
            {
                CurrentState = NextState;
                ComplateLoad(CurrentState);

                // 오퍼레이션 & 넥스트스테이트 다시 초기화
                Operation = null;
                NextState = eSceneType.Scene_None;
                UITools.Instance.HideUI(eUIType.Pf_UI_Loading, true);
            }
            else
                return;

        }

        if (CurrentState == eSceneType.Scene_None)
            return;

        if(NextState != eSceneType.Scene_None && CurrentState != NextState)
        {
            // 닫기 작업
            // 현재 신을 사용하지 않게 만들어준다.
            // 그 후 다음 신을 로드하게 넘어간다.
            DisableScene(CurrentState);

            // 신 전환
            if(IsAsyc)
            {
                // 비동기 방식 ( AsyncOperation 반환 )
                Operation = SceneManager.LoadSceneAsync(NextState.ToString());
                StackTime = 0.0f;
                // 로딩바 생성
                UITools.Instance.ShowLoadingUI(0.0f);
            }
            else
            {
                // 동기 방식
                SceneManager.LoadScene(NextState.ToString());
                CurrentState = NextState;
                NextState = eSceneType.Scene_None;
                // 신 로드를 다 끝내고 컴플리트 시켜준다.
                ComplateLoad(CurrentState);
            }
        }
    }

    // 각 씬에 대한 정리 ( 필요한 상황들을 넣어주면 된다. )
    void ComplateLoad(eSceneType type)
    {
        UITools.Instance.SubRootCreate();
        switch (type)
        {
            case eSceneType.Scene_None:
                break;
            case eSceneType.Scene_Logo:
                break;
            case eSceneType.Scene_Lobby:
                LobbyManager.Instance.LoadLobby();
                GameManager.Instance.GameInit();
                break;
            case eSceneType.Scene_Game:
                GameManager.Instance.LoadGame();
                break;
            default:
                break;
        }
    }

    // 각 씬에 대한 사용이 끝났을때 
    // Destroy 같은 개념
    void DisableScene(eSceneType type)
    {
        switch (type)
        {
            case eSceneType.Scene_None:
                break;
            case eSceneType.Scene_Logo:
                break;
            case eSceneType.Scene_Lobby:
                LobbyManager.Instance.DisableLobby();
                break;
            case eSceneType.Scene_Game:
                SkillManager.Instance.ClearSkill();
                break;
            default:
                break;
        }

        UITools.Instance.Clear();
    }
}
