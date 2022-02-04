using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 mOffset;

    private float mZCoord;

    private Rigidbody rb;

    private Renderer _renderer;

    private bool outOffScreenDelay = false;

    private Transform centerPoint;

    public Transform spawnPoint;

    public void respawn()
    {
        spawnPoint.GetComponent<SpawnItems>().StartSpawn();
    }

    private void Start()
    {
        
        _renderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        
        centerPoint = GameObject.FindGameObjectWithTag("CenterPoint").transform;      
    }

    private void Awake()
    {
        StartCoroutine(FreezeConstraints());
    }

    private IEnumerator FreezeConstraints()
    {        
        yield return new WaitForSeconds(0.1f);
        outOffScreenDelay = true;
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }
    private IEnumerator EnableGravity()
    {
        yield return new WaitForSeconds(0.05f);
        rb.useGravity = true;
    }

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        //Store offset = gameObject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousepoint = Input.mousePosition;

        mousepoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousepoint);
    }

    void OnMouseDrag()
    {
        rb.useGravity = false;
        transform.position = GetMouseWorldPos() + mOffset;
    }
    private void OnMouseUp()
    {
        rb.velocity = Vector3.zero;
        StartCoroutine(EnableGravity());       
    }

    private void Update()
    {

        if (!_renderer.isVisible && outOffScreenDelay)
        {
           transform.position = centerPoint.position;
        }
    }

}
