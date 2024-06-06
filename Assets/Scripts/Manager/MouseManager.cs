using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.Events;


//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3> { }

public class MouseManager : Singleton<MouseManager>
{

    public Texture2D point, doorway, attack, target, arrow;

    RaycastHit hitInfo;

    public event Action<Vector3> OnMouseClicked;

    public event Action<GameObject> OnEnemyClicked;


    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(this);
    }
    void Update()
    {
        SetCursorTexture();
        MouseControl();

    }

    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //切换鼠标贴图

            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
                //Debug.Log("Mouse click");
                OnMouseClicked?.Invoke(hitInfo.point);
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                //Debug.Log("Mouse click");
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);

        }

    }



    // Start is called before the first frame update
    //public static MouseManager instance;
    //private Ray ray;
    //private RaycastHit hit;
    //public Texture2D point, doorway, attack, target, arrow;
    //public event Action<Vector3> onMouseClicked;
    //public event Action<GameObject> onEnemyClicked;
    ////public event Action<GameObject> onEnemyClicked;
    //void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Destroy(gameObject);
    //    }
    //    instance = this;
    //}
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //更改鼠标图案
    //    SetCursorTexture();
    //    //点击地面和敌人，调用player中的函数
    //    MouseControl();
    //}
    //void SetCursorTexture()
    //{
    //    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        switch (hit.collider.tag)
    //        {
    //            case "ground":
    //                Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
    //                break;
    //            case "enemy":
    //                Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
    //                break;
    //        }
    //    }
    //}
    //void MouseControl()
    //{
    //    if (Input.GetMouseButtonDown(0) && hit.collider != null)
    //    {
    //        if (hit.collider.tag == "ground")
    //        {
    //            onMouseClicked?.Invoke(hit.point);
    //        }
    //        if (hit.collider.tag == "enemy")
    //        {
    //            onEnemyClicked?.Invoke(hit.collider.gameObject);
    //        }
    //    }
    //}
}


/*
在C#中，问号（?）运算符是可空类型和空条件操作符的一部分。
具体来说，?. 是空条件（null-conditional）操作符。
它用于简化对可能为null的对象进行成员访问或方法调用时的空值检查。
*/