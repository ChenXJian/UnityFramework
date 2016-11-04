using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour 
{
    public AudioSource backsoundSource = null;

    private Dictionary<string, AudioClip> clipCache = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioSource> effectsounCache = new Dictionary<string, AudioSource>();

    void Awake()
    {
        this.backsoundSource = GetComponent<AudioSource>();
        if (backsoundSource == null)
        {
            backsoundSource = gameObject.AddComponent<AudioSource>();
            gameObject.AddComponent<AudioListener>();
        }
    }

    /// <summary>
    /// ‘ÿ»Î“ª∏ˆ“Ù∆µ
    /// </summary>
    public AudioClip LoadAudioClip(string name)
    {

        AudioClip ac = null;
        clipCache.TryGetValue(name, out ac);
        if (ac == null)
        {
            ac = (AudioClip)Resources.Load("Sound/" + name, typeof(AudioClip));
            if (ac != null) clipCache.Add(name, ac);
        }

        return ac;
    }

    /// <summary>
    /// ≤•∑≈±≥æ∞“Ù¿÷
    /// </summary>
    public void PlayBacksound(string name, bool canPlay = true)
    {
        if (this.backsoundSource.clip != null)
        {
            if (name.IndexOf(this.backsoundSource.clip.name) > -1)
            {
                if (!canPlay)
                {
                    this.backsoundSource.Stop();
                    this.backsoundSource.clip = null;
                }
                return;
            }
        }

        if (canPlay)
        {
            this.backsoundSource.loop = true;
            this.backsoundSource.clip = LoadAudioClip(name);
            this.backsoundSource.Play();
        }
        else
        {
            this.backsoundSource.Stop();
            this.backsoundSource.clip = null;
        }
    }

    /// <summary>
    /// ≤•∑≈“Ù–ß
    /// </summary>
    public void PlayEffectsound(string name)
    {
        if (effectsounCache.ContainsKey(name))
        {
            //∂≈æ¯÷ÿ∏¥≤•∑≈
            return;
        }

        GameObject gameObject = new GameObject("sound effect");
        gameObject.transform.position = Vector3.zero;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));

        var clip = LoadAudioClip(name);
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 1;
        audioSource.Play();
        effectsounCache.Add(name, audioSource);

        var clearTime = clip.length * ((Time.timeScale >= 0.01f) ? Time.timeScale : 0.01f);

        Util.DelayCall(gameObject, clearTime, (go) =>
        {
            GameObject.Destroy(go as GameObject);
            effectsounCache.Remove(name);
        });
    }

    public void Play(string name)
    {
        AudioSource.PlayClipAtPoint(LoadAudioClip(name), new Vector3(), 1);
    }
}
