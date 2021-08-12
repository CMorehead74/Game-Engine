using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace EngineHelper.EHAudio
{
    public sealed class EHAudioManager
    {
        public EHSFX SFX;
        public EHMusic Music;

        //This is shared between the EHSFX and EHMusic classes
        private ContentManager content;

        public void Initialize(Game game)
        {
            //Create our own Content Manager
            content = new ContentManager(game.Content.ServiceProvider, game.Content.RootDirectory);

            //Point SFX and Music to use our "personal" content manager
            SFX = new EHSFX(content);
            Music = new EHMusic(content);
        }
    }

    public class EHSFX
    {
        ContentManager content;
        Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        //Set this to true if SFX should be played.
        public bool Enabled { get; set; }

        //Gets or sets the master volume for all sounds. 1.0f is max volume.
        public float MasterVolume
        {
            get { return SoundEffect.MasterVolume; }
            set { SoundEffect.MasterVolume = value; }
        }

        public EHSFX(ContentManager content)
        {
            this.content = content;
            Enabled = true;
        }

        //Loads a SoundEffect. Will just do nothing if sound already loaded.
        public void LoadSound(string soundName, string soundPath)
        {
            if (!sounds.ContainsKey(soundName.ToLower()))
            {
                //throw new InvalidOperationException(string.Format("Sound '{0}' has already been loaded", soundName));
                sounds.Add(soundName.ToLower(), content.Load<SoundEffect>(soundPath));
            }
        }

        //Plays the sound of the given name.
        public void PlaySound(string soundName)
        {
            PlaySound(soundName, 1.0f, 0.0f, 0.0f);
        }

        public void PlaySound(string soundName, float volume)
        {
            PlaySound(soundName, volume, 0.0f, 0.0f);
        }

        //Plays the sound of the given name at the given volume.
        public void PlaySound(string soundName, float volume, float pitch, float pan)
        {
            if (!Enabled)
                return;

            SoundEffect sound;

            if (!sounds.TryGetValue(soundName.ToLower(), out sound))
            {
                throw new ArgumentException(string.Format("Sound '{0}' not found", soundName));
            }

            sound.Play(volume, pitch, pan);
        }
    }

    public class EHMusic
    {
        private ContentManager content;

        private Dictionary<string, Song> songs = new Dictionary<string, Song>();

        private Song currentSong = null;

        private bool isMusicPaused = false;

        private bool isEnabled = true;

        //Set to true if Music should be played.
        public bool Enabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;

                if (isEnabled == false)
                    Pause();
                else
                    Resume();
            }
        }
        //Returns true if the game can play its music, false if the user is playing their own custom music.
        public bool GameHasControl { get { return MediaPlayer.GameHasControl; } }

        //Gets the name of the currently playing song, or null if no song is playing.
        public string CurrentSong { get; private set; }

        //Gets or sets the volume to play songs. 1.0f is max volume.
        public float MusicVolume
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = value; }
        }

        public EHMusic(ContentManager content)
        {
            this.content = content;

            if (GameHasControl == false)
                Enabled = false;
            else
                Enabled = true;
        }

        //Loads a Song.
        public void LoadSong(string songName)
        {
            LoadSong(songName, songName);
        }

        //Loads a Song. Will just do nothing if song already loaded.
        public void LoadSong(string songName, string songPath)
        {
            if (!songs.ContainsKey(songName))
            {
                songs.Add(songName, content.Load<Song>(songPath));
            }
        }

        //Starts playing the song with the given name. If it is already playing, this method does nothing. If another song is currently playing, it is stopped first.
        public void PlaySong(string songName)
        {
            PlaySong(songName, false);
        }

        //Starts playing the song with the given name. If it is already playing, this method does nothing. If another song is currently playing, it is stopped first.
        public void PlaySong(string songName, bool loop)
        {
            if (currentSong != null)
            {
                MediaPlayer.Stop();
            }

            if (!songs.TryGetValue(songName, out currentSong))
            {
                throw new ArgumentException(string.Format("Song '{0}' not found", songName));
            }

            CurrentSong = songName;

            isMusicPaused = false;
            MediaPlayer.IsRepeating = loop;
            MediaPlayer.Play(currentSong);

            if (!Enabled)
            {
                MediaPlayer.Pause();
            }
        }

        //Pauses the currently playing song.
        public void Pause()
        {
            if (currentSong != null && !isMusicPaused)
            {
                MediaPlayer.Pause();
                isMusicPaused = true;
            }
        }

        //Continues playing the song from the point where it was Paused.
        public void Resume()
        {
            if (currentSong != null && isMusicPaused)
            {
                if (Enabled)
                    MediaPlayer.Resume();
                isMusicPaused = false;
            }
        }

        //Stops the song and resets it back to the beginning.
        public void Stop()
        {
            if (currentSong != null && MediaPlayer.State != MediaState.Stopped)
            {
                MediaPlayer.Stop();
                isMusicPaused = false;
            }
        }
    }
}
