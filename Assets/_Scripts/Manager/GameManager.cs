using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
	bool IsInit = false;
	public Actor PlayerActor;

    bool IsGameOver = true;
    public bool GAME_OVER { get { return IsGameOver; } }

    public int SelectStage = 0;
    StageInfo SelectStageInfo = null;

    float StackTime = 0.0f;
    int KillCount = 0;

	//void Start ()
	//{
	//	GameInit();
	//	LoadGame();
	//}

	public void GameInit()
	{
		if (IsInit == true)
			return;

        // 스테이지 매니저의 스테이지 이닛 실행
        StageManager.Instance.StageInit();

        ItemManager.Instance.ItemInit();


		IsInit = true;
	}

	public void LoadGame()
	{
        // Init
        StackTime = 0.0f;
        KillCount = 0;
        IsGameOver = false;

        // Stage
        SelectStageInfo = StageManager.Instance.LoadStage(SelectStage);

		// Player 
		// Get actor
		PlayerActor = ActorManager.Instance.PlayerLoad();

        // Item
        foreach(KeyValuePair<eSlotType, ItemInstance>pair in ItemManager.Instance.DIC_EQUIP)
        {
            StatusData status = pair.Value.ITEM_INFO.STATUS;
            // 장비의 능력치를 더해주는 작업
            PlayerActor.SelfCharacter.GetCharacterStatus.AddStatusData(pair.Key.ToString(), status);
        }

        // 장비 장착시 늘어난 HP를 채워주는 작업
        PlayerActor.SelfCharacter.IncreaseCurrentHP(99999999999);
        
        // 체력바 최신화
        BaseBoard hpBoard = BoardManager.Instance.GetBoardData(PlayerActor, eBoardType.BOARD_HP);
        if(hpBoard != null)
        {
            hpBoard.SetData(ConstValue.SetData_HP, PlayerActor.GetStatusData(eStatusData.MAX_HP), PlayerActor.SelfCharacter.CURRENT_HP);
        }


        if(SelectStageInfo.CLEAR_TYPE == eClearType.CLEAR_TIME)
        {
            // 시간 체크
            UIManager.Instance.SetText(false, (float)SelectStageInfo.CLEAR_FINISH - StackTime);
        }
        else
        {
            // 킬 체크
            UIManager.Instance.SetText(true, (float)SelectStageInfo.CLEAR_FINISH - KillCount);
        }

		// Camera Setting
		CameraManager.Instance.CameraInit(PlayerActor);

	}

    private void Update()
    {
        if (IsGameOver == true)
            return;

        if (Scene_Manager.Instance.CURRENT_SCENE != eSceneType.Scene_Game)
            return;

        if (SelectStageInfo.CLEAR_TYPE == eClearType.CLEAR_TIME)
        {
            UIManager.Instance.SetText(false, (float)SelectStageInfo.CLEAR_FINISH - StackTime);

            StackTime += Time.deltaTime;
            if(SelectStageInfo.CLEAR_FINISH < StackTime)
            {
                SetGameOver();
            }
        }
    }

    public void KillCheck(Actor dieActor)
    {
        if (IsGameOver == true)
            return;

        if (Scene_Manager.Instance.CURRENT_SCENE != eSceneType.Scene_Game)
            return;

        if (SelectStageInfo.CLEAR_TYPE != eClearType.CLEAR_KILLCOUNT)
            return;

        if (PlayerActor.TeamType == dieActor.TeamType)
            return;

        KillCount++;
        UIManager.Instance.SetText(true, (float)SelectStageInfo.CLEAR_FINISH - KillCount);

        if (SelectStageInfo.CLEAR_FINISH <= KillCount)
            SetGameOver();
    }

    public void SetGameOver()
    {
        IsGameOver = true;          // 아무 처리도 할 수 없게.
        Time.timeScale = 0.1f;      // timeScale는 시간에 영향을 주기때문에 +가 되면 시간이 흘러가는 것이 느려진다.
        Debug.Log("GameOver");
        Invoke("GoLobby", 0.5f);    // Invoke 는 예약 작업으로 0.5초 후로 설정되어있다.
    }

    void GoLobby()
    {
        Time.timeScale = 1f;
        Scene_Manager.Instance.LoadScene(eSceneType.Scene_Lobby);
    }

}
