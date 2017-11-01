using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 함수에 대한 주소를 넘겨주고 그걸로 실행을 시키겠다.
public delegate void YesEvent();
public delegate void NoEvent();
// Function, Action

public class UI_Popup : BaseObject
{
    YesEvent Yes;
    NoEvent No;

    UILabel TitleLabel;
    UILabel ContentsLabel;

    UIButton YesButton;
    UIButton NoButton;

    private void Awake()
    {
        TitleLabel = GetChild("Title").GetComponent<UILabel>();
        ContentsLabel = GetChild("Contents").GetComponent<UILabel>();

        YesButton = GetChild("YesButton").GetComponent<UIButton>();
        NoButton = GetChild("NoButton").GetComponent<UIButton>();

        EventDelegate.Add(YesButton.onClick, new EventDelegate(this, "OnClickedYesButton"));
        EventDelegate.Add(NoButton.onClick, new EventDelegate(this, "OnClickedNoButton"));
    }

    public void Set(YesEvent yes, NoEvent no, string title, string contents)
    {
        Yes = yes;
        No = no;
        TitleLabel.text = title;
        ContentsLabel.text = contents;
    }

    public void OnClickedYesButton()
    {
        if (Yes != null)
            Yes();
    }

    public void OnClickedNoButton()
    {
        if (No != null)
            No();
    }
}
