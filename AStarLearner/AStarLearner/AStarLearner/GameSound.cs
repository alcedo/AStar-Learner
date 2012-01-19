using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Threading;

namespace XnaHelpers.GameEngine
{
    /// <summary>
    /// Represents a sound effect such as a WAV
    /// </summary>
    public class GameSFX
    {
        #region Variable Declarations
        //The base sound info
        SoundEffect baseEffect;
        SoundEffectInstance baseInstance;

        /// <summary>
        /// The volume of the sound, from 0 to 100
        /// </summary>
        public float Volume
        {
            //Gets the volume as a percentage
            get
            {
                return baseInstance.Volume * 100;
            }
            //Sets the volume, making sure it doesn't exceed 100
            set
            {
                baseInstance.Volume = MathHelper.Clamp(value / 100, 0, 1);
            }
        }

        /// <summary>
        /// The pitch of the sound, from -1 to 1
        /// </summary>
        public float Pitch
        {
            //Gets the pitch
            get
            {
                return baseInstance.Pitch;
            }
            //Sets the pitch with the correct bounds
            set
            {
                baseInstance.Pitch = MathHelper.Clamp(value, -1, 1);
            }
        }

        /// <summary>
        /// The direction to pan the sound in, from -1 to 1 with -1 being left and 1 being right
        /// </summary>
        public float Pan
        {
            //Gets the pan
            get
            {
                return baseInstance.Pan;
            }
            //Sets the pan with the correct bounds
            set
            {
                baseInstance.Pan = MathHelper.Clamp(value, -1, 1);
            }
        }

        /// <summary>
        /// Whether the sound effect should loop
        /// </summary>
        public bool Looping
        {
            //Binds this to the baseInstance.IsLooped property
            get
            {
                return baseInstance.IsLooped;
            }
            set
            {
                baseInstance.IsLooped = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new sound effect
        /// </summary>
        /// <param name="soundEffect">The base SoundEffect to use</param>
        public GameSFX(SoundEffect soundEffect)
        {
            //Sets the base sounds
            baseEffect = soundEffect;
            baseInstance = baseEffect.CreateInstance();
        }
        #endregion

        #region Playback Methods
        /// <summary>
        /// Plays the sound
        /// </summary>
        public void Play()
        {
            baseInstance.Play();
        }

        /// <summary>
        /// Plays the sound, but allow multiple calls of the same sound for over lapping
        /// </summary>
        public void MultiPlay()
        {
            baseEffect.Play();
        }
        

        public void PlayLooping()
        {
            //Sets Looping to true and plays the sound
            Looping = true;
            Play();
        }

        /// <summary>
        /// Pauses the sound
        /// </summary>
        public void Pause()
        {
            baseInstance.Pause();
        }

        /// <summary>
        /// Stops the sound
        /// </summary>
        public void Stop()
        {
            baseInstance.Stop();
        }

        /// <summary>
        /// Resumes the paused sound
        /// </summary>
        public void Resume()
        {
            baseInstance.Resume();
        }
        #endregion
    }

    /// <summary>
    /// Represents music such as an MP3 file
    /// </summary>
    public class GameMusic
    {
        #region Variable Declarations
        //The base music
        Song music;

        /// <summary>
        /// Whether the music is looping
        /// </summary>
        public bool Looping
        {
            //Binds this to MediaPlay.IsRepeating
            get
            {
                return MediaPlayer.IsRepeating;
            }
            set
            {
                MediaPlayer.IsRepeating = value;
            }
        }

        /// <summary>
        /// The volume of the music, from 0 to 100
        /// </summary>
        public float Volume
        {
            //Gets the volume as a percentage
            get
            {
                return MediaPlayer.Volume * 100;
            }
            //Sets the volume with the correct bounds
            set
            {
                MediaPlayer.Volume = MathHelper.Clamp(value / 100, 0, 100);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new GameMusic
        /// </summary>
        /// <param name="song"></param>
        public GameMusic(Song song)
        {
            //Sets the base music
            music = song;
        }
        #endregion

        #region Playback Methods
        /// <summary>
        /// Plays the music
        /// </summary>
        public void Play()
        {
            //Thanks to slade24 on the XNA forums for this one
            //Plays the music on a new thread to reduce startup delay
            ThreadPool.QueueUserWorkItem(delegate(object data)
            {
                MediaPlayer.Play(music);
            });
        }

        public void PlayLooping()
        {
            //Sets looping to true and plays the song
            Looping = true;
            Play();
        }

        /// <summary>
        /// Pauses the music
        /// </summary>
        public void Pause()
        {
            MediaPlayer.Pause();
        }

        /// <summary>
        /// Stops the music
        /// </summary>
        public void Stop()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// Resumes the paused music
        /// </summary>
        public void Resume()
        {
            MediaPlayer.Resume();
        }
        #endregion
    }

    /// <summary>
    /// The base which allows sounds to be played from an XACT file
    /// </summary>
    public class XACTBase
    {
        #region Variable Declarations
        /// <summary>
        /// The base audio engine
        /// </summary>
        public AudioEngine audioEngine;
        /// <summary>
        /// The wave bank in the XACT file
        /// </summary>
        public WaveBank waveBank;
        /// <summary>
        /// The sound bank in the XACT file
        /// </summary>
        public SoundBank soundBank;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the base for playing sounds from an XACT file
        /// </summary>
        /// <param name="audioEnginePath">The audio engine path, usually xactproject.xgs</param>
        /// <param name="waveBankPath">The wave bank for the XACT project, usually Wave Bank.xwb</param>
        /// <param name="soundBankPath">The sound bank for the XACT project, usually Sound Bank.xsb</param>
        public XACTBase(string audioEnginePath, string waveBankPath, string soundBankPath)
        {
            //Sets the base components required to play a sound with XACT
            audioEngine = new AudioEngine(audioEnginePath);
            waveBank = new WaveBank(audioEngine, waveBankPath);
            soundBank = new SoundBank(audioEngine, soundBankPath);
        }
        #endregion
    }

    /// <summary>
    /// A sound from an XACT file
    /// </summary>
    public class XACTSound
    {
        #region Variable Declarations
        //The XACT cue
        Cue sound;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new XACTSound
        /// </summary>
        /// <param name="xactBase">The XACTBase to get the sound from</param>
        /// <param name="soundName">The name of the sound to use</param>
        public XACTSound(XACTBase xactBase, string soundName)
        {
            //Gets the cue from the XACT file
            sound = xactBase.soundBank.GetCue(soundName);
        }
        #endregion

        #region Playback Methods
        /// <summary>
        /// Plays the sound
        /// </summary>
        public void Play()
        {
            sound.Play();
        }

        /// <summary>
        /// Pauses the sound
        /// </summary>
        public void Pause()
        {
            sound.Pause();
        }

        /// <summary>
        /// Stops the sound
        /// </summary>
        public void Stop()
        {
            sound.Stop(AudioStopOptions.AsAuthored);
        }
        #endregion
    }
}
