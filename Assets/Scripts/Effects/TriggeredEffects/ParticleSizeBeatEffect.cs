using System;
using System.Collections;
using UnityEngine;

namespace Detection
{
	public class ParticleSizeBeatEffect : MonoBehaviour, IEffect
	{
		[Range(0.01f, float.MaxValue)]
		[SerializeField] double minEffectDuration = 0.5;
		[Range(0.01f, 30)]
		[SerializeField] double maxEffectDuration = 2;

		[SerializeField] float loudnessToSizeScalar = 0.04f;

        public int Weight { get; set; }
		[SerializeField] private int weight = 5;

		public void Awake()
        {
			Weight = weight;
		}

		void IEffect.DoEffect(Action callback) => StartCoroutine(DoParticleSizeBeatEffect(callback));

        public IEnumerator DoParticleSizeBeatEffect(Action callback)
		{
			// generate a random duration within defined min/max values
			System.Random rand = new System.Random();
			double randomDuration = rand.NextDouble() * (maxEffectDuration - minEffectDuration) + minEffectDuration;

			for (float t = 0f; t < randomDuration; t += Time.deltaTime)
			{
				EffectManager.instance.effectEmitArgs.size = MusicAnalyzer.instance.currentAvgLoudnessNormalized * loudnessToSizeScalar;
				yield return null;
			}

			// reset the override size
			EffectManager.instance.effectEmitArgs.size = null;
			callback();
		}
	}
}
