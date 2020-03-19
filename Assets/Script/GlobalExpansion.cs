using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalExpansion
{
    public static Attribute GetSumOfAttributes(Attribute a, Attribute b)
    {
        a.Strenght += b.Strenght;
        a.Intelligence += b.Intelligence;
        a.Constitution += b.Constitution;
        a.Agility += b.Agility;
        a.Lucky += b.Lucky;

        a.Health += b.Health;
        a.Mana += b.Mana;
        a.Attack += b.Attack;
        a.Defense += b.Defense;
        a.HealthRegen += b.HealthRegen;
        a.ManaRegen += b.ManaRegen;

        a.AtkSpeed += b.AtkSpeed;
        a.MoveSpeed += b.MoveSpeed;

        a.CriticalRate += b.CriticalRate;
        a.CriticalDamageRate += b.CriticalDamageRate;
        return a;
    }
    public static Attribute GetSumOfAttributes(Attribute a, EquipmentVO b)
    {
        a.Strenght += b.Strenght;
        a.Intelligence += b.Intelligence;
        a.Constitution += b.Constitution;
        a.Agility += b.Agility;
        a.Lucky += b.Lucky;

        a.Health += b.Health;
        a.Mana += b.Mana;
        a.Attack += b.Attack;
        a.Defense += b.Defense;
        a.HealthRegen += b.HealthRegen;
        a.ManaRegen += b.ManaRegen;

        a.AtkSpeed += b.AtkSpeed;
        a.MoveSpeed += b.MoveSpeed;

        a.CriticalRate += b.CriticalRate;
        a.CriticalDamageRate += b.CriticalDamageRate;
        return a;
    }
}
