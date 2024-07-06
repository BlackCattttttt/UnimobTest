using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Category("✫ Blackboard")]
[Description("Check customer first wait slot!")]
public class CheckCustomerFirstSlot : ConditionTask
{
    public BBParameter<BaseCustomer> Customer;

    protected override string info => $"customer in first slot";

    protected override bool OnCheck()
    {
        return Customer.value.CheckFirstSlot();
    }
}
