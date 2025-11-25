using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using UnityEngine;


/// <summary>
/// This is taking from
/// https://github.com/Yword/Unity-SmoothLineTrail/blob/main/Assets/Scripts/SmoothLineTrailController.cs
/// </summary>
public class Chaikin : MonoBehaviour
{
    [Title("Chaikin Settings")] [SerializeField]
    private bool updateRunTime = false;

    [HideIf("updateRunTime"), HelpBox("Supply base points if NOT updating runtime")] [SerializeField]
    private Transform[] baseSupplyPoints;

    [Space(5), HelpBox("Number of iterations which represents the smoothness of the curve")]
    public int iterations = 3;


    [ShowIf("updateRunTime")]
    /// <summary>
    /// Life Time of each point
    /// </summary>
    public float trailLifeTime = 1;


    [ShowIf("updateRunTime")] public float minPointDistance = 0.1f;


    public bool ignoreTimeScale = false;


    private List<Vector3> points = new();
    private List<float> times = new();
    private Transform _transform;
    private LineRenderer _lineRenderer;

    private List<Vector3> _cachedPoints = new();

    private void Awake()
    {
      
    }


    private void Update()
    {
        if (!updateRunTime) return;

        Vector3 position = _transform.position;


        //Ensure that it always has at least 2 points
        while (points.Count < 2)
        {
            points.Insert(0, position);


            //Add time to the list (for late)
            times.Insert(0, 0);
        }


        //Add new point if the distance between thecurrent point and the previous point is
        //greater than the minPointDistance
        if ((points[1] - position).sqrMagnitude > minPointDistance * minPointDistance)
        {
            Debug.Log(
                $"The distance between the points {points[1]} and {position} is greater than {minPointDistance} ");

            points.Insert(0, position);
            times.Insert(0, 0);
        }


        //Update the timeline

        if (iterations > 0 && points.Count > 2)
        {
            var processedPoints = ApplyChaikinSmoothing(ref points);
        }

        //Set the first point as the current position
        points[0] = position;

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
    }


    //So this is the actual algorithm
    public ref List<Vector3> ApplyChaikinSmoothing(ref List<Vector3> pointsArray)
    {
        _cachedPoints = new List<Vector3>(pointsArray);


        for (int k = 0; k < iterations; k++)
        {
            //Create a new list to store the smooth points
            List<Vector3> smoothPoints = new();

            //Set the count to the number of points
            int count = _cachedPoints.Count;

            for (int i = 0; i < count - 1; i++)
            {
                // Chaikin's corner-cutting algorithm: generate two new points (Q and R) between p0 and p1
                Vector3 p0 = _cachedPoints[i];
                Vector3 p1 = _cachedPoints[(i + 1) % count];


                // This is the heart of the algorithm
                //Chaikin utilized fixed ratios on cutting off his corners, so that they were all cut the same. When written down mathematically, Chaikin
                //method proceeds as follows: Given a control Polygon {P0, P1, ..., Pn}, we refine this control polygon by generating a new squence of control points
                // {Q0,R0,Q1,R1,...,Qn-1,Qn}.
                //Where each new pair of points Qi, Ri are to be taken too be at a ration of 1/4 and 3/4 between the endpoints of the line segment 
                //PiPi+1
                // That is
                //Qi = 3/4 Pi + 1/4 Pi+1
                //These 2n new points can be considered a new control polgon - a refinement of the original polygon.
                //https://www.cs.unc.edu/~dm/UNC/COMP258/LECTURES/Chaikins-Algorithm.pdf

                Vector3 Q = Vector3.Lerp(p0, p1, 0.25f);
                Vector3 R = Vector3.Lerp(p0, p1, 0.75f);

                smoothPoints.Add(Q);
                smoothPoints.Add(R);
            }

            //Preserve the start and end points
            smoothPoints.Insert(0,_cachedPoints[0]);
            smoothPoints.Add(_cachedPoints[_cachedPoints.Count - 1]);


            _cachedPoints = smoothPoints;
        }


        return ref _cachedPoints;
    }
}