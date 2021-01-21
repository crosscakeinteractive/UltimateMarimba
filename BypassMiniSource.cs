using UnityEngine;
using System;
using System.Collections;

namespace AndroidAudioBypass
{

    /// <summary>
    /// AudioSource equivalent for the Android bypass mechanism.
    /// </summary>
    public class BypassMiniSource : MonoBehaviour
    {
        public BypassAudioManager m_bypassAudioManager;
        public string m_audioFile;

        [Range(0.0f, 1.0f)]
        public int m_priority = 1;          // stream priority (0 = lowest)

        [Range(0.0f, 1.0f)]
        public float m_volume = 1.0f;       // volume [0.0, 1.0]

        [Range(0.5f, 2.0f)]
        public float m_rate = 1.0f;         // playback rate (1.0 = normal; [0.5, 2.0])

        public bool m_playOnAwake = false;
        public bool m_loop = false;
        private int loop;
        private int m_soundId;

        public void Start()
        {
            if (String.IsNullOrEmpty(m_audioFile))
            {
                Debug.LogError("Audio file not specified in " + gameObject.name);
                return;
            }

            m_soundId = m_bypassAudioManager.RegisterSoundFile(m_audioFile);

            if (m_playOnAwake)
            {
                Play();
            }
            if(m_loop)
            {
                loop = -1;
            }
            else
            {
                loop = 0;
            }

        }

        public void Play()
        {
            if (m_rate < 0.5f)
            {
                m_rate = 0.5f;
            }
            if (m_rate > 2.0f)
            {
                m_rate = 2.0f;
            }


            m_bypassAudioManager.PlaySound(m_soundId, m_volume, m_volume, m_priority, loop, m_rate);
        }
    }

} // namespace AndroidAudioBypass

