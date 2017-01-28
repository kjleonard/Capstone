using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuSettings : MonoBehaviour {

    public Text OverrideText;
    public Text Walkperiod;
    public Text RushPeriod;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

}
