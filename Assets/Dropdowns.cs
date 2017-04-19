﻿using System.Collections;
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
        PlayerPrefs.SetInt(x.name, x.value);
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
