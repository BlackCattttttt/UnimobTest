using NodeCanvas.Framework;
using ParadoxNotion;
using System.ComponentModel;
using UnityEngine;

namespace MainGame.Scripts.IdleGame.BehaviourTree.Actions
{
    [Category("✫ Utility")]
    public class WaitUntilReceiveOrder : ActionTask
    {
        public BBParameter<BaseCustomer> Customer;
        public CompactStatus finishStatus = CompactStatus.Success;

        protected override string info
        {
            get { return string.Format("Wait receive order."); }
        }

        protected override void OnUpdate()
        {
            if (!Customer.value.CheckHasOrder())
            {
                EndAction(finishStatus == CompactStatus.Success ? true : false);
            }
        }
    }
}
