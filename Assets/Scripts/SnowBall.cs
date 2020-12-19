using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    public Transform snowball;
    private float growthRate=10f;
    private float growthAmount=1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( snowball )   // if the snowball exists
        {
            if (Input.GetKey(KeyCode.A)        // if the "up" key is held
                && snowball.localScale.x<= 100f) // and the snowball scale is <= 100
            {
                growthAmount = growthRate *Time.deltaTime;   // calculate growthAmount
                snowball.localScale += new Vector3(growthAmount,growthAmount,growthAmount); // apply the growthAmount to the localScale
            }
        }
    }
}
