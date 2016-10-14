using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine.UI;
using UnityEngine;


namespace HotFixCode
{
    public abstract class UILogic
    {
        protected GameObject gameObject;
        protected Transform transform;
        protected LShapBehaviour behaviour;

        /// <summary>
        /// 被原生C#启动
        /// </summary>
        /// <param name="parent">要挂载的UI Window</param>
        protected virtual void Startup(RectTransform parent)
        {
            WaitingLayer.Show();
        }

        /// <summary>
        /// 面板加载完成后的回调
        /// </summary>
        /// <param name="rGo">回传的面板对象</param>
        protected virtual void OnCreated(GameObject rGo)
        {
            WaitingLayer.Hide();
            gameObject = rGo;
            transform = rGo.GetComponent<Transform>();
            behaviour = rGo.GetComponent<LShapBehaviour>();

            var rPanel = PanelStack.Instance.PanelCurrent;
            if (gameObject.name.Contains(rPanel.PanelName))
            {
                rPanel.IsCreated = true;
                DebugConsole.Log("[script match complete]:" + gameObject.name);
            }
            else
            {
                DebugConsole.Log("[script match failed]:" + gameObject.name);
            }
        }

        /// <summary>
        /// 启用面板
        /// </summary>
        protected virtual void Enable()
        {
            if (!gameObject) return;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        /// <summary>
        /// 弃用面板
        /// </summary>
        protected virtual void Disable()
        {
            if (!gameObject) return;
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// 销毁面板
        /// </summary>
        protected virtual void Free()
        {
            Disable();
            if (gameObject != null)
            {
                PopupsManager.Instance.ClearCache();
                GameObject.Destroy(gameObject);
            }
        }
    }
}
