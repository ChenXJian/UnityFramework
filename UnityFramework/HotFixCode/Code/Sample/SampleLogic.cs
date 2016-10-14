using UnityEngine;
using System.Collections;

namespace HotFixCode
{
    public class SampleLogic : UILogic
    {
        SamplePanel panel = null;
        SamplePopups popupsTest = null;

        #region Example code
        void OnClick(GameObject rGo)
        {
            
            Debug.Log(rGo.name);
            switch (rGo.name)
            {
                case "ButtonDialog":
                    TestDialogBox();
                    break;
                case "ButtonPopups":
                    TestPopups();
                    break;
                case "ButtonCroutine":
                    TestCroutine();
                    break;
                case "ButtonWaiting":
                    TestWaitingLayer();
                    break;
                case "ButtonPanel":
                    TestPanelChange();
                    break;
                default:
                    break;
            }

        }

        void TestPanelChange()
        {
            PanelStack.Instance.PushPanel(LogicName.SampleTwo);
        }

        void TestWaitingLayer()
        {
            WaitingLayer.Show();
            Global.TaskManager.StartTask(WaitingLayerEnumerator());
        }

        void TestDialogBox()
        {
            UIUtil.ShowTips("this is tips");
        }

        void TestPopups()
        {
            PopupsManager.Instance.ShowPopups<SamplePopups>(PopupsName.Sample);
        }

        void TestCroutine()
        {
            Global.TaskManager.StartTask(CroutineEnumerator());
        }

        IEnumerator CroutineEnumerator()
        {
            var count =  0;
            while (count < 50)
            {
                Debug.Log(count);
                count++;
                yield return new WaitForSeconds(1);
            }
        }

        IEnumerator WaitingLayerEnumerator()
        {
            yield return new WaitForSeconds(2);
            WaitingLayer.Hide();
        }
        #endregion

        #region Must funcation

        protected override void Startup(RectTransform parent)
        {
            base.Startup(parent);
            UIUtil.CreateUI(PanelName.Sample, parent, OnCreated);
        }

        protected override void OnCreated(GameObject rGo)
        {
            base.OnCreated(rGo);

            panel = behaviour.GetLShapObject() as SamplePanel;
            panel.logic = this;

            behaviour.AddClick(panel.buttonDialog.gameObject, OnClick);
            behaviour.AddClick(panel.buttonCroutine.gameObject, OnClick);
            behaviour.AddClick(panel.buttonPopups.gameObject, OnClick);
            behaviour.AddClick(panel.buttonWaiting.gameObject, OnClick);
            behaviour.AddClick(panel.buttonPanel.gameObject, OnClick);
            Enable();
        }

        protected override void Enable()
        {
            base.Enable();
        }

        protected override void Disable()
        {
            base.Disable();
        }

        protected override void Free()
        {
            base.Free();
        }
        #endregion

    }
}

