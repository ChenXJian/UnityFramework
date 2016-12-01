using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotFixCode
{
    class SampleCollision
    {
        void Update()
        {
            UnityEngine.Debug.Log("Update");
        }

        void OnTriggerEnter(UnityEngine.Collider collider)
        {
            UnityEngine.Debug.Log("OnTriggerEnter");
        }

        void OnTriggerExit(UnityEngine.Collider collider)
        {
            UnityEngine.Debug.Log("OnTriggerExit");
        }


        void OnTriggerStay(UnityEngine.Collider collider)
        {
            UnityEngine.Debug.Log("OnTriggerStay");
        }


        void OnCollisionEnter(UnityEngine.Collision collider)
        {
            UnityEngine.Debug.Log("OnCollisionEnter");
        }


        void OnCollisionExit(UnityEngine.Collision collider)
        {
            UnityEngine.Debug.Log("OnCollisionExit");
        }

        void OnCollisionStay(UnityEngine.Collision collider)
        {
            UnityEngine.Debug.Log("OnCollisionStay");
        }
    }
}
