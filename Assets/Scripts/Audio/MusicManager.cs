using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
	public class MusicManager : MonoBehaviour
	{
		public static MusicManager musicManager;
		public List<String> songList;
		private List<String> haventPlayedList;

		private AudioSystem audioSystem;

		private int currentSongIndex = 0;

		void Awake()
		{
			// Ensure only one manager exists
			if (musicManager == null) musicManager = this;
			else Destroy(gameObject);

			haventPlayedList = new List<String>();
			foreach (String songName in songList)
			{
				haventPlayedList.Add(songName);
			}

			audioSystem = FindObjectOfType<AudioSystem>();
		}

		public string GetCurrentSongName()
		{
			if (songList.Count == 0)
			{
				Debug.LogError("songList is empty. There is no current song name");
				return null;
			}
			else return songList[currentSongIndex];
		}

		public Sound GetCurrentSong()
        {
			return audioSystem.TryGetSound(GetCurrentSongName());
		}

		public bool PlayNextSongInOrder()
		{
			if (currentSongIndex + 1 > songList.Count) return false;

			if (currentSongIndex == 0)
			{
				audioSystem.Play(songList[currentSongIndex]);
				StartCoroutine(VerifyPlaying());
				return true;
			}

			audioSystem.FadeOut(songList[currentSongIndex], 1);
			audioSystem.Play(songList[++currentSongIndex]);

			gameObject.GetComponent<MusicAnalyzer>().UpdateSongPlaying();

			Debug.Log("played song " + songList[currentSongIndex]);

			StartCoroutine(VerifyPlaying());
			return true;
		}

		IEnumerator VerifyPlaying()
		{
			while (true)
			{
				yield return new WaitForSeconds(1f);
				if (!GetCurrentSong().source.isPlaying)
				{
					if (PlayNextSongInOrder()) Debug.Log("switched to next song");
					else Debug.Log("no songs left to play for this level");
				}
			}
		}
	}
}
