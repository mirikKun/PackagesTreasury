using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Sounds
{
    [Serializable]
    public class SoundSystemEvent
    {
        [SerializeField]
        private string m_EventName;

        [SerializeReference]
        [ActionList]
        private List<ISoundSystemActionConfig> m_Actions = new();

        public string EventName => m_EventName;

        public List<ISoundSystemActionConfig> Actions => m_Actions;
    }
}