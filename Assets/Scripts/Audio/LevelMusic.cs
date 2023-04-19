using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Detection
{
	public class LevelMusic : MonoBehaviour
	{
		public static LevelMusic levelMusic;
		public List<Sound> songList;

		void Start()
		{
			// Ensure only one LevelMusic object exists at one time..
			if (levelMusic == null) levelMusic = this;
			else Destroy(gameObject);

			AddSongsToQueue();
		}

		public void AddSongsToQueue()
		{
			foreach (Sound song in songList)
			{
				if (!MusicSystem.instance.TryEnqueue(song))
                {
					Debug.Log("Failed to Enqueue song...");
                }

				int x = 0;
				StringBuilder sb = new StringBuilder();
				sb.Append("-LogMusicQueue-");
				foreach (Sound sss in MusicSystem.instance.musicQueue.ToArray())
				{
					sb.Append("Song " + x + " " + sss.name);
					x++;
				}
				sb.Append("------");
				Debug.Log(sb.ToString());
			}
		}
	}
}
