using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HotFixCode
{
    public class UIPopups : BehaviourBase
    {
        public UIPopups(PopupsBehaviour rBasePopups)
        {
            basePopups = rBasePopups;
        }

        /// <summary>
        /// 从原生C#中获得的基类
        /// </summary>
        public PopupsBehaviour basePopups = null;

        /// <summary>
        /// 打开一个弹出面板
        /// </summary>
        public virtual void Show(bool modal = true)
        {
            if(basePopups != null)
            {
                basePopups.Show(modal);
            }
        }

        /// <summary>
        /// 关闭一个弹出面板
        /// </summary>
        public virtual void Hide()
        {
            if (basePopups != null)
            {
                basePopups.Hide();
            }
        }

        public virtual void OnAnimateInEnd() { }

        public virtual void OnAnimateOutEnd() { }

        public virtual void OnReturnCache() { }

    }
}
