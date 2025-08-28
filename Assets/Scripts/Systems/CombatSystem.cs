using UnityEngine;



public static class CombatSystem 
{
    private static float damageCalculated;

    public static void AttackPerformed(GameObject attacker, GameObject reciever)
    {
        if (attacker.GetComponent<Unit>() == null || reciever.GetComponent<Unit>() == null) return;

        reciever.GetComponent<Unit>().RecieveDamage(CalculateDamage(attacker, reciever));
    }


    private static int CalculateDamage(GameObject attaker, GameObject reciever)
    {
        Debug.Log($"{attaker} attacked {reciever}");


        damageCalculated = attaker.GetComponent<Unit>().Stats.Attack / (reciever.GetComponent<Unit>().Stats.Defense * 10);
        return damageCalculated < 1 ? 1 : (int)damageCalculated;
    }
}
