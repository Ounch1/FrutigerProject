using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleItem : Item
{
    public override void UseItem()
    {
        print("Using item" + itemName);
    }
}
