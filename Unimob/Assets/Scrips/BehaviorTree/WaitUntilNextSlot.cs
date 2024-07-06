using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Category("✫ Utility")]
public class WaitUntilNextSlot : ActionTask
{
    public BBParameter<BaseCustomer> Customer;
    public CompactStatus finishStatus = CompactStatus.Success;

    protected override string info
    {
        get { return string.Format("Wait next slot"); }
    }

    protected override void OnUpdate()
    {
        if (Customer.value.CheckNextSlot())
        {
            EndAction(true);
        }
    }
}
