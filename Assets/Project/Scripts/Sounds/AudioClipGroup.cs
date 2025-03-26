using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Project.Scripts.Sounds
{
    public class AudioClipGroup
    {
        [SerializeField]
        private List<AudioClip> m_Clips;

        [SerializeField]
        private AudioMixerGroup m_Group;

        public List<AudioClip> Clips => m_Clips;

        public AudioMixerGroup Group => m_Group;
    }
}