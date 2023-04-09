using System;
using System.Collections;
using UnityEngine;

namespace Detection
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager instance;
        public VFXEmitArgs effectEmitArgs;
        private MusicAnalyzer musicAnalyzer;

        [Range(0, 1)]
        [SerializeField] private float loudnessTolerance = 0.8f;

        public bool allowMultipleEffectsAtOnce = false;
        private bool isApplyingEffect = false;
        private bool waitFlag = false;

        private WeightedRandom<IEffect> weightedEffectsBag;
        private IEffect[] effectsFound;

        private void Awake()
        {
            if (instance != null && instance != this) Destroy(this);
            else instance = this;
        }

        private void Start()
        {
            effectEmitArgs = new VFXEmitArgs(null, null, null);
            musicAnalyzer = FindObjectOfType<MusicAnalyzer>();
            effectsFound = GameObject.FindGameObjectWithTag("Effects").GetComponents<IEffect>();

            // populate the weightedRandom effects bag we can pick from
            weightedEffectsBag = new WeightedRandom<IEffect>();

            foreach (IEffect effect in effectsFound)
            {
                weightedEffectsBag.Add(effect, effect.Weight);
            }
        }

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
                    if (chosenEffect != null) chosenEffect.DoEffect(OnFinishedEffect);
                    
                    if (!waitFlag) StartCoroutine(WaitBeforeNextEffect());
                }
            }
        }

        public IEnumerator WaitBeforeNextEffect()
        {
            waitFlag = true;
            const float WAIT_TIME = 0.25f;

            double currentTimeCount = 0;
            while (currentTimeCount < WAIT_TIME)
            {
                currentTimeCount += Time.deltaTime;
                yield return null;
            }

            waitFlag = false;
        }

        void OnFinishedEffect()
        {
            isApplyingEffect = false;
        }
    }
}