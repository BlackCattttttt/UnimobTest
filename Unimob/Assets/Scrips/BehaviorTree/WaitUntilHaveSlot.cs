using NodeCanvas.Framework;
using ParadoxNotion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitUntilHaveSlot : ActionTask
{
    public BBParameter<BaseCustomer> Customer;
    public CompactStatus finishStatus = CompactStatus.Success;

    protected override void OnUpdate()
    {
        if (Customer.value.CheckHaveSlot())
        {
            EndAction(true);
        }
    }
}
