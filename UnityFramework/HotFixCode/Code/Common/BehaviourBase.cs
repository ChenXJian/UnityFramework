using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HotFixCode
{
    public abstract class BehaviourBase
    {
        protected GameObject gameObject;
        protected Transform transform;

        /// <summary>
        /// 子类必须重载并调用基类
        /// </summary>
        protected virtual void Awake(GameObject rGo)
        {
            gameObject = rGo;
            transform = rGo.GetComponent<Transform>();
        }

        /// <summary>
        /// 子类选择性重载
        /// </summary>
        protected virtual void Start() { }

        protected virtual void OnDestroy() { }

        protected virtual void Update() { }

        protected virtual void LateUpdate() { }

        protected virtual void FixedUpdate() { }

        protected virtual void OnTriggerEnter(UnityEngine.Collider collider) { }

        protected virtual void OnTriggerExit(UnityEngine.Collider collider) { }

        protected virtual void OnTriggerStay(UnityEngine.Collider collider) { }

        protected virtual void OnCollisionEnter(UnityEngine.Collision collider) { }

        protected virtual void OnCollisionExit(UnityEngine.Collision collider) { }

        protected virtual void OnCollisionStay(UnityEngine.Collision collider) { }

        protected virtual void OnTriggerEnter2D(UnityEngine.Collider2D collider) { }

        protected virtual void OnTriggerExit2D(UnityEngine.Collider2D collider) { }

        protected virtual void OnTriggerStay2D(UnityEngine.Collider2D collider) { }

        protected virtual void OnCollisionEnter2D(UnityEngine.Collision2D collider) { }

        protected virtual void OnCollisionExit2D(UnityEngine.Collision2D collider) { }

        protected virtual void OnCollisionStay2D(UnityEngine.Collision2D collider) { }

    }
}
