using UnityEngine;
using UnityEngine.Animations;

namespace VG.Animations
{
    public abstract class ExtendedSMB : SealedSMB
    {
        protected bool firstFrameHappened;
        protected bool lastFrameHappened;


        public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            firstFrameHappened = false;

            OnSLStateEnter(animator, stateInfo, layerIndex);
            OnSLStateEnter(animator, stateInfo, layerIndex, controller);
        }

        public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            lastFrameHappened = false;

            OnSLStateExit(animator, stateInfo, layerIndex);
            OnSLStateExit(animator, stateInfo, layerIndex, controller);
        }

        public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (!animator.gameObject.activeSelf)
                return;

            if (animator.IsInTransition(layerIndex) &&
                animator.GetNextAnimatorStateInfo(layerIndex).fullPathHash == stateInfo.fullPathHash)
            {
                OnSLTransitionToStateUpdate(animator, stateInfo, layerIndex);
                OnSLTransitionToStateUpdate(animator, stateInfo, layerIndex, controller);
            }

            if (!animator.IsInTransition(layerIndex) && firstFrameHappened)
            {
                OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
                OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex, controller);
            }

            if (animator.IsInTransition(layerIndex) && !lastFrameHappened && firstFrameHappened)
            {
                lastFrameHappened = true;

                OnSLStatePreExit(animator, stateInfo, layerIndex);
                OnSLStatePreExit(animator, stateInfo, layerIndex, controller);
            }

            if (!animator.IsInTransition(layerIndex) && !firstFrameHappened)
            {
                firstFrameHappened = true;

                OnSLStatePostEnter(animator, stateInfo, layerIndex);
                OnSLStatePostEnter(animator, stateInfo, layerIndex, controller);
            }

            if (animator.IsInTransition(layerIndex) && animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash ==
                stateInfo.fullPathHash)
            {
                OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex);
                OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex, controller);
            }

            OnSLStateUpdate(animator, stateInfo, layerIndex);
            OnSLStateUpdate(animator, stateInfo, layerIndex, controller);
        }

        public virtual void OnSLStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        public virtual void OnSLStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller) { }


        /// <summary>
        ///     Called before Updates when execution of the state first starts (on transition to the state).
        /// </summary>
        public virtual void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        /// <summary>
        ///     Called after OnSLStateEnter every frame during transition to the state.
        /// </summary>
        public virtual void
            OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        /// <summary>
        ///     Called on the first frame after the transition to the state has finished.
        /// </summary>
        public virtual void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        /// <summary>
        ///     Called every frame after PostEnter when the state is not being transitioned to or from.
        /// </summary>
        public virtual void
            OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        /// <summary>
        ///     Called on the first frame after the transition from the state has started.  Note that if the transition has a
        ///     duration of less than a frame, this will not be called.
        /// </summary>
        public virtual void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        /// <summary>
        ///     Called after OnSLStatePreExit every frame during transition to the state.
        /// </summary>
        public virtual void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        /// <summary>
        ///     Called after Updates when execution of the state first finshes (after transition from the state).
        /// </summary>
        public virtual void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        /// <summary>
        ///     Called before Updates when execution of the state first starts (on transition to the state).
        /// </summary>
        public virtual void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller) { }

        /// <summary>
        ///     Called after OnSLStateEnter every frame during transition to the state.
        /// </summary>
        public virtual void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller) { }

        /// <summary>
        ///     Called on the first frame after the transition to the state has finished.
        /// </summary>
        public virtual void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller) { }

        /// <summary>
        ///     Called every frame when the state is not being transitioned to or from.
        /// </summary>
        public virtual void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller) { }

        /// <summary>
        ///     Called on the first frame after the transition from the state has started.  Note that if the transition has a
        ///     duration of less than a frame, this will not be called.
        /// </summary>
        public virtual void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller) { }

        /// <summary>
        ///     Called after OnSLStatePreExit every frame during transition to the state.
        /// </summary>
        public virtual void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex, AnimatorControllerPlayable controller) { }

        /// <summary>
        ///     Called after Updates when execution of the state first finshes (after transition from the state).
        /// </summary>
        public virtual void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller) { }
    }

    //This class repalce normal StateMachineBehaviour. It add the possibility of having direct reference to the object
    //the state is running on, avoiding the cost of retrienving it through a GetComponent every time.
    //c.f. Documentation for more in depth explainations.
    public abstract class SealedSMB : StateMachineBehaviour
    {
        public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    }
}