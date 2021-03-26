using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attacks : MonoBehaviour
{

    public enum atts{ none };
    public atts[] attQueue;
    public int ammo, clip, apCost, baseDmg, index, basecrit, baseacc, attIndex, attQueueLength;

    // Start is called before the first frame update
    void Start()
    {
        doAttack();
    }

    // Update is called once per frame
    void Update()
    {
        if (attIndex > 1)
        {
            attIndex = 1;
        }
    }

    public void doAttack()
    {
        attIndex = 1;
        baseDmg = 7;
        apCost = 2;
        basecrit = 40;
        baseacc = 50;

           
    }
}
