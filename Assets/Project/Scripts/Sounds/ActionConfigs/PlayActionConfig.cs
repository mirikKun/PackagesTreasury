using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Sounds.Utils;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

namespace Project.Scripts.Sounds.ActionConfigs
{
     [Serializable]
    public class PlayActionConfig : ISoundSystemActionConfig
    {
        [SerializeField]
        [ClipNameDropdown]
        private string m_ClipName;

        [SerializeField]
        private bool m_Loop;

        [SerializeField]
        [Range(0f, 1f)]
        private float m_Volume = 1f;

        [SerializeField]
        private SoundSystemActionWaitType m_WaitType;

        public SoundSystemActionWaitType WaitType => m_WaitType;

        public bool IsCompleted { get; private set; }

   
        public async void Invoke(SoundSystemActionContext context)
        {
            IsCompleted = false;
            GameObject soundGameObject = new($"{m_ClipName}");
            soundGameObject.transform.parent = context.Parent;
            AudioSource source = soundGameObject.AddComponent<AudioSource>();
            AudioClip sourceClip = GetAudioClip(context.CurrentAsset) ?? GetAudioClip(FindSoundSystemAsset(context.LoadedAssets));

            if (sourceClip == null)
            {
                Object.Destroy(soundGameObject);
                IsCompleted = true;
                return;
            }

            context.AudioSources.Add(source);
            AudioMixerGroup mixerGroup = GetAudioMixer(context.CurrentAsset) ?? GetAudioMixer(FindSoundSystemAsset(context.LoadedAssets));
            soundGameObject.name += $" ({mixerGroup.name})";
            source.outputAudioMixerGroup = mixerGroup;
            source.playOnAwake = false;
            source.loop = m_Loop;
            source.volume = m_Volume;
            source.clip = sourceClip;

            source.Play();

            if (m_Loop)
                return;

            //await Awaiters.Seconds(sourceClip.length);
            IsCompleted = true;

            if (soundGameObject == null)
                return;

            context.AudioSources.Remove(source);
            Object.Destroy(soundGameObject);
        }

        private SoundSystemAsset FindSoundSystemAsset(List<SoundSystemAsset> loadedAssets)
        {
            return (from systemAsset in loadedAssets
                let mixerGroup = GetAudioMixer(systemAsset)
                where mixerGroup != null
                select systemAsset).FirstOrDefault();
        }

        private AudioClip GetAudioClip(SoundSystemAsset asset)
        {
            return asset.ClipGroups.Select(assetClipGroup => assetClipGroup.Clips.FirstOrDefault(c => c.name == m_ClipName))
                .FirstOrDefault(clip => clip != null);
        }

        private AudioMixerGroup GetAudioMixer(SoundSystemAsset asset)
        {
            return asset.ClipGroups.FirstOrDefault(audioClipGroup => audioClipGroup.Clips.Any(c => c.name == m_ClipName))?.Group;
        }
    }
}