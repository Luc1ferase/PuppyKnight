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

    public float LevelMultiplier //������LevelBuff
    {
        get
        {
            return 1 + (currentLevel - 1) * levelBuff;    //�������������2���Ļ�������(2-1)* levelBuff,3������(3-1)* levelBuff,ʵ����Խ������Ҫ����������Խ��
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
        //���������������ݷ���

        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        baseExp += (int)(baseExp * LevelMultiplier);//�����ɹ���,�¸��׶���Ҫ�ľ���ֵ
        maxHealth = (int)(maxHealth * LevelMultiplier);
        currentHealth = maxHealth;  //����֮�����Ѫ��

        Debug.Log("LEVEL UP!" + currentLevel + "MAX Health:" + maxHealth);
    }
}
