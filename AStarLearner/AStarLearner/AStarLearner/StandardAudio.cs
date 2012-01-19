using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

//Reference: http://msdn.microsoft.com/en-us/library/bb195038.aspxs

namespace XnaHelpers.Audio
{
    public class StandardSoundManager
    {
        #region Variable Declarations

        public string name;

        public List<Sound> soundEffects;

        #endregion

        #region Accessors

        public string Name
        { get { return name; } }

        public List<Sound> SoundEffects
        {
            get { return soundEffects; }
            set { soundEffects = value; }
        }

        public Sound this[string name]
        {
            get
            {
                foreach (Sound sfx in soundEffects)
                {
                    if (name.Equals(sfx.name))
                        return sfx;
                }
                return null;
            }
        }

        #endregion

        public StandardSoundManager(string Name)
        {
            this.name = Name;
            this.soundEffects = new List<Sound>();
        }

        public void Add(Sound Sound)
        {
            soundEffects.Add(Sound);
        }

        public void Remove(string soundName)
        {
            for (int i = 0; i < soundEffects.Count; i++)
            {
                if (soundName.Equals(soundEffects[i]))
                {
                    soundEffects.Remove(soundEffects[i]);
                    return;
                }
            }
        }

    }


    public class StandardAudio
    {
        public List<StandardSoundManager> soundEffectManagers;

        public StandardSoundManager this[string name]
        {
            get
            {
                for (int i = 0; i < soundEffectManagers.Count; i++)
                {
                    if (name.Equals(soundEffectManagers[i].name))
                        return soundEffectManagers[i];
                }
                return null;
            }
        }

        public StandardAudio()
        {
            soundEffectManagers = new List<StandardSoundManager>();
        }

        public bool IsEmpty()
        {
            if (soundEffectManagers.Count == 0)
                return true;
            else
                return false;
        }

        public void Add(StandardSoundManager soundManagers)
        {
            soundEffectManagers.Add(soundManagers);
        }

        public void Remove(string name)
        {
            for (int i = 0; i < soundEffectManagers.Count; i++)
            {
                if (name.Equals(soundEffectManagers[i].name))
                {
                    soundEffectManagers.Remove(soundEffectManagers[i]);
                    return;
                }
            }
        }

        public void Remove(StandardSoundManager soundEffectManager)
        {
            soundEffectManagers.Remove(soundEffectManager);
        }



    }

    
    public class Sound
    {
        #region Variable Declarations

        public string name;

        public SoundEffect sound;

        public SoundEffectInstance instance;

        #endregion

        #region Accessors

        public SoundState State
        { get { return instance.State; } }

        public float Volume
        { get { return instance.Volume; } }

        public float Pitch
        { get { return instance.Pitch; } set { instance.Pitch = value; } }

        public float Pan
        { get { return instance.Pan; } set { instance.Pan = value; } }

        #endregion

        public Sound(string Name, SoundEffect Sound)
        {
            this.name = Name;
            this.sound = Sound;
            this.instance = Sound.CreateInstance();
        }

        public void Play()
        {
            instance.Play();
        }

        public void Pause()
        {
            instance.Pause();
        }

        public void Resume()
        {
            instance.Resume();
        }

        public void Stop()
        {
            instance.Stop();
        }

        public void Apply3D(AudioListener listener, AudioEmitter emitter)
        {
            instance.Apply3D(listener, emitter);
        }

    }

}
