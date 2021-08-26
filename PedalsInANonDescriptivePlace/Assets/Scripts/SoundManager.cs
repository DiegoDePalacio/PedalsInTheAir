using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public enum SFX
    {
        None,
        BigPoopHit,
        LittlePoopHit,
        Crash,
        NoAvailablePoop,
        Eating,
        Healing,
        Pooping,
        HagenEee,
        HagenDisgusting,
        HagenYuk,
        HansEaw,
        HansEw,
        IngridScream,
        IngridScream2,
        MagnusEww1,
        MagnusEww2,
        MattDisgusting,
        MattGroan,
        StianAsshole,
        StianAww,
        StianFlyby
    }

    [Serializable]
    public class Sound
    {
        public SFX Sfx = SFX.None;
        public AudioClip clip = null;
    }
    
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        public List<Sound> _sounds = new List<Sound>();

        private AudioSource _audioSource;

        public void Awake()
        {
            Instance = this;
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(SFX sfx)
        {
            Sound sound = _sounds.FirstOrDefault(s => s.Sfx == sfx);
            _audioSource.PlayOneShot(sound.clip);
        }
    }
}