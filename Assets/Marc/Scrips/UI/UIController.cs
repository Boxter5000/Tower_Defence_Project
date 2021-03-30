using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    TMP_Text WaveCount;
    // Start is called before the first frame update
    void Awake()
    {
        WaveCount = transform.Find("WaveCount").GetComponent<TMP_Text>();
    }

    public void UpdateWave(int CurrentWave)
    {
        WaveCount.SetText(CurrentWave.ToString());
    }
}
