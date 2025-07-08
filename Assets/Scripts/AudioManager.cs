using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class AudioData
{
    public string name;
    public AudioClip audioClip;
    [Range(0, 1)] public float volume = 0.8f;
    public bool isMusic;
    public bool randomizePitch;
    public Vector2 randomizePitchValues = new(0.8f, 1.3f);
    public bool playOnAwake;
    public bool loop;
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("Mixer Settings")]
    [SerializeField] private AudioMixer m_mixer;
    [SerializeField] private AudioMixerGroup m_musicChannel, m_sfxChannel, m_voiceChannel;

    [SerializeField] private string m_musicVolumeParameter = "MusicVolume";
    [SerializeField] private string m_sfxVolumeParameter = "SFXVolume";
    [SerializeField] private string m_voiceVolumeParameter = "VoiceVolume";

    [Header("UI Settings")]
    [SerializeField] private Slider m_musicVolumeSlider;
    [SerializeField] private Slider m_sfxVolumeSlider;
    [SerializeField] private Slider m_voiceVolumeSlider;
    [SerializeField] private Toggle m_jazzMode;

    private float m_musicVolume;
    private float m_sfxVolume;
    private float m_voiceVolume;

    [Header("Audio SFX Settings")]
    [SerializeField] private List<AudioData> m_audioDataList;

    private static List<GameObject> m_audioSourceObjects;

    protected override void Awake()
    {
        base.Awake();
        InitializeAudioSources();
        InitializeUI();
    }
    private void Start()
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        m_mixer.SetFloat(m_musicVolumeParameter, Mathf.Log10(GetMusicVolume()) * 20);
        m_mixer.SetFloat(m_sfxVolumeParameter, Mathf.Log10(GetSFXVolume()) * 20);
        m_mixer.SetFloat(m_voiceVolumeParameter, Mathf.Log10(GetVoiceVolume()) * 20);
    }

    private void InitializeAudioSources()
    {
        if (m_audioDataList.Count > 0)
        {
            m_audioSourceObjects = new List<GameObject>();

            foreach (AudioData sfx in m_audioDataList)
            {
                GameObject sfxObject = new GameObject(sfx.name, typeof(AudioSource));
                sfxObject.transform.SetParent(transform);

                AudioSource newAudioSource = sfxObject.GetComponent<AudioSource>();
                newAudioSource.volume = sfx.volume;
                newAudioSource.clip = sfx.audioClip;
                if (sfx.playOnAwake)
                {
                    newAudioSource.playOnAwake = sfx.playOnAwake;
                    newAudioSource.Play();
                }
                newAudioSource.loop = sfx.loop;
                newAudioSource.outputAudioMixerGroup = sfx.isMusic ? m_musicChannel : m_sfxChannel;
                m_audioSourceObjects.Add(sfxObject);
            }
        }
    }

    private void InitializeUI()
    {
        m_musicVolumeSlider.minValue = .00001f;
        m_sfxVolumeSlider.minValue = .00001f;
        m_voiceVolumeSlider.minValue = .00001f;

        m_musicVolumeSlider.value = GetMusicVolume();
        m_sfxVolumeSlider.value = GetSFXVolume();
        m_voiceVolumeSlider.value = GetVoiceVolume();

        m_musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        m_sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        m_voiceVolumeSlider.onValueChanged.AddListener(OnVoiceVolumeChanged);
    }

    public AudioSource GetAudioSource(string _name)
    {
        foreach (GameObject obj in m_audioSourceObjects)
        {
            if (obj.name == _name) return obj.GetComponent<AudioSource>();
        }
        return null;
    }

    private AudioData GetAudioSFXByName(string _name)
    {
        foreach (AudioData sfx in m_audioDataList)
        {
            if (sfx.name == _name) return sfx;
        }
        return null;
    }

    private void OnMusicVolumeChanged(float value)
    {
        SetMusicVolume(value);
        ApplySettings();
    }

    private void OnSFXVolumeChanged(float value)
    {
        SetSFXVolume(value);
        ApplySettings();
    }

    private void OnVoiceVolumeChanged(float value)
    {
        SetVoiceVolume(value);
        ApplySettings();
    }


    private void SetMusicVolume(float value)
    {
        m_musicVolume = Mathf.Log10(value) * 20;
        m_mixer.SetFloat(m_musicVolumeParameter, m_musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    private void SetSFXVolume(float value)
    {
        m_sfxVolume = Mathf.Log10(value) * 20;
        m_mixer.SetFloat(m_sfxVolumeParameter, m_sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFXVolume", 0.5f);
    }

    private void SetVoiceVolume(float value)
    {
        m_voiceVolume = Mathf.Log10(value) * 20;
        m_mixer.SetFloat(m_voiceVolumeParameter, m_voiceVolume);
        PlayerPrefs.SetFloat("VoiceVolume", value);
    }

    private float GetVoiceVolume()
    {
        return PlayerPrefs.GetFloat("VoiceVolume", 0.5f);
    }


    public void PlaySound(string _name)
    {
        AudioData sfxStats = GetAudioSFXByName(_name);
        AudioSource source = GetAudioSource(_name);
        if (source)
        {
            if (sfxStats.randomizePitch)
            {
                source.pitch = Random.Range(sfxStats.randomizePitchValues.x, sfxStats.randomizePitchValues.y);
            }
            source.PlayOneShot(sfxStats.audioClip);
        }
    }

    public void PlayMusic(string _name)
    {
        AudioSource source = GetAudioSource(_name);
        if (source)
        {
            source.Play();
        }
    }

    public void StopSound(string _name)
    {
        AudioSource source = GetAudioSource(_name);

        if (source && source.isPlaying)
        {
            source.Stop();
        }
    }

    public bool JazzMode()
    {
        return m_jazzMode.isOn;
    }

    public void ToggleJazzMode()
    {
        StopSound("BGM_MainMenu");
        StopSound("BGM_JazzMainMenu");
        StopSound("BGM_GameMusic0");
        StopSound("BGM_GameMusic1");
        StopSound("BGM_JazzGameMusic");

        if (GameManager.I.M_CurrentState == GameState.MainMenu)
        {
            if (!JazzMode()) PlayMusic("BGM_MainMenu");
            else
            {
                PlayMusic("BGM_JazzMainMenu");
            }
        }
        else if (GameManager.I.M_CurrentState == GameState.Playing || GameManager.I.M_CurrentState == GameState.Paused)
        {
            if (!JazzMode()) PlayMusic("BGM_GameMusic" + Random.Range(0, 2));
            else
            {
                PlayMusic("BGM_JazzGameMusic");
            }
        }
        else if (GameManager.I.M_CurrentState == GameState.GameOver)
        {
            //if (!JazzMode()) PlayMusic("BGM_GameOverMusic");
            //else { PlayMusic("BGM_JazzGameOverMusic"); }
        }
    }
}