using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;
using Object = UnityEngine.Object;

public class UI_Button : UI_Popup
{
    private Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, Object[]>();
    enum Buttons
    {
        //List of Buttons by Name
        PointButton
    }

    enum Texts
    {
        //List of Texts by Name
        PointText,
        ScoreText,
    }

    enum GameObjects
    {
        //List of GameObjects by Name
        TestObject,
    }

    enum Images
    {
        //List of Images by Name
        ItemIcon,
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        //GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }
}
