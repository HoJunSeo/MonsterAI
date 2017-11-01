using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITools : MonoSingleton<UITools>
{
    Dictionary<eUIType, BaseObject> DicUI = new Dictionary<eUIType, BaseObject>();

    // DontDestroy 전용
    GameObject SubUIRoot = null;
    Dictionary<eUIType, BaseObject> DicSubUI = new Dictionary<eUIType, BaseObject>();

    Camera _UICamera = null;
    Camera UI_Camera
    {
        get
        {
            if(_UICamera == null)
            {
                // 인덱스가 가지고 있는 이름을 반환해준다 ( 매번 숫자를 넣어주기 힘드므로 네임을 통해 카메라를 자동적으로 찾아온다 )
                _UICamera = NGUITools.FindCameraForLayer(
                    LayerMask.NameToLayer("UI")
                    );
                //_UICamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
            }
            return _UICamera;
        }
    }

    // BaseObject 반환형
    BaseObject GetUI(eUIType uIType, bool isDontDestroy = false)
    {
        if(isDontDestroy == false)
        {
            if (DicUI.ContainsKey(uIType) == true)
                return DicUI[uIType];
        }
        else
        {
            if (DicSubUI.ContainsKey(uIType) == true)
                return DicSubUI[uIType];
        }

        GameObject makeUI = null;
        BaseObject baseObject = null;
        GameObject prefabUI = Resources.Load("Prefabs/UI/" + uIType.ToString()) as GameObject;

        // makeUI 를 딕셔너리에 매번 저장해준다.
        if(prefabUI != null)
        {
            if(isDontDestroy == false)
            {
                // UICamera Child
                // Instantiate() -> Initialize -> Parent
                makeUI = NGUITools.AddChild(UI_Camera.gameObject, prefabUI);

                baseObject = makeUI.GetComponent<BaseObject>();
                if (baseObject == null)
                {
                    Debug.Log(uIType.ToString() + " 오브젝트에 " + "BaseObject가 연결되어 있지 않습니다.");
                    baseObject = makeUI.AddComponent<BaseObject>();
                }

                DicUI.Add(uIType, baseObject);
            }
            else
            {
                // SubRoot Child
                if(SubUIRoot == null)
                {
                    SubRootCreate();
                }

                makeUI = NGUITools.AddChild(SubUIRoot, prefabUI);

                baseObject = makeUI.GetComponent<BaseObject>();
                if (baseObject == null)
                {
                    Debug.Log(uIType.ToString() + " 오브젝트에 " + "BaseObject가 연결되어 있지 않습니다.");
                    baseObject = makeUI.AddComponent<BaseObject>();
                }

                // 로딩바가 신전환이 되도 사라지지 않게 만들어준다.
                DicSubUI.Add(uIType, baseObject);

            }
        }
        return baseObject;
    }

    // baseobject 형으로 반환 필요
    public BaseObject ShowUI(eUIType uIType, bool isSub = false)
    {
        // 꺼져 있는놈을 켜주겠다.
        BaseObject uiObject = GetUI(uIType, isSub);
        if(uiObject != null && uiObject.SelfObject.activeSelf == false)
        {
            uiObject.SelfObject.SetActive(true);
        }

        return uiObject;
    }

    public void HideUI(eUIType uIType, bool isSub = false)
    {
        // 켜져 있는놈만 끄겠다.
        BaseObject uiObject = GetUI(uIType, isSub);
        if (uiObject != null && uiObject.SelfObject.activeSelf == true)
        {
            uiObject.SelfObject.SetActive(false);
        }
    }

    // 로딩바 관련
    public void ShowLoadingUI(float value)
    {
        BaseObject loadingUI = GetUI(eUIType.Pf_UI_Loading, true);

        if (loadingUI == null)
            return;

        if (loadingUI.gameObject.activeSelf == false)
            loadingUI.gameObject.SetActive(true);

        loadingUI.ThrowEvent("LoadingValue", value);
    }

    public void Clear()
    {
        foreach (KeyValuePair<eUIType,BaseObject> pair in DicUI)
        {
            Destroy(pair.Value.gameObject);
        }

        DicUI.Clear();
    }

    public void SubRootCreate()
    {
        // SubUIRoot 가 없다면 생성
        if(SubUIRoot == null)
        {
        GameObject subRoot = new GameObject();
        subRoot.transform.SetParent(this.transform);
            SubUIRoot = subRoot;

            SubUIRoot.layer = LayerMask.NameToLayer("UI");
        }

        // SubUIRoot 최신화
        UIRoot uIRoot = UI_Camera.GetComponentInParent<UIRoot>();

        SubUIRoot.transform.position = uIRoot.transform.position;
        SubUIRoot.transform.localScale = uIRoot.transform.localScale;
    }
}
