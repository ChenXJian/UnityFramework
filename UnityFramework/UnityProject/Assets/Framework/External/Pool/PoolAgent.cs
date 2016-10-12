using UnityEngine;
using System;
using System.Collections;

namespace LYNet
{
    public class PoolAgent : MonoBehaviour
    {

        private PoolManager.PoolObjectInternal poolObject;

        public Action onSwapEnd;
        public Action<object> onSwapEnd1;
        public Action<object, object> onSwapEnd2;
        public Action<object, object, object> onSwapEnd3;
        public Animation ani;
        public ParticleSystem particle;
        public new AudioSource audio;
        public bool recycleWhenParentDisable = false;   //当父节点disable是否回收
        void OnDisable()
        {
            if (gameObject.activeSelf)
            {
                if (recycleWhenParentDisable)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    return;
                }
            }

            if (poolObject != null)
            {
                poolObject.objList.Add(this);
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }

        public void Init(PoolManager.PoolObjectInternal _poolObject, PoolManager.PoolObject.EndType _endType = PoolManager.PoolObject.EndType.EndByScript)
        {
            poolObject = _poolObject;
            if (ani != null || particle != null || audio != null)
            {
                return;
            }

            var endType = (poolObject == null) ? _endType : poolObject.endType;
            if (endType == PoolManager.PoolObject.EndType.EndByAni)
            {
                ani = GetComponent<Animation>();
            }
            else if (endType == PoolManager.PoolObject.EndType.EndByParticle)
            {
                particle = GetComponent<ParticleSystem>();
            }
            else if (endType == PoolManager.PoolObject.EndType.EndBySound)
            {
                audio = GetComponent<AudioSource>();
            }
            else if (endType == PoolManager.PoolObject.EndType.EndByAI)
            {
                particle = GetComponent<ParticleSystem>();
                ani = GetComponent<Animation>();
                audio = GetComponent<AudioSource>();
            }
        }

        public void SwapEnd(params object[] args)
        {
            switch (args.Length)
            {
                case 0:
                    if (onSwapEnd != null)
                    {
                        onSwapEnd();
                    }
                    break;
                case 1:
                    if (onSwapEnd1 != null)
                    {
                        onSwapEnd1(args[0]);
                    }
                    break;
                case 2:
                    if (onSwapEnd2 != null)
                    {
                        onSwapEnd2(args[0], args[1]);
                    }
                    break;
                case 3:
                    if (onSwapEnd3 != null)
                    {
                        onSwapEnd3(args[0], args[1], args[2]);
                    }
                    break;
            }
        }

        void Update()
        {
            if (ani != null && !ani.isPlaying)
            {
                gameObject.SetActive(false);
            }
            else if (particle != null && !particle.isPlaying)
            {
                gameObject.SetActive(false);
            }
            else if (audio != null && !audio.isPlaying)
            {
                gameObject.SetActive(false);
            }
        }



    }
}