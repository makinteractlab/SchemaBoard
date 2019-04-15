// Draw lines to the connected game objects that a script has.
// If the target object doesn't have any game objects attached
// then it draws a line from the object to (0, 0, 0).

// using UnityEditor;
// using UnityEngine;

// class DottedLine : Editor
// {
//     float dashSize = 4.0f;

//     private static DottedLine instance;
//     public static DottedLine Instance
//     {
//         get
//         {
//             if (instance == null)
//                 instance = FindObjectOfType<DottedLine>();
//             return instance;
//         }
//     }

//     public void DrawDottedLine(Vector3 _start, Vector3 _end, float _dashSize)
//     {
//         Handles.DrawDottedLine(_start, _end, _dashSize);
//     }

//     void Update() {

//     }
// }


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DottedLine
{
    public class DottedLine : MonoBehaviour
    {
        // Inspector fields
        public Sprite Dot;
        [Range(0.01f, 1f)]
        public float Size = 0.05f;
        [Range(0.1f, 2f)]
        public float Delta = 0.2f;

        //Static Property with backing field
        private static DottedLine instance;
        public static DottedLine Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<DottedLine>();
                return instance;
            }
        }

        //Utility fields
        List<Vector3> positions = new List<Vector3>();
        List<GameObject> dots = new List<GameObject>();

        // Update is called once per frame
        void FixedUpdate()
        {
            if (positions.Count > 0)
            {
                DestroyAllDots();
                positions.Clear();
            }

        }

        private void DestroyAllDots()
        {
            foreach (var dot in dots)
            {
                Destroy(dot);
            }
            dots.Clear();
        }

        GameObject GetOneDot()
        {
            var gameObject = new GameObject();
            gameObject.transform.localScale = Vector3.one * Size;
            gameObject.transform.parent = transform;

            var sr = gameObject.AddComponent<SpriteRenderer>();
            sr.sprite = Dot;
            return gameObject;
        }

        public void DrawDottedLine(Vector3 start, Vector3 end)
        {
            DestroyAllDots();

            Vector3 point = start;
            Vector3 direction = (end - start).normalized;

            while ((end - start).magnitude > (point - start).magnitude)
            {
                positions.Add(point);
                point += (direction * Delta);
            }

            Render();
        }

        private void Render()
        {
            foreach (var position in positions)
            {
                var g = GetOneDot();
                g.transform.position = position;
                dots.Add(g);
            }
        }
    }
}