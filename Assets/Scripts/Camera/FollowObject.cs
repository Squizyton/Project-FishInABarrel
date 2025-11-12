using Alchemy.Inspector;
using UnityEngine;
using Utilities.Utilities;

public class FollowObject : MonoBehaviour
{
    [Title("Follox Axis")]
    public Vector3Int followAxis;
    
    
    [Title("Offset")]
    public Vector3 offset;

    [Space(20),Title("Target")]
    public Transform target;


    void FixedUpdate()
    {
        transform.position = target.position.MultiplyXYZByXYZ(followAxis) + offset;
    }
}
