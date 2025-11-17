using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using UnityEngine;

namespace Verlet_Intergration
{
    public class VerletIntergration : MonoBehaviour
    {
        [Title("Verlet Settings")]
        [SerializeField] private int iterations;
        [SerializeField] private bool processEveryNthFrame;

        [ShowIf("processEveryNthFrame")] [SerializeField]
        private float nthFrame;
        public Vector2 Gravity = Vector2.down;
        public float GravityStrength = 9.81f;
        

        
        
        
        
        
        
        
        private List<Point> _points;
        public List<Point> Points => _points;
        private List<Stick> _sticks;
        
        private float _time;
        
        public void SetupPoints(List<Vector3> points, int[] lockedPoints)
        {
            for (int i = 0; i < points.Count; i++)
            {
                var newlyCreatedPoint = new Point(points[i], points[i]);
                
                if(lockedPoints.ToList().Contains(i))
                    newlyCreatedPoint.IsLocked = true;
            }
        }

        
        public void Update()
        {

            if (_time > nthFrame)
            {
                ProcessPoints();
                _time = 0;
            }
            else
            {
                _time += Time.deltaTime;
            }
        }

        void ProcessPoints()
        {
            foreach (var point in _points)
            {
                if (!point.IsLocked)
                {
                    
                    
                    
                    
                }
            }


        }

        public class Point
        {
            public Point(Vector2 startPoint, Vector2 previousPosition)
            {
                Position = startPoint;
                PreviousPosition = previousPosition;
            }
            
            public Vector2 Position;
            public Vector2 PreviousPosition;

            public bool IsLocked;
        }

        public class Stick
        {
            public Stick(Point pointA, Point pointB)
            {
                PointA = pointA;
                PointB = pointB;
                Length = Vector2.Distance(pointA.Position, pointB.Position);
            }
            
            public Point PointA;
            public Point PointB;
            
            public float Length;
        }


    }
}