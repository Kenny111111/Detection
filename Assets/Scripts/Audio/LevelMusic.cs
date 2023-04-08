using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
	public class LevelMusic : MonoBehaviour
	{
		public static LevelMusic levelMusic;
		public List<Sound> songList;
		private MusicSystem musicSystem;

		void Start()
		{
			// Ensure only one LevelMusic object exists at one time..
			if (levelMusic == null) levelMusic = this;
			else Destroy(gameObject);

			musicSystem = FindObjectOfType<MusicSystem>();

			musicSystem.ResetQueue();
			AddSongsToQueue();
		}

		public void AddSongsToQueue()
		{
			foreach (Sound song in songList)
			{
				musicSystem.TryEnqueue(song);
			}
		}
	}
}
