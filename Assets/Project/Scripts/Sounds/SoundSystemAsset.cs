using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Sounds
{
    [CreateAssetMenu(fileName = nameof(SoundSystemAsset), menuName = "SoundSystem/" + nameof(SoundSystemAsset), order = 0)]
    public class SoundSystemAsset : ScriptableObject
    {
        [SerializeField]
        private List<AudioClipGroup> m_ClipGroups;

        [SerializeField]
        private List<SoundSystemEvent> m_SoundSystemEvents = new()
        {
            new SoundSystemEvent()
        };

        public List<AudioClipGroup> ClipGroups => m_ClipGroups;

        public List<SoundSystemEvent> SoundSystemEvents => m_SoundSystemEvents;
    }
}