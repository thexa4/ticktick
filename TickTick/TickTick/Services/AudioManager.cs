using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace TickTick.Services
{
    /// <summary>
    /// This class mangages all the audio in the game. It is added as a service to the game services
    /// container so you can retrieve it using Game.Services.GetService(typeof(AudioManager))
    /// </summary>
    public class AudioManager : GameComponent
    {
        protected Dictionary<String, SoundEffectInstance> _soundInstances;
        protected ContentManager _contentManager;

        /// <summary>
        /// Creates a new Audio manager
        /// </summary>
        /// <param name="game">Game to bind to</param>
        public AudioManager(Game game)
            : base(game)
        {
            // Add this as a service
            this.Game.Services.AddService(typeof(AudioManager), this);

            // Special content Manager just for sounds. Default directory is 
            // "Content/Audio". So if you want to load sounds, you don't need
            // to refer it as "Audio/asset" but you can just use "asset".
            _contentManager = new ContentManager(game.Services);
            _contentManager.RootDirectory = "Content/Audio";
        }

        /// <summary>
        /// Initializes Manager
        /// </summary>
        public override void Initialize()
        {
            _soundInstances = new Dictionary<String, SoundEffectInstance>();

            base.Initialize();

            // We don't need to update this component
            this.Enabled = false;
        }

        /// <summary>
        /// Loads and Caches a sound and creates an instance
        /// </summary>
        /// <param name="soundAsset">Asset path</param>
        /// <param name="instanceName">Instance name</param>
        /// <param name="volume">Volume</param>
        /// <param name="pitch">Pitch</param>
        /// <returns></returns>
        public SoundEffectInstance Load(String soundAsset, String instanceName, 
            Single volume = 1f, Single pitch = 0f)
        {
            SoundEffectInstance instance;

            // If the sound was never loaded, just reload it. If we want to play two sounds
            // simultainously, we need to load it twice. Lucky thing that the contentmanager caches
            // all its loads. So if you want to play a bullet sound twice SIMULTAINOUZZZLY:
            //
            //      Load("BulletSound", "BulletOne"); 
            //      Load("BulletSound", "BulletTwo");
            //
            // The object retrieved is an instance, so no disposed errors. XNA sound effects are not
            // accessible for a fraction of a second when they are played. That's why we use sound 
            // instances, which are always accessible.
            if (!_soundInstances.TryGetValue(instanceName, out instance))
            {
                instance = _contentManager.Load<SoundEffect>(soundAsset).CreateInstance();
                instance.Pitch = pitch;
                instance.Volume = volume;
                _soundInstances[instanceName] = instance;
            }

            return instance;
        }

        /// <summary>
        /// Plays a sound
        /// </summary>
        /// <param name="instanceName">Instance Name</param>
        public void Play(String instanceName)
        {
            SoundEffectInstance instance;
            if (_soundInstances.TryGetValue(instanceName, out instance))
                instance.Play();
        }

        /// <summary>
        /// Plays a sound with a new volume
        /// </summary>
        /// <param name="instanceName">Instance Name</param>
        /// <param name="volume">Volume</param>
        public void Play(String instanceName, Single volume)
        {
            SoundEffectInstance instance;
            if (_soundInstances.TryGetValue(instanceName, out instance))
            {
                instance.Volume = volume;
                instance.Play();
            }
        }

        /// <summary>
        /// Plays a sound a new volume and pitch
        /// </summary>
        /// <param name="instanceName">Instance Name</param>
        /// <param name="volume">Volume</param>
        /// <param name="pitch">Pitch</param>
        public void Play(String instanceName, Single volume, Single pitch)
        {
            SoundEffectInstance instance;
            if (_soundInstances.TryGetValue(instanceName, out instance))
            {
                instance.Volume = volume;
                instance.Pitch = pitch;
                instance.Play();
            }
        }

        /// <summary>
        /// Stops playing a sound
        /// </summary>
        /// <param name="instanceName">Instance name</param>
        public void Stop(String instanceName)
        {
            SoundEffectInstance instance;
            if (_soundInstances.TryGetValue(instanceName, out instance))
                instance.Stop();
        }

        /// <summary>
        /// Unloads all resources
        /// </summary>
        public void Unload()
        {
            _contentManager.Unload();
            foreach (var sound in _soundInstances)
                if (!sound.Value.IsDisposed)
                    sound.Value.Dispose();
            _soundInstances.Clear();
        }

        /// <summary>
        /// Loops a sound
        /// </summary>
        /// <param name="instanceName">InstanceName</param>
        public void Loop(String instanceName)
        {
            SoundEffectInstance instance;
            if (_soundInstances.TryGetValue(instanceName, out instance))
                instance.IsLooped = true;

            Play(instanceName);
        }
    }
}
