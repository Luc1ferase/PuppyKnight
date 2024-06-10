using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    TextMeshProUGUI levelText;
    //Text levelText;
    Image healthSlider;
    Image expSlider;

    void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        levelText.text = "Level " + GameManager.Instance.playerStats.characterData.currentLevel; //overload ToString方法,使得等级显示为01,02这种
        UpdateHealth();
        UpdateExp();

    }
    void UpdateHealth()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    void UpdateExp()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.characterData.currentExp / GameManager.Instance.playerStats.characterData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }
}
