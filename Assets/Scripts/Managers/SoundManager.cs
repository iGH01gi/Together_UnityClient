﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[Define.Sound.GetNames(typeof(Define.Sound)).Length];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
	        for (int i = 0; i < soundNames.Length; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }
            _audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effects, AudioSource audioSource = null, float pitch = 1.0f)
    {
	    switch (type)
	    {
		    case Define.Sound.Bgm:
				path = String.Concat("Bgm/", path);
			    break;
		    case Define.Sound.Effects:
			    path = String.Concat("Effects/", path);
			    break;
		    case Define.Sound.Heartbeat:
			    path = String.Concat("Bgm/", "Heartbeat");
			    break;
	    }
	    AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, audioSource, pitch);
    }

	public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effects, AudioSource audioSource = null, float pitch = 1.0f)
	{
		if (audioClip == null)	
            return;


		if (audioSource == null)
		{
			switch (type)
			{
				case Define.Sound.Bgm:
				{
					audioSource = _audioSources[(int)Define.Sound.Bgm];
					if (audioSource.isPlaying)
						audioSource.Stop();

					audioSource.pitch = pitch;
					audioSource.clip = audioClip;
					audioSource.Play();
					break;
				}
				case Define.Sound.Effects:
					audioSource = _audioSources[(int)Define.Sound.Effects];
					audioSource.pitch = pitch;
					audioSource.PlayOneShot(audioClip);
					break;
			}
		}
		else
		{
			audioSource.pitch = pitch;
			audioSource.PlayOneShot(audioClip);
		}
	}

	public void ChangeAudioVolume(Define.Sound type, float volume)
	{
		_audioSources[(int)type].volume = volume;
	}

	AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effects)
    {
		if (path.Contains("Sounds/") == false)
			path = $"Sounds/{path}";

		AudioClip audioClip = null;

		if (type == Define.Sound.Bgm)
		{
			audioClip = Managers.Resource.Load<AudioClip>(path);
		}
		else
		{
			if (_audioClips.TryGetValue(path, out audioClip) == false)
			{
				audioClip = Managers.Resource.Load<AudioClip>(path);
				_audioClips.Add(path, audioClip);
			}
		}

		if (audioClip == null)
			Debug.Log($"AudioClip Missing ! {path}");

		return audioClip;
    }

	public IEnumerator FadeIn(Define.Sound type, string path, float fadeTime = 1.0f, float fadeDuration = 0.1f)
	{
		AudioSource audioSource = _audioSources[(int)type];
		float startVolume = 0.0f;
		float endVolume = audioSource.volume;
		float time = 0.0f;

		audioSource.volume = startVolume;
		Play(path, type);

		while (time < fadeTime)
		{
			float volume = Mathf.Lerp(startVolume, endVolume, time / fadeTime);
			audioSource.volume = volume;
			time += fadeDuration;
			yield return new WaitForSeconds(fadeDuration);
		}
	}
	
	public IEnumerator FadeOut(Define.Sound type, float fadeTime = 1.0f, float fadeDuration = 0.1f)
	{
		AudioSource audioSource = _audioSources[(int)type];
		float endVolume = 0.0f;
		float startVolume = audioSource.volume;
		float time = 0.0f;

		while (time < fadeTime)
		{
			float volume = Mathf.Lerp(startVolume, endVolume, time / fadeTime);
			audioSource.volume = volume;
			time += fadeDuration;
			yield return new WaitForSeconds(fadeDuration);
		}
		audioSource.Stop();
		audioSource.volume = startVolume;
	}
}
