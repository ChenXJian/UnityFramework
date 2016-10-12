using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace HotFixCode
{
    public class SamplePopups : UIPopups
    {
        public SamplePopups(PopupsBehaviour rBasePopups) : base(rBasePopups) { }

        Button btnClose = null;

        protected override void Awake(GameObject rGo)
        {
            base.Awake(rGo);
            btnClose = Util.Get<Button>(gameObject, "ButtonClose");
            btnClose.onClick.AddListener(() =>
            {
                Hide();
            });
        }

        public override void Show(bool modal = true)
        {
            base.Show(modal);
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void OnAnimateInEnd()
        {
            base.OnAnimateInEnd();
        }

        public override void OnAnimateOutEnd()
        {
            base.OnAnimateOutEnd();
        }

        public override void OnReturnCache()
        {
            base.OnReturnCache();
        }
    }
}
