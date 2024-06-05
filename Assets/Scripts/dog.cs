using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class dog : MonoBehaviour
{
    public GameObject ball;
    private NavMeshAgent nav;
    private RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)&&Input.GetMouseButtonDown(0)) 
        {
            if (hit.collider.tag == "ground")
            {
                nav.SetDestination(hit.point);

            }
            
        }
        nav.SetDestination(ball.transform.position);
    }
}
