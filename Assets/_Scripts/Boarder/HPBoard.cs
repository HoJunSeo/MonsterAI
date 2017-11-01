using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBoard : BaseBoard {

    [SerializeField]
    UIProgressBar ProgressBar = null;

    [SerializeField]
    UILabel Label;

    public override eBoardType BOARD_TYPE
    {
        get
        {
            return eBoardType.BOARD_HP;
        }
    }

    public override void SetData(string strKey, params object[] datas)
    {
        if(strKey.Equals(ConstValue.SetData_HP))
        {
            double maxHP = (double)datas[0];    // SetData_HP의 0번째 데이터로 maxHP 관리
            double curHP = (double)datas[1];    // SetData_HP의 1번째 데이터로 curHP 관리

            ProgressBar.value = (float)(curHP / maxHP);
            Label.text = curHP.ToString() + " / " + maxHP.ToString();
        }
        else
            base.SetData(strKey, datas);
    }
}
