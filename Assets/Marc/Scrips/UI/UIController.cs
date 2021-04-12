using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    TMP_Text WaveCount;
    TMP_Text Money;
    TMP_Text Life;

    public int InitialLife;
    public int InitialMoney;

    public int currentMoney;
    private int currentLife;
    private int currentWave;
    // Start is called before the first frame update
    void Awake()
    {
        WaveCount = transform.Find("Wave").transform.Find("WaveCount").GetComponent<TMP_Text>();
        Money = transform.Find("Money").transform.Find("MoneyNumber").GetComponent<TMP_Text>();
        Life = transform.Find("Life").transform.Find("LifeNumber").GetComponent<TMP_Text>();

        currentLife = InitialLife;
        currentMoney = InitialMoney;

        WaveCount.SetText(currentWave.ToString());
        Money.SetText(currentMoney.ToString());
        Life.SetText(currentLife.ToString());
    }

    public void UpdateWave(int CurrentWave)
    {
        WaveCount.SetText(CurrentWave.ToString());
    }

    public void AddMoney(int MoneyToAdd)
    {
        currentMoney += MoneyToAdd;
        Money.SetText(currentMoney.ToString());
    }

    public void UpdateLife(int Damage)
    {
        currentLife -= Damage;
        Life.SetText(currentLife.ToString());

        if(currentLife <= 0)
        {
            Debug.Log("You Are Dead, fucking noob");
        }
    }
}
