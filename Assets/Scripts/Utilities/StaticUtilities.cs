using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Utilities.Utilities
{
    public static class StaticUtilities
    {
        public static readonly float TAU = Mathf.PI * 2;


        private static Dictionary<string, GameObject> staticObjectsFound = new Dictionary<string, GameObject>();


        /// <summary>
        /// Returns a random value from a list.
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RandomValue<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Translates a screen point to a canvas point.
        /// </summary>
        /// <param name="camera">The cam parameter should be the camera associated with the screen point. For a RectTransform in a Canvas set to Screen Space - Overlay mode, the cam parameter should be null. Use</param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector3 ScreenPointToCanvasPoint(this Camera camera, RectTransform parent, Vector3 position)
        {
            try
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, position, camera, out var anchoredPos);
                return anchoredPos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public static string GetTimeFromSeconds(float seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            string str = time.ToString(@"hh\:mm\:ss\:fff");

            return str;
        }


        /// <summary>
        /// ScreenPointToLocalPointInRectangle requires a camera ref to be null if using Screen Space - Overlay for canvas
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ScreenPointToCanvasPointNoCameraReference(RectTransform parent, Vector3 position,
            Camera nullCamera = null)
        {
            try
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, position, nullCamera,
                    out var anchoredPos);
                return anchoredPos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public static Vector3 ClampPositionBasedOnFrustrumView(Vector3 position, float padding)
        {
            if (_staticMainCamera == null)
                _staticMainCamera = Camera.main;

            Vector3 viewportPosition = _staticMainCamera.WorldToViewportPoint(position);

            //clamp the values
            float clampedX = Mathf.Clamp(viewportPosition.x, 0.0f + padding, 1.0f - padding);
            float clampedY = Mathf.Clamp(viewportPosition.y, 0.0f, 1.0f - padding);

            return _staticMainCamera.ViewportToWorldPoint(new Vector3(clampedX, clampedY, 0));
        }


        ///<summary>
        ///This Allows T to be Instantiated as Unity Objects or normal objects
        /// </summary>
        public static T Instantiate<T>(this Object unityObject, T t) where T : Object
        {
            return Object.Instantiate(t) as T;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Instantiate<T>(this T unityObject) where T : Object
        {
            return Object.Instantiate(unityObject) as T;
        }


        /// <summary>
        /// Rounds the Vector to the nearest Int
        /// Please note that to get a 1 or -1, normalize it first.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 RoundToint(this Vector2 vector)
        {
            //Round the x and y
            var x = Mathf.RoundToInt(vector.x);
            var y = Mathf.RoundToInt(vector.y);

            //Set the values back to the vector
            vector.x = x;
            vector.y = y;

            //Return the vector
            return vector;
        }


        private static Camera _staticMainCamera;

        /// <summary>
        /// Hold's a reference to the Main Camera statically
        /// If using Rider, it will be mad that is "expensive", it's not
        /// </summary>
        /// <returns></returns>
        public static Camera GetMainCamera()
        {
            if (_staticMainCamera == null)
                _staticMainCamera = Camera.main;

            return _staticMainCamera;
        }


        /// <summary>
        /// Calculates a frame-independent damping value based on the provided speed and the current frame delta time.
        /// Keep in mind that if lerping, the closer the object is to the target point, the slower it is. 
        /// </summary>
        /// <param name="speed">The speed factor that dictates how quickly the damping effect occurs.</param>
        /// <returns>The calculated damping value that is consistent across variable frame rates.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float FrameIndependentDamping(float speed)
        {
            return 1 - Mathf.Exp(-speed * Time.deltaTime);
        }

        /// <summary>
        /// Get's an object then stores it in a Dictionary for faster grabbing. 
        /// </summary>
        /// <param name="ObjectName"></param>
        /// <returns></returns>
        public static GameObject GetObject(string ObjectName)
        {
            if (!staticObjectsFound.ContainsKey(ObjectName))
            {
                staticObjectsFound.Add(ObjectName, GameObject.Find(ObjectName));
            }

            return staticObjectsFound[ObjectName];
        }
    }
}