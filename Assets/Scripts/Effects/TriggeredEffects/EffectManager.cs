using System;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager instance;
        public VFXEmitArgs effectEmitArgs;

        [Range(0, 1)]
        [SerializeField] private float loudnessTolerance = 0.8f;
        private MusicAnalyzer musicAnalyzer;
        private GameObject playerObj;

        public bool allowMultipleEffectsAtOnce = false;
        private bool isApplyingEffect = false;
        private bool waitFlag = false;

        private WeightedRandom<IEffect> weightedEffectsBag;

        private void Awake()
        {
            if (instance != null && instance != this) Destroy(this);
            else instance = this;

            playerObj = GameObject.FindGameObjectWithTag("Player");
        }

        private void Start()
        {
            musicAnalyzer = FindObjectOfType<MusicAnalyzer>();
            effectEmitArgs = new VFXEmitArgs(null, null, null);

            List<KeyValuePair<IEffect, double>> effectList = new List<KeyValuePair<IEffect, double>>();

            gameObject.AddComponent<ParticleSizeBeatEffect>().Initialize(musicAnalyzer);

            effectList.Add(new KeyValuePair<IEffect, double>(gameObject.GetComponent<ParticleSizeBeatEffect>(), 10));

            // populate the weightedRandom effects bag we can pick from
            weightedEffectsBag = new WeightedRandom<IEffect>();
            foreach (var x in effectList)
            {
                weightedEffectsBag.Add(x.Key, x.Value);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if ((allowMultipleEffectsAtOnce || !isApplyingEffect) && !waitFlag)
            {
                // if currentAvgLoudness is greater than a tolerance percentage of the maxLoudness
                if (musicAnalyzer.currentAvgLoudnessNormalized > loudnessTolerance)
                {
                    isApplyingEffect = true;

                    // randomly decide an event and some duration
                    var chosenEffect = weightedEffectsBag.GetRandomWeighted();
                    chosenEffect.DoEffect(OnFinishedEffect);
                }
            }
        }

        void OnFinishedEffect()
        {
            isApplyingEffect = false;
        }
    }
}