using UnityEngine;

namespace VG.Animations
{
    public class SceneLinkedSMB<T> : ExtendedSMB
    {
        protected T owner;

        public static void Initialize(Animator animator, T owner)
        {
            var sceneLinkedSMBs = animator.GetBehaviours<SceneLinkedSMB<T>>();

            for (var i = 0; i < sceneLinkedSMBs.Length; i++)
                sceneLinkedSMBs[i].InternalInitialize(animator, owner);
        }

        protected void InternalInitialize(Animator animator, T owner)
        {
            this.owner = owner;
            OnStart(animator);
        }


        /// <summary>
        ///     Called by a MonoBehaviour in the scene during its Start function.
        /// </summary>
        public virtual void OnStart(Animator animator) { }
    }
}