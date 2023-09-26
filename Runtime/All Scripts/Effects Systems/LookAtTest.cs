using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTest : MonoBehaviour
{
    [SerializeField] private bool active;
    [SerializeField] private float headThreshold;
    [SerializeField] private float headSmoothness;
    [SerializeField] private Transform headTransform; // We rotate this head transform but calculate dot from this transform 

    [SerializeField] private Transform headDefault;


    [SerializeField] private float dotResult;

    [SerializeField] private bool drawGizmos;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            Transform playerTransform = GameObject.FindWithTag("Player").transform;

            // Calculate player direction from this transform
            Vector3 playerDirection = playerTransform.position - transform.position;
            playerDirection.Normalize();


            // Calculate dot product using the master (not rotated) transform
            float dot = Vector3.Dot(headDefault.forward, playerDirection);
            dotResult = dot;

            if (dot > headThreshold)
            {
                headTransform.rotation = Quaternion.Slerp(headTransform.rotation, Quaternion.LookRotation(playerDirection), headSmoothness * Time.deltaTime);
            }
            else
            {
                headTransform.rotation = Quaternion.Slerp(headTransform.rotation, headDefault.rotation, headSmoothness * Time.deltaTime);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Transform playerTransform = GameObject.FindWithTag("Player").transform;
        
            Vector3 playerDirection = playerTransform.position - transform.position;
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, playerDirection);
    
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.forward);
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, headTransform.forward);    
        }
    }
}
