using System;
using System.Collections;
using UnityEngine;

namespace Detection
{
	public class ParticleSizeBeatEffect : MonoBehaviour, IEffect
	{
		private MusicAnalyzer musicAnalyzer;

		[Range(0.01f, float.MaxValue)]
		[SerializeField] double minEffectDuration = 0.5;
		[Range(0.01f, 30)]
		[SerializeField] double maxEffectDuration = 2;

		[SerializeField] float loudnessToSizeScalar = 0.02f;

        public int Weight { get; set; }

        public void Awake()
        {
			Weight = 10;
			musicAnalyzer = FindObjectOfType<MusicAnalyzer>();
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
				EffectManager.instance.effectEmitArgs.size = musicAnalyzer.currentAvgLoudnessNormalized * loudnessToSizeScalar;
				yield return null;
			}

			// reset the override size
			EffectManager.instance.effectEmitArgs.size = null;

			callback();
		}
	}
}
