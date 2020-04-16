using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class GlobalExpansion
{
    /// <summary>
    /// 属性对象加属性对象
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
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
    /// <summary>
    /// 属性对象加装备对象
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
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
    /// <summary>
    /// 时间坐标转到FGUI
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Vector2 WorldPos2FguiPos(Vector3 pos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        //原点位置转换
        screenPos.y = Screen.height - screenPos.y;
        return GRoot.inst.GlobalToLocal(screenPos);
    }
    public static bool IsInFguiScreen(Vector2 pos)
    {
        return (pos.x > 0 && pos.x < Stage.inst.stageWidth) && (pos.y > 0 && pos.y < Stage.inst.stageHeight);
    }
    public static void AttributeCopy(Attribute to, Attribute from)
    {
        to.Strenght = from.Strenght;
        to.Intelligence = from.Intelligence;
        to.Constitution = from.Constitution;
        to.Agility = from.Agility;
        to.Lucky = from.Lucky;

        to.Health = from.Health;
        to.Mana = from.Mana;
        to.Attack = from.Attack;
        to.Defense = from.Defense;
        to.HealthRegen = from.HealthRegen;
        to.ManaRegen = from.ManaRegen;

        to.AtkSpeed = from.AtkSpeed;
        to.MoveSpeed = from.MoveSpeed;

        to.CriticalRate = from.CriticalRate;
        to.CriticalDamageRate = from.CriticalDamageRate;
    }
}
