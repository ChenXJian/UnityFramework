using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace HotFixCode
{
    public class SampleTwoPanel : UIPanel
    {
        public SampleTwoLogic logic;
        public Button buttonBack;

        protected override void Awake(GameObject rGo)
        {
            base.Awake(rGo);
            buttonBack = Util.Get<Button>(gameObject, "ButtonBack");
        }

    }
}
