using UnityEngine;

[CreateAssetMenu(menuName = "Parkour Action/New Parkour Action")]
public class ParkourAction : ScriptableObject
{
    public string animName;
    public float minHeight;
    public float maxHeight;
    public bool rotateTowardsObstacle;
    [Header("_________Target Matching_________")]
    public bool enableTargetMatching;
    public AvatarTarget target;
    public float startMatch;
    public float targetMatch;
    
    public Quaternion targetRotation { get; set; }
    public Vector3 matchPos { get; set; }
    public bool IsValid(RaycastHit forwardRay,RaycastHit heightRay,Transform player)
    {
        float height = Mathf.Abs(heightRay.point.y - player.position.y);
        //Debug.Log(height);
        if(rotateTowardsObstacle)
        {
            targetRotation = Quaternion.LookRotation(-forwardRay.normal);

            Vector3 direction = forwardRay.point - player.position;
            direction.y = 0;
            player.rotation = Quaternion.LookRotation(direction);
        }

        if(enableTargetMatching)
        {
            matchPos = heightRay.point;
        }

        return forwardRay.collider != null && height >= minHeight && height <= maxHeight;

    }
}

