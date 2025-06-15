using UnityEngine;

namespace VG.Animations
{
    public class RandomStateSMB : StateMachineBehaviour
    {
        public string parameter = "RandomIdle";

        public int statesCount = 3;
        public float minNormTime;
        public float maxNormTime = 5f;

        private int hashParam;
        private float randomNormTime;


        private void Awake()
        {
            hashParam = Animator.StringToHash(parameter);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Randomly decide a time at which to transition.
            randomNormTime = Random.Range(minNormTime, maxNormTime);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // If trainsitioning away from this state reset the random idle parameter to 0.
            if (animator.IsInTransition(layerIndex) && animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash ==
                stateInfo.fullPathHash) animator.SetInteger(hashParam, 0);

            // If the state is beyond the randomly decided normalised time and not yet transitioning then set a random idle.
            if (stateInfo.normalizedTime > randomNormTime && !animator.IsInTransition(layerIndex))
                animator.SetInteger(hashParam, Random.Range(1, statesCount + 1));
        }
    }
}