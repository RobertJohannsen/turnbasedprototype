using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class plyAttacks : MonoBehaviour
{
    public GameObject target , pointerUI ,pointerUItext , attDisp , desDisp;
    public GameObject[] attWhere;
    public GameObject[] attQDisp;
    public enum attacks { none, attShoot, attMelee, attSpray, attReload };
    public attacks[] attQueue;
    public int ammo, clip, apCost, baseDmg, index, basecrit, baseacc, attIndex, attQueueLength ,apCostTotal;
    public bool aimMode , selectAtt ,beginAtt ,canAttack;
    public int whatPart , _apCost;
    public string _attDispText, _attDesText;
    public int _dmg, _ap;
    public float _critChance;
    // Start is called before the first frame update
    void Start()
    {
        canAttack = false;
        beginAtt = false;
        selectAtt = false;
        aimMode = false;
        attQueueLength = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            beginAtt = !beginAtt;
        }

        if(beginAtt)
        {
            
            AssignAttacks();
            UpdateDisp();
        }
        else
        {
            attDisp.GetComponent<TextMeshPro>().text = _attDispText;
            desDisp.GetComponent<TextMeshPro>().text = _attDesText;

            _attDispText = "";
            _attDesText =
                "";
        }

     

    }

    void attShoot()
    {
            
         baseDmg = 5;
         apCost = 2;
        baseacc = 20;
        basecrit = 10;

    }
    
    void attMelee()
    {
        baseDmg = 2;
         apCost = 1;
        basecrit = 40;
    }

    void attSpray()
    {
         baseDmg = 25;
         apCost = 8;
        basecrit = 5;
    }

    void attReload()
    {
       // GetComponentInParent<stats>().currentap += 2;

    }

   public  void loadAtt()
    {
        switch (attQueue[index])
        {
            case attacks.attShoot:
                attShoot();
                break;
            case attacks.attMelee:
                attMelee();
                break;
            case attacks.attSpray:
                attSpray();
                break;
            case attacks.attReload:
                attReload();
                break;

        }
    }

    void AssignAttacks()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            attIndex += 1;
            if(aimMode)
            {
                whatPart += 1;
            }
           
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            attIndex -= 1;
            if(aimMode)
            {
                whatPart -= 1;
            }
           
        }

        attIndex = Mathf.Clamp(attIndex, 1, 4);
        whatPart = Mathf.Clamp(whatPart, 0, 7);

        if (!selectAtt)
        {

            //select part here
            if (aimMode)
            {

                int _accMod = GetComponentInParent<stats>().acc;

                
                pointerUI.transform.position = new Vector3(target.GetComponent<stats>().bodyPart[whatPart].transform.position.x, target.GetComponent<stats>().bodyPart[whatPart].transform.position.y, target.GetComponent<stats>().bodyPart[whatPart].transform.position.z - 1);

                switch (target.GetComponent<stats>().bodyPart[whatPart].tag)
                {
                    case "head":
                        baseacc = target.GetComponent<stats>().bodyPart[whatPart].GetComponent<headBehaviour>().baseAcc;
                         break;
                    case "chest":
                        baseacc = target.GetComponent<stats>().bodyPart[whatPart].GetComponent<chestBehaviour>().baseAcc;
                        break;
                    case "arm":
                        baseacc = target.GetComponent<stats>().bodyPart[whatPart].GetComponent<armBehaviour>().baseAcc;
                        break;
                    case "leg":
                        baseacc = target.GetComponent<stats>().bodyPart[whatPart].GetComponent <legBehaviour>().baseAcc;
                        break;
                    case "hat":
                        baseacc = target.GetComponent<stats>().bodyPart[whatPart].GetComponent<hatBehaviour>().baseAcc;
                        break;
                    case "weapon":
                        baseacc = target.GetComponent<stats>().bodyPart[whatPart].GetComponent<weaponBehaviour>().baseAcc;
                        break;
                }
                

                int _finalAcc = Mathf.Clamp((baseacc + (_accMod * 2)), 0, 100);

                pointerUItext.GetComponent<TextMeshPro>().text = _finalAcc+"%";

                //select part here ends

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    aimMode = false;
                    selectAtt = true;

                }

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                aimMode = true;
            }


         
        }
        else
        {

            aimMode = false;

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                selectAtt = false;
                aimMode = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                canAttack = true;

                //attacks are submitted here



                attWhere[attQueueLength] = target.GetComponent<stats>().bodyPart[whatPart];


                if (apCostTotal <= this.GetComponent<stats>().currentap)
                {
                 
                    switch (attIndex)
                    {

                        case 1:
                            attQueue[attQueueLength] = attacks.attShoot;
                            attShoot();
                            _apCost = apCost; //I promise this will make sense
                            apCostTotal += apCost;
                            attQDisp[attQueueLength].GetComponent<TextMeshPro>().text = ">shoot";

                            attQueueLength += 1;
                            break;
                        case 2:
                            attQueue[attQueueLength] = attacks.attMelee;
                            attMelee();
                            _apCost = apCost; //I promise this will make sense
                            apCostTotal += apCost;
                            attQDisp[attQueueLength].GetComponent<TextMeshPro>().text = ">melee";
                            attQueueLength += 1;
                            break;
                        case 3:
                            attQueue[attQueueLength] = attacks.attSpray;
                            attSpray();
                            _apCost = apCost; //I promise this will make sense
                            apCostTotal += apCost;
                            attQDisp[attQueueLength].GetComponent<TextMeshPro>().text = ">spray";
                            attQueueLength += 1;
                            break;
                        case 4:
                            attQueue[attQueueLength] = attacks.attReload;
                            attReload();
                            _apCost = apCost; //I promise this will make sense
                            apCostTotal += apCost;
                            attQDisp[attQueueLength].GetComponent<TextMeshPro>().text = ">rest";
                            attQueueLength += 1;
                            break;



                    }

                    if(apCostTotal >= this.GetComponent<stats>().currentap)
                    {
                        attQueueLength -= 1;
                        attQueue[attQueueLength] = attacks.none;
                        apCostTotal -= _apCost;

                        //reduce apcost back to thing because attack cant register

                    }
                }
                else
                {
                    //cant submit attacks
                }
               

                
            }

          
        }



       

    }

    void UpdateDisp()
    {

        switch (attIndex)
        {
            case 1:
                attShoot();
                _dmg = baseDmg;
                _ap = apCost;
                _critChance = Mathf.Clamp(((basecrit / 100) + 1) * (GetComponentInParent<stats>().critmod + 25), 0, 100);


                _attDispText = ">Shoot<";
                _attDesText = 
                    "Shoot the target \n"
                    +"AP cost : " + _ap+ "\n"
                    +"Damage : "+_dmg+"\n"
                    +"Crit Chance : "+_critChance+"%" ;
                break;   
            case 2:

                attMelee();
                _dmg = baseDmg;
                _ap = apCost;
                _critChance = Mathf.Clamp(((basecrit / 100) + 1) * (GetComponentInParent<stats>().critmod + 25), 0, 100);

                _attDispText = ">Melee<";
                _attDesText =
                   "Stab the target \n"
                   + "AP cost : " + _ap + "\n"
                   + "Damage : " + _dmg + "\n"
                   + "Crit Chance : " + _critChance + "%";
                break;
            case 3:

                attSpray();
                _dmg = baseDmg;
                _ap = apCost;
                _critChance = Mathf.Clamp(((basecrit / 100) + 1) * (GetComponentInParent<stats>().critmod + 25), 0, 100);

                _attDispText = ">Spray<";
                _attDesText =
                   "Shoot the target multiple times \n"
                   + "AP cost : " + _ap + "\n"
                   + "Damage : " + _dmg + "\n"
                   + "Crit Chance : " + _critChance + "%";
                break;
            case 4:

                attReload();
                _ap = apCost;
                _attDispText = ">Rest<";
                _attDesText =
                   "Rest to gain 2 AP\n"
                   + "AP cost : " + _ap
                  ;
                break;




        }
        attDisp.GetComponent<TextMeshPro>().text = _attDispText;
        desDisp.GetComponent<TextMeshPro>().text = _attDesText;


    }
}
