using System;
using System.Collections;
using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Vector3 forwardRayOffset = new Vector3(0,2.5f,0);
    [SerializeField] private float ForwardScanDistance = 5f;
    [SerializeField] private float heightRayCastLength = 5f;
    


    // Update is called once per frame
    void Update()
    {
        //ScanObstacle();
    }

    public ObstacleInfo ScanObstacle()
    {
        var hitData = new ObstacleInfo();
        hitData.forwardHitFound = Physics.Raycast(transform.position + forwardRayOffset, transform.forward, out hitData.forwardHit,ForwardScanDistance);

        Debug.DrawRay(transform.position + forwardRayOffset, transform.forward * ForwardScanDistance, (hitData.forwardHitFound) ? Color.red : Color.white);

        if(hitData.forwardHitFound)
        {
            var heightOrigin = hitData.forwardHit.point + Vector3.up * heightRayCastLength;
            
            hitData.heightHitFound = Physics.Raycast(heightOrigin, Vector3.down, out hitData.heightHit, heightRayCastLength, obstacleLayer);
            Debug.Log($"Height Hit Found: {hitData.heightHitFound} at point: {hitData.heightHit.point}");
            Debug.DrawRay(heightOrigin, Vector3.down * heightRayCastLength, (hitData.heightHitFound) ? Color.red : Color.white);
        }


        return hitData;
    }

    public struct ObstacleInfo
    {
        public bool forwardHitFound;
        public bool heightHitFound;
        public RaycastHit forwardHit;
        public RaycastHit heightHit;

    }



}
