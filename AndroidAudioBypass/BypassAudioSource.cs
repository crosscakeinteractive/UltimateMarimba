/*
 * Copyright 2020 CrossCakeInteractive. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace AndroidAudioBypass {

    /// <summary>
    /// AudioSource equivalent for the Android bypass mechanism.
    /// </summary>
    public class BypassAudioSource : MonoBehaviour {
        public GameObject Dampener;
        M_Manager master;

        [Serializable]
        public class DampenerThreshold
        {
            [Range(0.0f, 1.0f)]
            public float DampenerThreshValue;
            [Range(0.0f, 1.0f)]
            public float AttackThreshValue;
        }
        public DampenerThreshold MarimbaDampValue;
        public DampenerThreshold VibraphoneDampValue;

        public BypassAudioManager m_bypassAudioManager;
        public Record record;
        public Slider vol;
        public string [] m_audioFile;
        public int instrument_mode;
        [Range(0.0f, 1.0f)]
        public int m_priority = 1;          // stream priority (0 = lowest)

        [Range(0.0f, 1.0f)]
        public float m_volume = 1.0f;       // volume [0.0, 1.0]

        [Range(0.5f, 2.0f)]
        public float m_rate = 1.0f;         // playback rate (1.0 = normal; [0.5, 2.0])

        public bool m_playOnAwake = false;
        public bool m_loop = false;
        public int [] m_soundId;
        public int Play_ID;

        public bool Dampen = false;
        float step_dampfade = 0.30f;
        float threshold;
        float AttackThreshold;
        
        public void Start() {

            master = gameObject.GetComponent<M_Manager>();
            if(PlayerPrefs.HasKey("INSTRUMENT"))
            {
                Load(PlayerPrefs.GetString("INSTRUMENT"));
            }
            else { Load("marimba"); }

            if(PlayerPrefs.HasKey("dampener"))
            {
                if(PlayerPrefs.GetInt("dampener") == 0)
                {
                    EnableDampButton(true);
                }
                else if(PlayerPrefs.GetInt("dampener") == 1)
                {
                    EnableDampButton(false);
                    
                }
            }
                        
            if (m_playOnAwake)
            {
                Play(0);
            }
            vol.value = m_volume;
        }
        public void volume_slider()
        {
            m_volume = vol.value;
        }
        public void VolumeDampener(bool activate)
        {
           
            if(activate)
            {
                m_bypassAudioManager.ChangeVolumeRealtime(threshold*m_volume);
                Dampen = true;           
            }
            else if(!activate)
            {
                Dampen = false;
                m_bypassAudioManager.ChangeVolumeRealtime(m_volume);
                Debug.Log("damper has released");
            }
            if (record.Recording)
            {
               // record.RecDamp();
            }
        }
        IEnumerator CaptureExp()
        {
            yield return new WaitForSeconds(AttackThreshold);
            m_bypassAudioManager.ChangeVolumeRealtime(threshold*m_volume);
            StopCoroutine(CaptureExp());
        }
        public void EnableDampButton(bool param)
        {
            Dampener.SetActive(param);
            if(param)
            {
                PlayerPrefs.SetInt("dampener", 0);
            }
            else if(!param)
            {
                PlayerPrefs.SetInt("dampener", 1);
            }
        }

        public void Load(string InstrumentFolder)
        {
            for (int i = 0; i < m_audioFile.Length; i++)
            {
                if (String.IsNullOrEmpty(m_audioFile[i]))
                {
                    Debug.LogError("Audio file not specified in or not found " + gameObject.name);
                    return;
                }

                m_soundId[i] = m_bypassAudioManager.RegisterSoundFile(InstrumentFolder + "/" + m_audioFile[i]);
                Debug.Log(m_audioFile[i]);
                if(InstrumentFolder == "marimba")
                {
                    instrument_mode = 0;
                    threshold = MarimbaDampValue.DampenerThreshValue;
                    AttackThreshold = MarimbaDampValue.AttackThreshValue;
                    
                }
                else if(InstrumentFolder == "vibraphone")
                {
                    instrument_mode = 1;
                    threshold = VibraphoneDampValue.DampenerThreshValue;
                    AttackThreshold = VibraphoneDampValue.AttackThreshValue;
                  

                }
                else if(InstrumentFolder == "xylophone")
                {
                    instrument_mode = 2;
                    threshold = MarimbaDampValue.DampenerThreshValue;
                    AttackThreshold = MarimbaDampValue.AttackThreshValue;

                }
                record.Rec_InstChanger(instrument_mode);
                PlayerPrefs.SetString("INSTRUMENT", InstrumentFolder);
            }
        }
        public void UnloadResources()
        {
            for (int i = 0; i < m_audioFile.Length; i++)
            {
                m_bypassAudioManager.UnregisterSound(m_soundId[i]);              
            }
           
        }
       
        public void Play(int n)
        {         
            m_bypassAudioManager.PlaySound(m_soundId[n], m_volume, m_volume, m_priority,
                                           m_loop ? -1 : 0, m_rate);
            if (Dampen)
            {
                StartCoroutine(CaptureExp());
            }

        }
    }

} // namespace AndroidAudioBypass
