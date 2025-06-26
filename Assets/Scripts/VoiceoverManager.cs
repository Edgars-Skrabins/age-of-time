using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceoverManager : Singleton<VoiceoverManager>
{
    [System.Serializable]
    public class VoiceoverGroup
    {
        public string key; // e.g., "EnemyHit", "BuyUpgrade"
        public AudioClip[] clips;
        public float cooldown = 1f;
        [HideInInspector] public float lastPlayTime = -999f;
    }

    [Header("Setup")]
    [SerializeField] private AudioSource voiceoverSource;
    [SerializeField] private List<VoiceoverGroup> voiceGroups = new();

    private Dictionary<string, VoiceoverGroup> voiceDict = new();

    protected override void Awake()
    {
        base.Awake();

        foreach (VoiceoverGroup group in voiceGroups)
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

        AudioClip clip = group.clips[Random.Range(0, group.clips.Length)];
        voiceoverSource.PlayOneShot(clip);
        group.lastPlayTime = Time.unscaledTime;
    }
}
