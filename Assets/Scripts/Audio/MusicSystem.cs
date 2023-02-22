using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

// Usage example: FindObjectOfType<MusicSystem>();
public class MusicSystem : MonoBehaviour
{
	public static MusicSystem instance;
	public AudioMixerGroup audioMxrGroup;
	public Queue<Sound> musicQueue;

	public static event Action<Sound> OnSongChanged;

	void Awake()
	{
		// Ensure only one instance exists
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else Destroy(gameObject);

		musicQueue = new Queue<Sound>();

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
				instance.musicQueue.Peek().source.Play();
				OnSongChanged?.Invoke(instance.musicQueue.Peek());

				float currentSongLength = instance.TryGetCurrentSong().source.clip.length;
				yield return new WaitForSeconds(currentSongLength + waitAmount);
				// remove it from the list since it has completed playing

				instance.musicQueue.Dequeue();
			}

			yield return new WaitForSeconds(waitAmount);
		}
	}


	public bool TryEnqueue(Sound songToAdd)
    {
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
		OnSongChanged?.Invoke(song);
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
