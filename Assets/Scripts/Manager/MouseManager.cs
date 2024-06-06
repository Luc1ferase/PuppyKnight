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
            //�л������ͼ

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
    //    //�������ͼ��
    //    SetCursorTexture();
    //    //�������͵��ˣ�����player�еĺ���
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
��C#�У��ʺţ�?��������ǿɿ����ͺͿ�������������һ���֡�
������˵��?. �ǿ�������null-conditional����������
�����ڼ򻯶Կ���Ϊnull�Ķ�����г�Ա���ʻ򷽷�����ʱ�Ŀ�ֵ��顣
*/