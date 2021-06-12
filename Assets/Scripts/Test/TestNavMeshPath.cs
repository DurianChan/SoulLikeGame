using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

///<summary>
///
///</summary>
public class TestNavMeshPath : MonoBehaviour
{
    public GameObject instance;
    public GameObject target;
    private NavMeshPath path;
    //private Animator animator;
    private float dis;
    private float RunSpeed = 1;
    void Start()
    {
        //animator = GetComponent<Animator>();
        path = new NavMeshPath();
        dis = GetComponent<CapsuleCollider>().radius * 2;
        //创建路径
        NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, path);
        NavMeshHit hit;
        //改变路径中每个点的位置，为了不贴边缘走，这样很怪。
        for (int i = 1; i < path.corners.Length - 2; i++)
        {
            bool result = NavMesh.FindClosestEdge(path.corners[i], out hit, NavMesh.AllAreas);
            if (result && hit.distance < dis)
                path.corners[i] = hit.position + hit.normal * dis;
        }
        Mydebug();
    }

    private float Speed;
    private float Direction;
    private int k = 1;
    void Update()
    {
        Speed = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space))
        {
            RunSpeed = Mathf.MoveTowards(RunSpeed, 2f, Time.deltaTime);
        }
        else
        {
            RunSpeed = Mathf.MoveTowards(RunSpeed, 1f, Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, path.corners[k]) <= RunSpeed && k < path.corners.Length - 1)
        {
            k++;
        }
        if (k < path.corners.Length)
            LookRightDirection(path.corners[k]);



    }


    //用于实时改变方向
    public void LookRightDirection(Vector3 tr)
    {
        Vector3 relative = transform.InverseTransformPoint(new Vector3(tr.x, 0, tr.z));
        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        Direction = angle / 180f * RunSpeed * 1.2f;
        if (Mathf.Abs(Direction) > 1)
        {
            Direction = Mathf.RoundToInt(Direction);
            Debug.Log(Direction);
        }
    }


    public void Mydebug()
    {
        Debug.Log(path.corners.Length);
        for (int i = 0; i < path.corners.Length; i++)
        {
            GameObject.Instantiate(instance, path.corners[i], Quaternion.identity);
        }
        // Debug.Log(path.corners.Length);

    }
}
