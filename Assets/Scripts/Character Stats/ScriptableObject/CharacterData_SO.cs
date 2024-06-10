using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]

    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;

    [Header("Kill")]

    public int killPoint;


    [Header("Level")]

    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;

    public float LevelMultiplier //逐级提升LevelBuff
    {
        get
        {
            return 1 + (currentLevel - 1) * levelBuff;    //如果现在升级到2级的话，就是(2-1)* levelBuff,3级就是(3-1)* levelBuff,实现了越往后需要的升级经验越多
        }
    }

    public void UpdateExp(int point)
    {
        currentExp += point;
        if (currentExp >= baseExp)
            LevelUp();
    }

    private void LevelUp()
    {
        //所有想提升的数据方法

        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        baseExp += (int)(baseExp * LevelMultiplier);//升级成功后,下个阶段需要的经验值
        maxHealth = (int)(maxHealth * LevelMultiplier);
        currentHealth = maxHealth;  //升级之后回满血量

        Debug.Log("LEVEL UP!" + currentLevel + "MAX Health:" + maxHealth);
    }
}
