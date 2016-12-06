using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace HotFixCode
{
    public class SamplePanel : BehaviourBase
    {
        public SampleLogic logic;
        public Button buttonDialog;
        public Button buttonPopups;
        public Button buttonTween;
        public Button buttonCroutine;
        public Button buttonWaiting;
        public Button buttonPanel;

        protected override void Awake(GameObject rGo)
        {
            base.Awake(rGo);
            buttonDialog = Util.Get<Button>(gameObject, "ButtonDialog");
            buttonPopups = Util.Get<Button>(gameObject, "ButtonPopups");
            buttonCroutine = Util.Get<Button>(gameObject, "ButtonCroutine");
            buttonWaiting = Util.Get<Button>(gameObject, "ButtonWaiting");
            buttonPanel = Util.Get<Button>(gameObject, "ButtonPanel");
        }

    }
}
