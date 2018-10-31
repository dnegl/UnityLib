using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//SIMPLE AUDIO MANAGER FOR TEST PURPOSES

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioSources;
    public GameObject audioPrefabSource;
    public Dictionary<string,AudioClip> audioClips;
	public static AudioManager audioManager;
    private static GameObject audioPrefab;
    private static GameObject instance;
    private static AudioSource musicPlayer;   
    private Dictionary<string,Audio> aliveSounds = new Dictionary<string, AudioClip>();
    private AudioListener al;
	private const string AudioPath = "Audio/";

    void Awake()
    {
		audioManager = this;
		al = GetComponent<AudioListener>();
		audioClips 
		foreach(AudioClip a in audioSources) 
		{
			audioClips.Add(a.name, a);
		}

		instance = this.gameObject;
		audioPrefab = audioPrefabSource;
		musicPlayer = audio;
		aliveSounds = new Dictionary<string, Audio>();
    }

    void Update()
    {
		if(!GameSetting.hasMusic) 
		{
			musicPlayer.Pause();
		} 
		else 
		{
			if(!musicPlayer.isPlaying) 
			{
				musicPlayer.Play();
			}
		}
		if(!GameSetting.hasSound && aliveSounds.Count > 0) 
		{
			foreach(Audio a in aliveSounds.Values) 
			{
				a.StopSound ();
			}
			aliveSounds.Clear ();
		}
		if(!al.enabled) 
		{
			al.enabled = true;
		}
    }

    public static void PlaySoundOnce(string name)
    {
		if(!GameSetting.hasSound) 
		{
			return;
		}
		if(!audioManager.audioClips.ContainsKey(name)) 
		{
			return;
		}
		GameObject go = GameObject.Instantiate(audioPrefab) as GameObject;
		go.transform.parent = instance.transform;
		Audio a = go.GetComponent<Audio>();
		a.PlaySoundOnce(audioManager.audioClips[name]);
    }


    public static void PlayMusic (string name)
    {
		if(!GameSetting.hasMusic) 
		{
			return;
		}

		if(musicPlayer.clip == null || musicPlayer.clip.name != name) 
		{
			musicPlayer.clip = Resources.Load(AudioPath + name, typeof(AudioClip)) as AudioClip;
			musicPlayer.Stop();
			musicPlayer.loop = true;
			musicPlayer.Play();
		} 
		else 
		{
			musicPlayer.loop = true;
			musicPlayer.Play();
		}
    }
}