﻿using System;
using System.Linq;
using UnityEngine;

namespace Project.Scripts.Sounds.ActionConfigs
{
    [Serializable]
    public class StopActionConfig : ISoundSystemActionConfig
    {
        [SerializeField]
        [ClipNameDropdown]
        private string _clipName;

        [SerializeField]
        private SoundSystemActionWaitType _waitType;

        public SoundSystemActionWaitType WaitType => _waitType;
        public bool IsCompleted { get; private set; }

  
        public void Invoke(SoundSystemActionContext context)
        {
            IsCompleted = false;

            foreach (AudioSource audioSource in context.AudioSources.Where(audioSource => audioSource.clip.name == _clipName).ToList())
            {
                audioSource.Stop();
                context.AudioSources.Remove(audioSource);
            }

            IsCompleted = true;
        }
    }
}