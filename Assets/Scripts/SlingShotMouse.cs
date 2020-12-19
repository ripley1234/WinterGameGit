using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SlingShotMouse : MonoBehaviour
{
   public bool enable = false;
    
    public Transform StartPoint;
    public Transform EndPoint;
    public Transform MiddlePoint;
    public float speed;
    public bool attached = false;
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private float ropeSegLen = 0.35f;
    private int segmentLength = 25;
    private float lineWidth = 0.2f;
    //Sling shot 
    private bool moveToMouse = false;
    private Vector3 mousePositionWorld;
    private int indexMousePos;
    RaycastHit hit;
    public Vector3 firstMiddlePoint;
    bool bagliydi = false;
    Vector3 changeAmount2;
    Vector3 changeAmount3;
    Ray ray;
    //float ratio=0.2f;
   public GameObject puzzle;
    public float distance;
    private AudioSource audio;
    //private AudioManager _audioManager;
    public LineRenderer trajectory;
    
    
    Vector3 screenMousePos;

    //private Slider powerbar;
    //private float powerBarValue;
    
    
    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        //_audioManager = GameObject.Find("CameraParent").GetComponent<AudioManager>();
       // powerbar = GameObject.Find("power bar").GetComponent<Slider>();
       // powerbar.minValue = 0f;
       // powerbar.maxValue = 10f;
       // powerbar.value = powerBarValue;
       
       
        this.lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = StartPoint.position;

        for (int i = 0; i < segmentLength; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }

        StartCoroutine(CreateCollider());
    }

    IEnumerator CreateCollider()
    {
        yield return new WaitForSeconds(0.5f);
        firstMiddlePoint = MiddlePoint.transform.position;
        BoxCollider col = transform.gameObject.AddComponent<BoxCollider>();
        col.size = new Vector3(11.63f, 0.4f, 0.7f);
        col.center = new Vector3(1.13f, 0, 0);
        float lineLength = Vector3.Distance(StartPoint.transform.position, EndPoint.transform.position); // length of line
        Vector3 midPoint = (StartPoint.transform.position + EndPoint.transform.position) / 2;
        
    }

    void SetTrajectoryActive(bool active)
    {
        trajectory.enabled = active;
    }



    // Update is called once per frame
    void Update()
    {

        
        if (enable == true)
        {

            this.DrawRope();
        }




        if (Input.GetMouseButtonDown(0))
        {
            //ray = cam2.ScreenPointToRay(Input.mousePosition);

            //this.mousePositionWorld = Vector3.MoveTowards(firstMiddlePoint, Vector3.back, 1f);
            
                if (attached == true)
                {
                   
                    Debug.Log("asdasd");
                    bagliydi = true;
                    //distance = Vector3.Distance(MiddlePoint.transform.position, puzzle.transform.position);
                    //MiddlePoint.position = this.mousePositionWorld;
                    //firstMiddlePoint=Vector3.MoveTowards(firstMiddlePoint,Vector3.back, 1f );
                    //this.mousePositionWorld = firstMiddlePoint;
                    this.moveToMouse = true;
                    //puzzle.transform.position = MiddlePoint.position;
                    GetComponent<BoxCollider>().enabled = false;
                    GameObject.Find("Player").GetComponent<Rigidbody>().useGravity = false;
                    
                  
                }
               
               


        }
        else if (Input.GetMouseButtonUp(0)&& attached==true)
        {
            //audio.Stop();
           //  todo _audioManager.İpBırakma();
           

            //this.mousePositionWorld = Vector3.MoveTowards(firstMiddlePoint, Vector3.back, 1f);
            //ray = cam2.ScreenPointToRay(Input.mousePosition);
            if (distance > 5f)
            {
                attached = false;
                bagliydi = false;
                Firlat(distance);
                this.mousePositionWorld = MiddlePoint.transform.position;
            }

            SetTrajectoryActive(false);

        }
        else if (this.moveToMouse == true && Input.GetMouseButton(0)&& attached==true)
        {
/*   todo         if (!audio.isPlaying)
            {
                audio.Play();
            }*/
            screenMousePos = Input.mousePosition;
            screenMousePos.y = 0;
            screenMousePos.z = 0;
            // this.mousePositionWorld = Vector3.Lerp(screenMousePos,Vector3.back, 2.5f * Time.fixedDeltaTime);
            firstMiddlePoint=Vector3.MoveTowards(puzzle.transform.position,Vector3.back, 2.5f*Time.deltaTime );    
            this.mousePositionWorld = firstMiddlePoint;
            distance = Vector3.Distance(MiddlePoint.transform.position, puzzle.transform.position);
           // powerbar.value = distance;
           
           
            if (distance>=5f) // TODO 5 den sonra takılı kalıyor sonra tasarla
            {
                //audio.Stop();
               // SetTrajectoryActive(true);
                
                firstMiddlePoint= puzzle.transform.position;

                firstMiddlePoint.x = (screenMousePos.x * Time.fixedDeltaTime) / (distance/1.8f);

                 this.mousePositionWorld = firstMiddlePoint;
            }


            float pullDistance = Vector3.Distance(MiddlePoint.transform.position, puzzle.transform.position);
            ShowTrajectory(pullDistance);

            //bagliydi = true;
            //distance = Vector3.Distance(firstMiddlePoint, puzzle.transform.position);

            //this.mousePositionWorld = Vector3.MoveTowards(firstMiddlePoint, Vector3.back, 1f);
            //MiddlePoint.position = this.mousePositionWorld;
            //GameObject.Find("Player").transform.position = MiddlePoint.position;
            //GetComponent<BoxCollider>().enabled = false;
            //GameObject.Find("Player").GetComponent<Rigidbody>().useGravity = false;
        }
       // float distance = Vector3.Distance(firstMiddlePoint, puzzle.transform.position);
        
        if (bagliydi)
        {
            puzzle.transform.position = Vector3.Lerp(puzzle.transform.position, firstMiddlePoint, 1f);
            

        }
        if (enable == true)
        {
            //screenMousePos = 0.1f;
            float xStart = StartPoint.position.x;
            float xEnd = EndPoint.position.x;
            
            //this.mousePositionWorld = Vector3.MoveTowards(firstMiddlePoint, Vector3.back, 1f);
            float currX = mousePositionWorld.x;
            float ratio = (currX - xStart) / (xEnd - xStart);
            if (ratio > 0)
            {
                this.indexMousePos = (int)(this.segmentLength * ratio);
            }
        }
        
        
    }

    private void ShowTrajectory(float pullDistance)
    {
        SetTrajectoryActive(true);
        Vector3 diff = MiddlePoint.transform.position - puzzle.transform.position;
        int segmentCountt = 25;
        Vector3[] segments = new Vector3[segmentCountt];
        segments[0] = puzzle.transform.position;
        
        Vector3 segVelocity = new Vector3(diff.x, diff.y, diff.z) * distance;
        for (int i = 1; i < segmentCountt; i++)
        {
            float timeCurve = (i * Time.fixedDeltaTime * 5);
            segments[i] = segments[0] + segVelocity * timeCurve + 0.5f * Physics.gravity * Mathf.Pow(timeCurve, 2);
        }

        trajectory.positionCount = segmentCountt;
        for (int j = 0; j < segmentCountt; j++)
        {
            trajectory.SetPosition(j, segments[j]);
        }
    }


    public void OnBecameVisible()
    {
        enable = true;
    }
    public void OnBecameInvisible()
    {
        enable = false;
    }

    IEnumerator ActivateCollider()
    {
        yield return new WaitForSeconds(0.6f);
        GetComponent<BoxCollider>().enabled = true;
    }

    void Firlat(float distance)
    {
        
        
        GameObject.Find("Player").GetComponent<Rigidbody>().useGravity = true;
        StartCoroutine(ActivateCollider());
        Vector3 velocity = MiddlePoint.transform.position - puzzle.transform.position;
        puzzle.GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, velocity.y, velocity.z)  * distance;



    }

    public void FixedUpdate()
    {
        if (enable == true)
        {
            this.Simulate();
        }
    }

    private void Simulate()
    {
        // SIMULATION
        Vector3 forceGravity = new Vector3(0f, -1f, 0f);

        for (int i = 1; i < this.segmentLength; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector3 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 50; i++)
        {
            this.ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        //Constrant to First Point 
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = this.StartPoint.position;
        this.ropeSegments[0] = firstSegment;


        //Constrant to Second Point 
        RopeSegment endSegment = this.ropeSegments[this.ropeSegments.Count - 1];
        endSegment.posNow = this.EndPoint.position;
        this.ropeSegments[this.ropeSegments.Count - 1] = endSegment;

        for (int i = 0; i < this.segmentLength - 1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);
            Vector3 changeDir = Vector3.zero;

            if (dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }


            Vector3 changeAmount = changeDir * error;
            changeAmount2 = new Vector3(0 * 0.8f, changeAmount.x * 0.8f, changeAmount.z * -1) / 2.5f;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }

            if (this.moveToMouse && indexMousePos > 0 && indexMousePos < this.segmentLength - 1 && i == indexMousePos)
            {
                RopeSegment segment = this.ropeSegments[i];
                RopeSegment segment2 = this.ropeSegments[i + 1];
                segment.posNow = new Vector3(this.mousePositionWorld.x, this.mousePositionWorld.y, this.mousePositionWorld.z);
                segment2.posNow = new Vector3(this.mousePositionWorld.x, this.mousePositionWorld.y ,this.mousePositionWorld.z);
                this.ropeSegments[i] = segment;
                this.ropeSegments[i + 1] = segment2;
            }
        }
    }

    public void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentLength];
        for (int i = 0; i < this.segmentLength; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    public struct RopeSegment
    {
        public Vector3 posNow;
        public Vector3 posOld;

        public RopeSegment(Vector3 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }



    public bool Iptemi()
    {
        GameObject[] allropes = GameObject.FindGameObjectsWithTag("Rope");
        bool degiyormu = false;
        for (int i = 0; i < allropes.Length; i++)
        {
            if (allropes[i].GetComponent<SlingShotMouse>().attached)
            {
                degiyormu = true;
                break;
            }
        }
        return degiyormu;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.name == "Player")
        {
            if(Iptemi())
            {
                return;
            }

            attached = true;
            GameObject.Find("Player").GetComponent<Rigidbody>().velocity = Vector3.zero; 
            GameObject.Find("Player").GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            
            
            
           
        }
       
    }
    

}
