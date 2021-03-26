using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class combatLoop : MonoBehaviour
{
    public GameObject attackObj1 , attackObj2;
    public bool hasInitiative;
    public GameObject attacker, target;
    public int combatPhase,attIndex , _baseAtt ,_baseAcc ,_baseCrit;
    public GameObject[] combatLog;
    public int combatLogIndex, _attQueueLength;
    public string[] combatMsg;
    public int waitTimer ,waitThreshold ,timerCount;
    public bool waitTimerActive; 

    // Start is called before the first frame update
    void Start()
    {
        combatLogIndex = 0;
        combatPhase = 1;
        EvaluateTurn(attackObj1 , attackObj2);
    }

    // Update is called once per frame
    void Update()
    {
        if(attackObj1.GetComponent<plyAttacks>().canAttack)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                doTurn();
            }



            if (waitTimerActive)
            {
                waitTimer++;
            }





            if (waitTimer > waitThreshold)
            {

                waitTimer = 0;
                timerCount++;

            }

            switch (timerCount)
            {
                case 1:
                    combatLog[0].GetComponent<TextMeshPro>().text = combatMsg[0];
                    break;
                case 2:
                    combatLog[1].GetComponent<TextMeshPro>().text = combatMsg[1];
                    break;
                case 3:
                    combatLog[2].GetComponent<TextMeshPro>().text = combatMsg[2];
                    break;
            }

            if (timerCount == 3)
            {
                waitTimerActive = false;
            }

        }




    }

    void EvaluateTurn(GameObject obj1 , GameObject obj2)
    {
        stats _objstats1 = obj1.GetComponent<stats>();
        stats _objstats2 = obj2.GetComponent<stats>();

        int _agi1 = _objstats1.agi;
        int _agi2 = _objstats2.agi;


        if(combatPhase == 1)
        {
            if (_agi1 > _agi2)
            {
                attacker = attackObj1;
                target = attackObj2;
            }
            else
            {
                attacker = attackObj2;
                target = attackObj1;
            }
        }
        else
        {
            if (_agi1 > _agi2)
            {
                attacker = attackObj2;
                target = attackObj1;
            }
            else
            {
                attacker = attackObj1;
                target = attackObj2;
            }
        }
      

      

    }

    void HandleDirectAttacks()
    {


        //get attacker stats
        float _crit;
        int _HP = attacker.GetComponent<stats>().hp;
        int _attMod = attacker.GetComponent<stats>().att;
        int _critmod = attacker.GetComponent<stats>().critmod;
        int _accMod = attacker.GetComponent<stats>().acc;

        if(attacker.tag == "ply")
        {
            plyAttacks _attacks = attacker.GetComponent<plyAttacks>();

            _baseAtt = _attacks.baseDmg;
            _baseCrit = _attacks.basecrit;
           
        }
        else
        {
            attacks _attacks = attacker.GetComponent<attacks>();

            _baseAtt = _attacks.baseDmg;
            _baseCrit = _attacks.basecrit;
            _baseAcc = _attacks.baseacc;
        }


        if(target.GetComponent<attacks>())
        {
            // sets the accuracy to be based on what part is being attacked
            switch (target.GetComponent<stats>().bodyPart[attIndex].tag)
            {
                case "head":
                    _baseAcc = target.GetComponent<stats>().bodyPart[attIndex].GetComponent<headBehaviour>().baseAcc;
                    break;
                case "chest":
                    _baseAcc = target.GetComponent<stats>().bodyPart[attIndex].GetComponent<chestBehaviour>().baseAcc;
                    break;
                case "arm":
                    _baseAcc = target.GetComponent<stats>().bodyPart[attIndex].GetComponent<armBehaviour>().baseAcc;
                    break;
                case "leg":
                    _baseAcc = target.GetComponent<stats>().bodyPart[attIndex].GetComponent<legBehaviour>().baseAcc;
                    break;
                case "hat":
                    _baseAcc = target.GetComponent<stats>().bodyPart[attIndex].GetComponent<hatBehaviour>().baseAcc;
                    break;
                case "weapon":
                    _baseAcc = target.GetComponent<stats>().bodyPart[attIndex].GetComponent<weaponBehaviour>().baseAcc;
                    break;
            }

        }

        int _targetArm = target.GetComponent<stats>().armor;


        int _finalAcc = Mathf.Clamp((_baseAcc + (_accMod * 2)), 0, 100);

        int accChance = Random.Range(1, 100);
       

        if (accChance < _finalAcc)
        {

            if (_baseCrit != 0)
            {
                float _critChance = Mathf.Clamp(((_baseCrit / 100) + 1) * (_critmod + 25), 0, 100);

                int ccRoll = Random.Range(1, 100);
                if (Random.Range(1, 100) <= _critChance)
                {
                    Debug.Log("crit");
                    _crit = 1.5f;
                }
                else
                {
                    _crit = 1;
                }
            }
            else
            {
                _crit = 1;
            }

            int _bonusdmg = (_attMod / 10) * (_baseAtt/2);
            int _dmg = (int)((_baseAtt + _bonusdmg) * (_crit));

            _dmg = Mathf.Clamp((_dmg - _targetArm), 0, 200);

            // damage section

            // damages the part you selected
            if (target.GetComponent<attacks>())
            {
                switch (target.GetComponent<stats>().bodyPart[attIndex].tag)
                {
                    case "head":
                        target.GetComponent<stats>().bodyPart[attIndex].GetComponent<headBehaviour>().partHP -= _dmg;
                        break;
                    case "chest":
                        target.GetComponent<stats>().bodyPart[attIndex].GetComponent<chestBehaviour>().partHP -= _dmg;
                        break;
                    case "arm":
                        target.GetComponent<stats>().bodyPart[attIndex].GetComponent<armBehaviour>().partHP -= _dmg;
                        break;
                    case "leg":
                        target.GetComponent<stats>().bodyPart[attIndex].GetComponent<legBehaviour>().partHP -= _dmg;
                        break;
                    case "hat":
                        target.GetComponent<stats>().bodyPart[attIndex].GetComponent<hatBehaviour>().partHP -= _dmg;
                        break;
                    case "weapon":
                        target.GetComponent<stats>().bodyPart[attIndex].GetComponent<weaponBehaviour>().partHP -= _dmg;
                        break;
                }

                target.GetComponent<stats>().hp -= _dmg;
            }

            if(target.tag == "ply")
            {
                target.GetComponent<stats>().hp -= _dmg;
            }
         


            //end of damage section


            if(_crit == 1)
            {
                combatMsg[combatLogIndex] = attacker.name + " hit " + target.name + " for " + _dmg;

            }
            else
            {
                combatMsg[combatLogIndex] = attacker.name + " crit hit " + target.name + " for " + _dmg;
            }

        }
        else
        {
            combatMsg[combatLogIndex] = attacker.name + " missed";
            // attack misses
        }

        combatLogIndex += 1;

        if (combatLogIndex > 2)
        {
            combatLogIndex = 0;
        }

        waitTimerActive = true;






    }

    void doTurn()
    {
        if(attacker.GetComponent<plyAttacks>())
        {
             _attQueueLength = attacker.GetComponent<plyAttacks>().attQueueLength;

        }
        else if(attacker.GetComponent<attacks>())
        {
            _attQueueLength = 1;
        }


        combatPhase = 1;
        EvaluateTurn(attackObj1, attackObj2);
        //do attack 1

        //go here

        //attIndex is the index for the attack and also links the body part
        for(attIndex = 0; attIndex < _attQueueLength; attIndex++)
        {
            attacker.GetComponent<plyAttacks>().index = attIndex;
            attacker.GetComponent<plyAttacks>().loadAtt();

            HandleDirectAttacks(); // this does the damage
        }

        attacker.GetComponent<plyAttacks>().attQueueLength = 0 ;
        attacker.GetComponent<plyAttacks>().index = 0;

        if(attacker.tag == "ply")
        {
            attacker.GetComponent<stats>().currentap -= attacker.GetComponent<plyAttacks>().apCostTotal;
            attacker.GetComponent<plyAttacks>().apCostTotal = 0;

        }

        
        attacker.GetComponent<stats>().currentap = Mathf.Clamp(attacker.GetComponent<stats>().currentap += 2, 0, attacker.GetComponent<stats>().maxap);


        target.GetComponent<stats>().hp = Mathf.Clamp(target.GetComponent<stats>().hp, 0, 10000);

        if (target.GetComponent<stats>().hp != 0)
        {
            //
            combatPhase = 2;
            EvaluateTurn(attackObj1, attackObj2);
            HandleDirectAttacks();

            target.GetComponent<stats>().hp = Mathf.Clamp(target.GetComponent<stats>().hp, 0, 10000);

            if (target.GetComponent<stats>().hp == 0)
            {
                //player

                combatMsg[combatLogIndex] = "player has died";
                combatLogIndex++;

            }

        }
        else
        {

            combatMsg[combatLogIndex] = "enemy has died";
            combatLogIndex++;

        }

        combatPhase = 1;
        combatMsg[combatLogIndex] = "combat over";

        attackObj1.GetComponent<plyAttacks>().canAttack = false;

        if (target.GetComponent<plyAttacks>())
        {
            target.GetComponent<plyAttacks>().apCostTotal = 0;
            target.GetComponent<plyAttacks>().attQueueLength = 0 ;
            target.GetComponent<plyAttacks>().baseacc = 0;
            target.GetComponent<plyAttacks>().baseDmg = 0;
            target.GetComponent<plyAttacks>().basecrit = 0;

            target.GetComponent<plyAttacks>().attQueue[0] = plyAttacks.attacks.none;
            target.GetComponent<plyAttacks>().attQueue[1] = plyAttacks.attacks.none;
            target.GetComponent<plyAttacks>().attQueue[2] = plyAttacks.attacks.none;

            target.GetComponent<plyAttacks>().attQDisp[0].GetComponent<TextMeshPro>().text = "";
            target.GetComponent<plyAttacks>().attQDisp[1].GetComponent<TextMeshPro>().text = "";
            target.GetComponent<plyAttacks>().attQDisp[2].GetComponent<TextMeshPro>().text = "";



        }
    }

   

    
   
}
