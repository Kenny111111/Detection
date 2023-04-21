using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;

namespace Detection
{
	// Usage example: FindObjectOfType<MusicSystem>();
	public class MusicSystem : MonoBehaviour
	{
		public static MusicSystem instance;
		public AudioMixerGroup audioMxrGroup;
		public Queue<Sound> musicQueue;
		public Sound songPlaying;

		public static event Action<Sound> UpdatedSongPlaying;

		void Awake()
		{
			// Ensure only one musicSystem exists
			if (instance == null)
			{
				instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
			else Destroy(gameObject);

			musicQueue = new Queue<Sound>();
		}

        private void Start()
        {
			StartCoroutine(PlaySongQueue());
		}

        private static IEnumerator PlaySongQueue()
		{
			float waitAmount = 0.25f;

			while (true)
			{
				if (instance.musicQueue.Count > 0)
				{
					// Start playing the next song
					instance.songPlaying = instance.musicQueue.Peek();
					instance.songPlaying.source.Play();

					UpdatedSongPlaying?.Invoke(instance.songPlaying);

					Sound songForDequeue = instance.songPlaying;

					float currentSongLength = instance.songPlaying.source.clip.length;
					yield return new WaitForSeconds(currentSongLength + waitAmount);

					// Try to remove it from the front of the queue
					instance.TryDequeue(songForDequeue);
				}

				yield return new WaitForSeconds(waitAmount);
			}
		}

		public bool TryDequeue(Sound songToRemove)
		{
			if (musicQueue.Count == 0) return false;

			Debug.Log("TryDequeue.. " + songToRemove.name);
			Sound top = musicQueue.Peek();
			if (songToRemove.name == top.name)
			{
				musicQueue.Dequeue();
				return true;
			}
			else return false;
		}

		public void TryStopAndClearQueue()
        {
			if (songPlaying == null || songPlaying.source == null || songPlaying.source.isPlaying == false) return;

			songPlaying.source.Stop();
			musicQueue.Clear();
		}

		public void ResetQueue()
		{
			StopCoroutine(PlaySongQueue());

			TryStopAndClearQueue();

			StartCoroutine(PlaySongQueue());
		}

		public bool TryEnqueue(Sound songToAdd)
		{
			if (songToAdd == null || songToAdd.name == null) return false;

			// If we arent able to find the current song in the queue, add it.
			if (musicQueue.ToList().Find(item => item.name == songToAdd.name) == null)
			{
				songToAdd.source = gameObject.AddComponent<AudioSource>();
				songToAdd.source.clip = songToAdd.clip;
				songToAdd.source.loop = songToAdd.loop;
				songToAdd.source.outputAudioMixerGroup = audioMxrGroup;

				float volumeVariance = UnityEngine.Random.Range(-songToAdd.volumeDeviation / 2f, songToAdd.volumeDeviation / 2f) + 1f;
				float pitchVariance = UnityEngine.Random.Range(-songToAdd.pitchDeviation / 2f, songToAdd.pitchDeviation / 2f) + 1f;
				songToAdd.source.volume = songToAdd.volume * volumeVariance;
				songToAdd.source.pitch = songToAdd.pitch * pitchVariance;

				musicQueue.Enqueue(songToAdd);

				return true;
			}
			else return false;
		}

		public Sound TryGetCurrentSong()
		{
			if (musicQueue.Count > 0) return musicQueue.Peek();
			else return null;
		}

		private Sound TryGetSongInQueue(string soundName)
		{
			Sound sound = musicQueue.ToList().Find(item => item.name == soundName);
			if (sound == null)
			{
				Debug.Log("Couldnt find song: " + soundName + ", in MusicSystem");
				return null;
			}
			else return sound;
		}

		private void ForcePlay(string songName)
		{
			Sound song = TryGetSongInQueue(songName);
			if (song == null)
			{
				Debug.LogError("Sound: " + song.name + ", not found");
				return;
			}

			float volumeVariance = UnityEngine.Random.Range(-song.volumeDeviation / 2f, song.volumeDeviation / 2f) + 1f;
			float pitchVariance = UnityEngine.Random.Range(-song.pitchDeviation / 2f, song.pitchDeviation / 2f) + 1f;
			song.source.volume = song.volume * volumeVariance;
			song.source.pitch = song.pitch * pitchVariance;

			FadeOut(TryGetCurrentSong(), 2);
			song.source.Play();
		}

		private void FadeOut(Sound song, float duration)
		{
			StartCoroutine(FadeOutSound(song, duration));
		}

		private static IEnumerator FadeOutSound(Sound sound, float duration)
		{
			if (sound != null)
			{
				float startVolume = sound.volume;

				while (sound.volume > 0)
				{
					sound.volume -= startVolume * Time.deltaTime / duration;

					yield return null;
				}

				sound.source.Stop();
				sound.volume = startVolume;
			}
		}
	}
}
