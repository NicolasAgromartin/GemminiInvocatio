using UnityEngine;



public static class CombatSystem 
{
    public static void AttackPerformed(GameObject attacker, GameObject reciever)
    {
        Debug.Log($"{attacker} attacked {reciever}");
        if (attacker.GetComponent<Unit>() == null || reciever.GetComponent<Unit>() == null) return;

        reciever.GetComponent<Unit>().RecieveDamage(CalculateDamage(attacker, reciever));
    }


    private static int CalculateDamage(GameObject attacker, GameObject reciever)
    {
        //Debug.Log(attacker.GetComponent<Unit>().Stats.Attack);
        return attacker.GetComponent<Unit>().Stats.Attack;
    }
}
