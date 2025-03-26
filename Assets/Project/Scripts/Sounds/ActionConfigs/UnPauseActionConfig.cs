using System;
using System.Linq;
using UnityEngine;

namespace Project.Scripts.Sounds.ActionConfigs
{
    [Serializable]
    public class UnPauseActionConfig : ISoundSystemActionConfig
    {
        [SerializeField]
        [ClipNameDropdown]
        private string m_ClipName;

        [SerializeField]
        private SoundSystemActionWaitType m_WaitType;

        public SoundSystemActionWaitType WaitType => m_WaitType;
        public bool IsCompleted { get; private set; }


        public void Invoke(SoundSystemActionContext context)
        {
            IsCompleted = false;

            foreach (AudioSource audioSource in context.AudioSources.Where(audioSource => audioSource.clip.name == m_ClipName))
            {
                audioSource.UnPause();
            }

            IsCompleted = true;
        }
    }
}