using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceoverManager : Singleton<VoiceoverManager>
{
    [System.Serializable]
    public class VoiceoverGroup
    {
        public string key;
        public AudioClip[] clips;
        public float cooldown = 1f;
        [HideInInspector] public float lastPlayTime = -999f;
    }

    [Header("Setup")]
    [SerializeField] private AudioSource m_voiceoverSource;
    [SerializeField] private List<VoiceoverGroup> m_voiceGroups = new();

    private Dictionary<string, VoiceoverGroup> voiceDict = new();

    protected override void Awake()
    {
        base.Awake();

        foreach (VoiceoverGroup group in m_voiceGroups)
        {
            if (!voiceDict.ContainsKey(group.key))
                voiceDict.Add(group.key, group);
        }
    }

    public void Play(string key)
    {
        if (!voiceDict.TryGetValue(key, out VoiceoverGroup group)) return;

        // Cooldown check
        if (Time.unscaledTime - group.lastPlayTime < group.cooldown) return;

        if (group.clips.Length == 0) return;

        m_voiceoverSource.pitch = 1;
        AudioClip clip = group.clips[Random.Range(0, group.clips.Length)];
        m_voiceoverSource.PlayOneShot(clip);
        group.lastPlayTime = Time.unscaledTime;
    }

    public void Play(string key, Vector2 randomizeMinMax)
    {
        if (!voiceDict.TryGetValue(key, out VoiceoverGroup group)) return;

        if (Time.unscaledTime - group.lastPlayTime < group.cooldown) return;

        if (group.clips.Length == 0) return;

        m_voiceoverSource.pitch = Random.Range(randomizeMinMax.x, randomizeMinMax.y);

        AudioClip clip = group.clips[Random.Range(0, group.clips.Length)];
        m_voiceoverSource.PlayOneShot(clip);
        group.lastPlayTime = Time.unscaledTime;
    }
}
