using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dropdowns : MonoBehaviour {

    // PlayerPrefs can be found in the following path:
    // RegEdit -> HKEY_CURRENT_USER -> SOFTWARE -> Unity -> UnityEditor -> android -> game

   public void clearValue(string x)
    {
        PlayerPrefs.SetInt(x, 0);
    }

    public void setValue(Dropdown x)
    {
		if (x.name == "selDuration") {
			int durationPref = x.value;

			if (durationPref == 0) {
				durationPref = 1;
			} else if (durationPref == 1) {
				durationPref = 2;
			} else if (durationPref == 2) {
				durationPref = 4;
			} else if (durationPref == 3) {
				durationPref = 5;
			} else if (durationPref == 4) {
				durationPref = 6;
			} else if (durationPref == 5) {
				durationPref = 8;
			} else if (durationPref == 6) {
				durationPref = 10;
			}

			int freq = PlayerPrefs.GetInt("selObstacleFrequency");

			durationPref *= 60000;
			if (freq == 0) {
				durationPref += 15000;
			} else if (freq == 1) {
				durationPref += 10000;
			} else {
				durationPref += 6000;
			}
			PlayerPrefs.SetInt (x.name, durationPref);

		} else {
			PlayerPrefs.SetInt (x.name, x.value);
		}
    }

	public void setFloat(Dropdown x)
	{
		int menuIndex = x.GetComponent<Dropdown> ().value;
		List<Dropdown.OptionData> menuOptions = x.GetComponent<Dropdown> ().options;
		string value = menuOptions [menuIndex].text;
		float f;
		float.TryParse (value, out f);
		PlayerPrefs.SetFloat(x.name, f);
	}

    public void getValue(Dropdown x)
    {
        x.value = PlayerPrefs.GetInt(x.name);
    }

    public float getFloat(Dropdown x)
    {
        return PlayerPrefs.GetFloat(x.name);
    }

}
