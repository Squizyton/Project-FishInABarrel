using System;
using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using UnityEngine;

namespace Verlet_Intergration
{
    public class VerletIntergration : MonoBehaviour
    {
        [Title("Verlet Settings")] [SerializeField]
        private int iterations;

        [SerializeField] private bool processEveryNthFrame;
        [SerializeField,ShowIf("processEveryNthFrame")] private float lerpSpeed;
        
        [ShowIf("processEveryNthFrame")] [SerializeField]
        private float nthFrame;

        [SerializeField] private float drag;
        public Vector2 Gravity = Vector2.down;
        public float GravityStrength = 9.81f;


        [Title("Options")] [SerializeField] private bool savePoints;
        [SerializeField] private bool processTheSticks;
        [SerializeField] private bool drawPoints;


        /// <summary>
        /// Saved Points that will be used to calculate the next frame
        /// 
        /// </summary>
        [Title("Running options")] public bool pauseProcessing;

        public List<Point> SavedPoints
        {
            get { return _points; }
        }


        private List<Point> _points;

        //public List<Point> Points => _points;
        private List<Stick> _sticks;

        private float _time;

        private void Awake()
        {
            _points = new List<Point>();
            _sticks = new List<Stick>();
        }

        public void SetupPoints(List<Vector3> points, int[] lockedPoints)
        {
            _points.Clear();
            _sticks.Clear();

            for (int i = 0; i < points.Count; i++)
            {
                var newlyCreatedPoint = new Point(points[i], points[i]);

                if (lockedPoints.Contains(i))
                    newlyCreatedPoint.IsLocked = true;
                
                _points.Add(newlyCreatedPoint);
            }

            CreateSticks();
        }


        private void CreateSticks()
        {
            for (int i = 0; i < _points.Count - 1; i++)
            {
                var stick = new Stick(_points.ElementAt(i), _points.ElementAt(i + 1));
                _sticks.Add(stick);
            }
        }


        public void UpdateEachPointCurrentPosition(List<Vector3> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                _points.ElementAt(i).Position = points[i];
            }
        }


        public void FixedUpdate()
        {
            if (pauseProcessing) return;


            if (_points.Count == 0) return;

            if (processEveryNthFrame)
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
            else ProcessPoints();
        }

        void ProcessPoints()
        {
            //debug.Log("Processing Points");
            for (int i = 0; i < _points.Count; i++)
            {
                if (!_points[i].IsLocked)
                {
                    Vector3 position = _points[i].Position;
                    Vector3 previousPosition = _points[i].PreviousPosition;

                    //Extra acceleration
                    Vector3 acceleration = Gravity * GravityStrength;

                    Vector3 temp = position;

                    position += (position - previousPosition) * drag;
                    position += acceleration * Mathf.Pow(Time.deltaTime, 2);

                    _points.ElementAt(i).PreviousPosition = temp;
                    _points.ElementAt(i).Position = position;
                }
            }

            if (processTheSticks)
            {
                ProcessSticks();
            }
        }

        void ProcessSticks()
        {
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < _sticks.Count; j++)
                {
                    Vector3 pointAPosition = _sticks[j].PointA.Position;
                    Vector3 pointBPosition = _sticks[j].PointB.Position;

                    // Calculate the vector between the 3 points
                    float dx = pointBPosition.x - pointAPosition.x;
                    float dy = pointBPosition.y - pointAPosition.y;
                    float dz = pointBPosition.z - pointAPosition.z;

                    // Calculate the current distance (magnitude of the vector)
                    float distance = Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
                    // Calculate how much the current distance differs from the target stick length
                    float difference = _sticks.ElementAt(j).Length - distance;

                    // Calculate the percentage of the difference to apply to each point
                    // We divide by distance to normalize the direction, and by 2 so each point moves half the needed distance
                    float percent = difference / distance / 2;
                    float offsetX = dx * percent;
                    float offsetY = dy * percent;
                    float offsetZ = dz * percent;

                    // Move Point A and Point B in opposite directions to restore the correct distance

                    if (processEveryNthFrame)
                    {
                        if (!_sticks[j].PointA.IsLocked)
                            _sticks[j].PointA.Position = Vector3.Lerp(_sticks[j].PointA.Position,
                                _sticks[j].PointA.Position - new Vector3(offsetX, offsetY, offsetZ),
                                Time.deltaTime * lerpSpeed);

                        if (!_sticks[j].PointB.IsLocked)
                            _sticks[j].PointB.Position = Vector3.Lerp(_sticks[j].PointB.Position,
                                _sticks[j].PointB.Position + new Vector3(offsetX, offsetY, offsetZ),
                                Time.deltaTime * lerpSpeed);
                    }
                    else
                    {
                        
                        if (!_sticks[j].PointA.IsLocked)
                            _sticks[j].PointA.Position -= new Vector3(offsetX, offsetY, offsetZ);

                        if (!_sticks[j].PointB.IsLocked)
                            _sticks[j].PointB.Position += new Vector3(offsetX, offsetY, offsetZ);
                    }
                }
            }
        }

        public HashSet<Vector3> ReturnRawPoints()
        {
            return new HashSet<Vector3>(_points.Select(point => point.Position));
        }

        public class Point
        {
            public Point(Vector3 startPoint, Vector3 previousPosition)
            {
                Position = startPoint;
                PreviousPosition = previousPosition;
            }

            public Vector3 Position;
            public Vector3 PreviousPosition;

            public bool IsLocked;
        }

        public class Stick
        {
            public Stick(Point pointA, Point pointB)
            {
                PointA = pointA;
                PointB = pointB;
                Length = Vector3.Distance(pointA.Position, pointB.Position);
            }

            public Point PointA;
            public Point PointB;

            public float Length;
        }


        public void UpdatePointIndex(int index, Vector3 position)
        {
            _points[index].Position = position;
        }

        private void OnDrawGizmos()
        {
            if (drawPoints)
            {
                if (_points == null) return;

                foreach (var point in _points)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(point.Position, 0.01f);
                }

                foreach (var stick in _sticks)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(stick.PointA.Position, stick.PointB.Position);
                }
            }
        }


        [Button]
        public void ProcessOneFrame()
        {
            ProcessPoints();
        }
    }
}