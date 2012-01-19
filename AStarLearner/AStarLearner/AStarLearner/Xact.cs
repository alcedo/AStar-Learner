using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace XnaHelpers.Audio
{
    public class XactSoundManager
    {
        #region Variable Declarations

        public string name;

        public AudioEngine engine;
        public string enginePath;

        public SoundBank soundBank;
        public string soundBankPath;

        public WaveBank waveBank;
        public string waveBankPath;

        #endregion

        #region Accessors

        public string Name
        { get { return name; } }

        public AudioEngine Engine
        { get { return engine; } }

        public string EnginePath
        { get { return enginePath; } }

        public SoundBank SoundBank
        { get { return soundBank; } }

        public string SoundBankPath
        { get { return soundBankPath; } }

        public WaveBank WaveBank
        { get { return waveBank; } }

        public string WaveBankPath
        { get { return waveBankPath; } }

        #endregion

        public XactSoundManager(string Name, string EnginePath, string SoundBankPath, string WaveBankPath)
        {
            this.name = Name;
            this.engine = new AudioEngine(EnginePath);
            this.soundBank = new SoundBank(engine, SoundBankPath);
            this.waveBank = new WaveBank(engine, WaveBankPath);
        }

        public void Unload()
        {
            this.engine.Dispose();
            this.soundBank.Dispose();
            this.waveBank.Dispose();
        }

        public void Update()
        {
            this.engine.Update();
        }

        public void Play(string SoundName)
        {
            this.soundBank.GetCue(SoundName).Play();
        }

        public void Pause(string SoundName)
        {
            this.soundBank.GetCue(SoundName).Pause();
        }

        public void Resume(string SoundName)
        {
            this.soundBank.GetCue(SoundName).Resume();
        }

        public void Stop(string SoundName, AudioStopOptions StopOptions)
        {
            this.soundBank.GetCue(SoundName).Stop(StopOptions);
        }

    }
  
    public class XactAudio
    {
        #region Variable Declarations

        public List<XactSoundManager> xactSoundManagers;

        #endregion

        #region Accessors

        public XactSoundManager this[string name]
        {
            get
            {
                for (int i = 0; i < xactSoundManagers.Count; i++)
                {
                    if (name.Equals(xactSoundManagers[i].name))
                        return xactSoundManagers[i];
                }
                return null;
            }
        }

        public bool IsEmpty
        { get { if (xactSoundManagers.Count > 0) return false; else return true; } }

        #endregion

        public XactAudio()
        {
            xactSoundManagers = new List<XactSoundManager>();
        }

        public void Add(XactSoundManager xactSoundManager)
        {
            xactSoundManagers.Add(xactSoundManager);
        }

        public void Remove(string name)
        {
            for (int i = 0; i < xactSoundManagers.Count; i++)
            {
                if (name.Equals(xactSoundManagers[i].name))
                {
                    xactSoundManagers.Remove(xactSoundManagers[i]);
                    return;
                }
            }
        }

        public void Remove(XactSoundManager xactSoundManager)
        {
            xactSoundManagers.Remove(xactSoundManager);
        }
    }
}
