
#if (UNITY_2019 || UNITY_2018|| UNITY_2017 || UNITY_5 || UNITY_4)
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Extensions.UnityEngine
{
    public static class AudioMixerExtensions
    {
        // Methods.
        // Range transformation methods.
        /// <summary>
        /// Gets the value of the specified exposed parameter, but converting the specified value from an attenuation
        /// value in decibels to the appropiate normalized [0, 1] range.
        /// </summary>
        public static void GetFloatAsLinear(this AudioMixer audioMixer, string name, out float value)
        {
            if (audioMixer.GetFloat(name, out value))
            {
                value = Mathf.Exp(value * 0.05f);
            }
        }
        
        /// <summary>
        /// Sets the value of the specified exposed parameter, but converting the specified value from a normalized
        /// [0, 1] range to the appropiate attenuation value in decibels.
        /// </summary>
        public static void SetFloatAsLinear(this AudioMixer audioMixer, string name, float value)
        {
            value = Mathf.Clamp(value, 0.002f, 1.0f);
            audioMixer.SetFloat(name, Mathf.Log(value) * 20.0f);
        }
    }
    
}
#endif