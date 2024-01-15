using System.Collections;
using UnityEngine;

namespace ProgramAR
{
    namespace Menu
    {
        public class PageController : MonoBehaviour
        {
            // Make this a singleton because there should only be one PageController
            public static PageController Instance { get; private set; }

            public bool debug;
            // Page the loads at beginning
            public PageType entryPage;
            public Page[] pages;

            public PageType currentPage = PageType.None;

            // Relation between PageType and Pages to locate pages easily 
            private Hashtable m_Pages;

            #region Unity Functions
            private void Awake()
            {
                if (Instance != null && Instance != this)
                {
                    Destroy(this);
                }
                else
                {
                    Instance = this;
                    m_Pages = new Hashtable();
                    RegisterAllPages();

                    if (entryPage != PageType.None)
                    {
                        TurnPageOn(entryPage);
                    }
                }
            }

            #endregion

            #region Public Functions
            public void TurnPageOn(PageType _type)
            {
                // Confirm page is not None type
                if (_type == PageType.None) return;

                // If page isn't there log a warning
                if (!PageExists(_type))
                {
                    LogWarning("You are trying to turn a page on [" + _type + "] that has not been registered.");
                    return;
                }

                // Turn page on
                Page _page = GetPage(_type);
                _page.gameObject.SetActive(true);
                _page.Animate(true);

                currentPage = _type;
            }

            public void TurnPageOnSingle(PageType _type)
            {
                if (currentPage == PageType.None)
                {
                    TurnPageOn(_type);
                }
                else
                {
                    TurnPageOff(currentPage, _type);
                }
            }
            public void TurnPageOff(PageType _off, PageType _on = PageType.None, bool _waitForExit = false)
            {
                if (_off == PageType.None) return;

                // If page isn't there log a warning
                if (!PageExists(_off))
                {
                    LogWarning("You are trying to turn a page off [" + _off + "] that has not been registered.");
                    return;
                }

                // Turn page off
                Page _offPage = GetPage(_off);
                if (_offPage.gameObject.activeSelf)
                {
                    _offPage.Animate(false);
                }

                if (_on != PageType.None)
                {
                    Page _onPage = GetPage(_on);

                    if (_waitForExit)
                    {
                        //StopCoroutine("WaitForPageExit");
                        StartCoroutine(WaitForPageExit(_onPage, _offPage));
                    }
                    else
                    {
                        TurnPageOn(_on);
                    }
                }

                // Keep track of current page to create radio buttons
                if (_on != PageType.None)
                {
                    currentPage = _on;
                }
            }

            #endregion

            #region Private Functions
            // Wait for off page to turn off before turning on page on
            private IEnumerator WaitForPageExit(Page _on, Page _off)
            {
                // While animating, wait
                while (_off.targetState != Page.FLAG_NONE)
                {
                    yield return null;
                }

                TurnPageOn(_on.type);
            }
            private void RegisterAllPages()
            {
                foreach (Page _page in pages)
                {
                    RegisterPage(_page);
                }
            }
            private void RegisterPage(Page _page)
            {
                if (PageExists(_page.type))
                {
                    LogWarning("You are trying to register a page [" + _page.type + "] that has already been registered: " + _page.gameObject.name);
                    return;
                }

                m_Pages.Add(_page.type, _page);
                Log("Registered new page [" + _page.type + "]");
            }

            private Page GetPage(PageType _type)
            {
                if (!PageExists(_type))
                {
                    LogWarning("You are trying to get a page [" + _type + "] that has not been registered");
                    return null;
                }
                // Hash tables carry generic types so we need to type cast the page
                return (Page)m_Pages[_type];
            }

            private bool PageExists(PageType _type)
            {
                return m_Pages.ContainsKey(_type);
            }
            private void Log(string _msg)
            {
                if (!debug) return;
                Debug.Log("[PageController]: " + _msg);
            }

            private void LogWarning(string _msg)
            {
                if (!debug) return;
                Debug.LogWarning("[PageController]: " + _msg);
            }
            #endregion
        }
    }
}
