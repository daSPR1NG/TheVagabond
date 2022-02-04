using UnityEngine;

namespace Khynan_Coding
{
    public static class AudioHelper
    {
        #region States - isPlaying / canPlay
        public static bool CanThisAudioSourcePlaySound(AudioSource audioSource)
        {
            if (audioSource.isActiveAndEnabled && audioSource.clip != null && audioSource.clip.loadState == AudioDataLoadState.Loaded)
            {
                return true;
            }

            return false;
        }

        public static bool IsThisAudioSourcePlaying(AudioSource audioSource)
        {
            if (audioSource.isPlaying)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region AudioSource - Play
        public static void AudioSourcePlay(AudioSource audioSource, AudioClip audioClip = null)
        {
            if (audioClip) { audioSource.clip = audioClip; }
            
            if (CanThisAudioSourcePlaySound(audioSource))
            {
                audioSource.Play();
            }
        }

        public static void AudioSourcePlayWithDelay(AudioSource audioSource, float delay)
        {
            if (CanThisAudioSourcePlaySound(audioSource))
            {
                audioSource.PlayDelayed(delay);
            }
        }
        #endregion

        #region AudioSource - Stop / Un.Pause / Mute
        public static void AudioSourceStop(AudioSource audioSource)
        {
            if (IsThisAudioSourcePlaying(audioSource))
            {
                audioSource.Stop();
            }
        }

        public static void AudioSourcePause(AudioSource audioSource)
        {
            if (IsThisAudioSourcePlaying(audioSource) && !audioSource.ignoreListenerPause)
            {
                audioSource.Pause();
            }
        }

        public static void AudioSourceUnpause(AudioSource audioSource)
        {
            audioSource.UnPause();
        }

        public static void AudioSourceMute(AudioSource audioSource)
        {
            audioSource.mute = true;
        }

        public static void AudioSourceUnmute(AudioSource audioSource)
        {
            audioSource.mute = false;
        }
        #endregion

        #region Getter - Playback position
        public static float GetAudioSourcePlaybackPosition(AudioSource audioSource)
        {
            return audioSource.time;
        }
        #endregion

        #region Setter - Pitch / Volume
        public static void SetAudioSourePitchValue(AudioSource audioSource, float value)
        {
            audioSource.pitch = value;
        }

        public static void SetAudioSourceVolume(AudioSource audioSource, float volume)
        {
            if (!audioSource.ignoreListenerVolume)
            {
                audioSource.volume = volume;
            }
        }
        #endregion

        #region Toggle - Play on awake / isLooping states
        public static void ToggleAudioSourcePlayOnAwakeState(AudioSource audioSource, bool value)
        {
            audioSource.playOnAwake = value;
        }

        public static void ToggleAudioSourceClipLoopState(AudioSource audioSource, bool value)
        {
            audioSource.loop = value;
        }
        #endregion
    }
}