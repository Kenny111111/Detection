using System;
using System.Collections;
using UnityEngine;

namespace Detection
{
	public class ParticleSizeBeatEffect : MonoBehaviour, IEffect
	{
		public MusicAnalyzer musicAnalyzer;

		[Range(0.01f, float.MaxValue)]
		[SerializeField] double minEffectDuration = 0.5;
		[Range(0.01f, 30)]
		[SerializeField] double maxEffectDuration = 15;

		public void Initialize(MusicAnalyzer mAnalyzer) 
		{ 
			musicAnalyzer = mAnalyzer;
		}

		void IEffect.DoEffect(Action callback) => StartCoroutine(DoParticleSizeBeatEffect(callback));

        public IEnumerator DoParticleSizeBeatEffect(Action callback)
		{
			// generate a random duration within defined min/max values
			System.Random rand = new System.Random();
			double randomDuration = rand.NextDouble() * (maxEffectDuration - minEffectDuration) + minEffectDuration;

			double currentTimeCount = 0;

			while (currentTimeCount < randomDuration)
			{
				EffectSystem.effectSystem.effectEmitArgs.size = musicAnalyzer.currentLoudness;
				yield return null;
			}

			// reset the override size
			EffectSystem.effectSystem.effectEmitArgs.size = null;

			callback();
		}
	}
}
