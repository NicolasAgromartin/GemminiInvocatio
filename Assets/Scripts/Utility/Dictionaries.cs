using System;
using System.Collections.Generic;

public static class Dictionaries
{
    private readonly static Random statsRange = new();

    public static Dictionary<FiendType, UnitStats> StatsPerFiendType = new()
{
    { FiendType.Skeleton, new UnitStats(
        statsRange.Next(15, 21), // HP medio
        statsRange.Next(6, 10),  // ataque alto (Offensive+)
        statsRange.Next(1, 4),   // defensa baja (Defensive--)
        statsRange.Next(3, 6),   // velocidad rápida
        1
    )},

    { FiendType.Zombie, new UnitStats(
        statsRange.Next(25, 36), // HP alto
        statsRange.Next(5, 9),   // ataque aceptable (Offensive+)
        statsRange.Next(7, 11),  // defensa alta (Defensive++)
        statsRange.Next(1, 3),   // velocidad lenta
        1
    )},

    { FiendType.Creature, new UnitStats(
        statsRange.Next(20, 31), // HP medio-alto
        statsRange.Next(8, 13),  // ataque fuerte (Offensive++)
        statsRange.Next(6, 9),   // defensa buena (Defensive+)
        statsRange.Next(3, 5),   // velocidad balanceada
        1
    )},

    { FiendType.Demon, new UnitStats(
        statsRange.Next(30, 41), // HP alto
        statsRange.Next(12, 18), // ataque muy fuerte (Offensive+++)
        statsRange.Next(9, 14),  // defensa muy fuerte (Defensive+++)
        statsRange.Next(4, 6),   // velocidad rápida
        1
    )}
};


}
