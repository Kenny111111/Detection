using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


namespace Detection
{
	// Usage example: FindObjectOfType<AudioSystem>();
	public class AudioSystem : MonoBehaviour
	{
		public static AudioSystem instance;
		public AudioMixerGroup audioMxrGroup;
		public List<Sound> soundsList;

		void Awake()
		{
			// Ensure only one instance exists
			if (instance == null)
			{
				instance = this;
			}
			else Destroy(gameObject);

			foreach (Sound sound in soundsList)
			{
				sound.source = gameObject.AddComponent<AudioSource>();
				sound.source.clip = sound.clip;
				sound.source.loop = sound.loop;
				sound.source.outputAudioMixerGroup = audioMxrGroup;
			}

			DontDestroyOnLoad(this.gameObject);
		}

		public void Play(string soundName)
		{
			Sound mySound = TryGetSound(soundName);
			if (mySound == null)
			{
				Debug.LogError("Sound " + soundName + " not found");
				return;
			}

			float volumeVariance = UnityEngine.Random.Range(-mySound.volumeDeviation / 2f, mySound.volumeDeviation / 2f) + 1f;
			float pitchVariance = UnityEngine.Random.Range(-mySound.pitchDeviation / 2f, mySound.pitchDeviation / 2f) + 1f;
			mySound.source.volume = mySound.volume * volumeVariance;
			mySound.source.pitch = mySound.pitch * pitchVariance;

			mySound.source.Play();
		}

		public void FadeOut(string soundName, float duration)
		{
			StartCoroutine(FadeOutSound(TryGetSound(soundName), duration));
		}

		private static IEnumerator FadeOutSound(Sound sound, float duration)
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

		public Sound TryGetSound(string soundName)
		{
			Sound sound = soundsList.Find(item => item.name == soundName);
			if (sound == null)
			{
				Debug.Log("Couldnt find sound " + soundName + " in audioSystem");
				return null;
			}
			else return sound;
		}
	}
}
