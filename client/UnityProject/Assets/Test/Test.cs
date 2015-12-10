using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
        string selectedLanguage = "English (US)"; // The default language.

        Localization.Language = selectedLanguage;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
