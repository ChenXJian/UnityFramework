using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Localize : MonoBehaviour {
	
	public string Key;

	void Awake()
	{
		LoadKey();
	}

	public void LoadKey()
	{
		if (this.GetComponent<Text>() != null) 
		{
            this.GetComponent<Text>().text = Localization.Get(this.gameObject, Key);
		}
		else if (this.GetComponent<Image>() != null)
		{
            var filePath = Localization.Get(this.gameObject, "FilePath");
            Sprite tempSprite = Resources.Load<Sprite>(filePath + Localization.Get(this.gameObject, Key));
			this.GetComponent<Image>().sprite = tempSprite;
		}
		else if (this.GetComponent<RawImage>() != null) 
		{
            var filePath = Localization.Get(this.gameObject, "FilePath");
            Texture tempTexture = Resources.Load<Texture>(filePath + Localization.Get(this.gameObject, Key));
			this.GetComponent<RawImage>().texture = tempTexture;
		}
	}
}
