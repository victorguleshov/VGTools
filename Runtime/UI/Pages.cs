// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VG.UI
{
    public interface IAppearanceAnimator
    {
        void AppearAnimation(GameObject go, Action onComplete = null);
        void DisappearAnimation(GameObject go, Action onComplete = null);
    }

    [ExecuteAlways]
    public class Pages : MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private GameObject[] pages;
        [SerializeField] private Toggle[] pagination;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button prevButton;
        [SerializeField] private int currentIndex;
        [SerializeField] private bool loopPages;

        private IAppearanceAnimator pageAnimator;

        private UnityAction<bool>[] paginationActions;

        public int CurrentIndex
        {
            get => currentIndex;
            protected set => currentIndex = value;
        }

        public int Count => pages?.Length ?? 0;

        public GameObject Current =>
            pages != null && CurrentIndex >= 0 && CurrentIndex < Count ? pages[CurrentIndex] : null;

        public RectTransform Content => content;

        protected void Start()
        {
            Subscribe();
            UpdateNavigationButtons();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            if (paginationActions != null) Unsubscribe();

            nextButton?.onClick.AddListener(NextPage);
            prevButton?.onClick.AddListener(PreviousPage);

            if (pagination == null || pages == null) return;

            paginationActions = new UnityAction<bool>[pagination.Length];

            for (var i = 0; i < pagination.Length; i++)
            {
                var index = i;
                if (pagination[i].isOn) CurrentIndex = index;

                paginationActions[i] = isOn =>
                {
                    if (isOn)
                    {
                        CurrentIndex = index;
                        ShowPage(pages[index]);
                        UpdateNavigationButtons();
                    }
                    else
                    {
                        HidePage(pages[index]);
                    }
                };

                pagination[i].onValueChanged.AddListener(paginationActions[i]);
            }
        }

        private void Unsubscribe()
        {
            nextButton?.onClick.RemoveListener(NextPage);
            prevButton?.onClick.RemoveListener(PreviousPage);

            if (pagination == null || pages == null || paginationActions == null) return;

            for (var i = 0; i < pagination.Length; i++)
                if (paginationActions[i] != null)
                    pagination[i].onValueChanged.RemoveListener(paginationActions[i]);
        }

        private void UpdateNavigationButtons()
        {
            if (prevButton) prevButton.interactable = loopPages || CurrentIndex > 0;
            if (nextButton) nextButton.interactable = loopPages || CurrentIndex < Count - 1;
        }

        public void ShowPage(GameObject page, bool withAnimation = true)
        {
            if (page == null) return;
            if (withAnimation && pageAnimator != null)
                pageAnimator.AppearAnimation(page);
            else
                page.SetActive(true);
        }

        public void HidePage(GameObject page, bool withAnimation = true)
        {
            if (page == null) return;
            if (withAnimation && pageAnimator != null)
                pageAnimator.DisappearAnimation(page);
            else
                page.SetActive(false);
        }

        public void SelectPage(int index)
        {
            ClampIndex(ref index);

            if (pagination != null && index < pagination.Length)
            {
                pagination[index].isOn = true;
                return;
            }

            if (pagination != null && CurrentIndex < pagination.Length)
                pagination[CurrentIndex].isOn = false;
            else
                HidePage(Current);

            CurrentIndex = index;
            ShowPage(GetPageByIndex(index));
            UpdateNavigationButtons();
        }

        private void ClampIndex(ref int index)
        {
            if (loopPages)
            {
                index %= Count;
                while (index < 0) index += Count;
            }
            else
            {
                if (index < 0) index = 0;
                if (index >= Count) index = Count - 1;
            }
        }

        private void SelectPageImmediately(int index)
        {
            ClampIndex(ref index);

            if (pagination != null && index < pagination.Length)
            {
                pagination[index].isOn = true;
                return;
            }

            if (pagination != null && CurrentIndex < pagination.Length)
                pagination[CurrentIndex].isOn = false;
            else
                GetPageByIndex(CurrentIndex)?.SetActive(false);

            CurrentIndex = index;
            GetPageByIndex(index)?.SetActive(true);
            UpdateNavigationButtons();
        }

        public void NextPage()
        {
            if (loopPages || CurrentIndex < Count - 1)
                SelectPage(CurrentIndex + 1);
        }

        public void PreviousPage()
        {
            if (loopPages || CurrentIndex > 0)
                SelectPage(CurrentIndex - 1);
        }

        public void SetAnimator(IAppearanceAnimator appearanceAnimator)
        {
            pageAnimator = appearanceAnimator;
        }

        private GameObject GetPageByIndex(int index)
        {
            return pages != null && index >= 0 && index < Count ? pages[index] : null;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!EditorApplication.isPlaying && pages != null && pages.Length > 0)
                for (var i = 0; i < pages.Length; i++)
                    if (pages[i].activeSelf && CurrentIndex != i)
                    {
                        SelectPageImmediately(i);
                        break;
                    }
        }

        private void OnValidate()
        {
            if (pages == null || pages.Length == 0) return;
            ClampIndex(ref currentIndex);

            SelectPageImmediately(currentIndex);
            UpdateNavigationButtons();
        }

#endif
    }
}