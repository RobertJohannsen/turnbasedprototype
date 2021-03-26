using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dispStats : MonoBehaviour
{
    public GameObject HPBar , APBar ,ARBar;
    public stats objStats;
    public string angerStrg;

    // Start is called before the first frame update
    void Start()
    {
        objStats = GetComponentInParent<stats>();
    }

    // Update is called once per frame
    void Update()
    {
        
        HPBar.GetComponent<TextMeshPro>().text = "HP : " + objStats.hp+"/"+ objStats.maxhp + "";
        APBar.GetComponent<TextMeshPro>().text = "AP : " + objStats.currentap + "/" + objStats.maxap + angerStrg;
        ARBar.GetComponent<TextMeshPro>().text = "AR : " + objStats.armor;

        if (GetComponentInParent<plyAttacks>())
        {
            if(GetComponentInParent<plyAttacks>().apCostTotal != 0)
            {
                angerStrg = " -(" + GetComponentInParent<plyAttacks>().apCostTotal + ")";
            }
            else
            {
                angerStrg = "";
            }
            
        }
    }
}
