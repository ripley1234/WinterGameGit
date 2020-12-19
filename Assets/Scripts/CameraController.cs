using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform[] views;
    public float transitionSpeed;
    Transform currentView;
    private int i = 0;
    public GameObject ballLauncher;
    public Transform ballLaunchertransform;

    public GameObject Target;
    public Transform spawnPoint;


    private void Awake()
    {
        InvokeRepeating("SpawnDelay",5,5);
    }

    private void Start()
    {
        currentView = views[0];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit)) 
            {
                if(hit.collider.tag == "firstcam")
                {
                    currentView = views[1];
                    StartCoroutine(delay());
                }
            
                if (hit.collider.tag=="secondcam")
                {
                    currentView = views[2];
                }
                if (hit.collider.tag=="thirdcam")
                {
                    currentView = views[3];
                }
            }
        }

       



    }

    
    void SpawnDelay()
    {
        
            Instantiate(Target, spawnPoint.position, Quaternion.Euler(16.65f,90f,0f));

        
        
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(ballLauncher, ballLaunchertransform.position, Quaternion.identity);
        
        
    }

    

    public void Button()
    {
        currentView = views[0];
        
    }


    void LateUpdate()
    {

        //Lerp position
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);

        Vector3 currentAngle = new Vector3(
            Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x,
                Time.deltaTime * transitionSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y,
                Time.deltaTime * transitionSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z,
                Time.deltaTime * transitionSpeed));

        transform.eulerAngles = currentAngle;

    }
}
