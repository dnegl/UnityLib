using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Views;
using strange.extensions.context.api;
using Object = UnityEngine.Object;

namespace Services.Bootstrap
{
    /// <summary>
    /// Collects all data of folders in Resources folder to be looked up, 
    /// Looks up during several frames (depends on looking up speed and amount of folders that need to be looked up)
    /// </summary>
    public class BootstrapService : IBootstrapService
    {
        /// <summary>
        /// If it's true, that means all folders are looked up.
        /// </summary>
        public bool IsDone
        {
            get { return Progress >= 1f; }
        }
        /// <summary>
        /// Progress of looking up. Must be 1 when looking up completes. See <see cref="IsDone" /> property
        /// </summary>
        public float Progress
        {
            get { return _progress; }
        }

        /// <summary>
        /// Here all the information about folders that need to be looked up must be set.
        /// </summary>
        private void SetFolders()
        {
			//How to Add
            //Add<FieldObjectFxBubbles>("FX/Bubbles/", "FX_Bubbles");
        }

        /// <summary>
        /// It's a maximum time that looking up folders should take in one frame.
        /// If looking up takes more time, it will be stops and continues at next frame.
        /// </summary>
        private const double MaxTimePerTickInSeconds = 0.000001f;

        #region Internal Logic

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject ContextView { get; set; }

        /// <summary>
        /// List of all folders that need to be looked up
        /// </summary>
        private List<CheckFolderInfo> _foldersToCheck;   

        /// <summary>
/// Stopwatch to measure the time that looking up folders operation takes
        /// </summary>
        private Stopwatch _stopwatch;

        private float _progress;

        public BootstrapService()
        {
            _foldersToCheck = new List<CheckFolderInfo>();
            _stopwatch = new Stopwatch();

            SetFolders();
        }

        /// <summary>
        /// Starts Looking up folders. Works in coroutine. See properties <see cref="Progress"/> and <see cref="IsDone"/>
        /// </summary>
        public void Run()
        {
            var mb = ContextView.GetComponent<MonoBehaviour>();

            _progress = 0;
            _stopwatch.Start();
            mb.StartCoroutine(CheckFolders());
        }

        /// <summary>
        /// Adds information about folder that needs to be looked up
        /// </summary>
        /// <typeparam name="T">type of objects to search</typeparam>
        /// <param name="folder"></param>
        /// <param name="section"></param>
        private void Add<T>(string folder, string section) where T : Object
        {
            _foldersToCheck.Add(new CheckFolderInfo<T>(folder, section));
        }
        /// <summary>
        /// Adds information about folder that needs to be looked up
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="section"></param>
        private void Add(string folder, string section)
        {
            Add<Object>(folder, section);
        }

        /// <summary>
        /// Coroutine function that looks up folders. 
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckFolders()
        {
            for (int i = 0; i < _foldersToCheck.Count; i++)
            {
                _progress = (i + 1f)/_foldersToCheck.Count;

                _foldersToCheck[i].CheckFolder();

                if (_stopwatch.Elapsed.TotalSeconds >= MaxTimePerTickInSeconds)
                {
                    yield return null;

                    _stopwatch.Reset();
                    _stopwatch.Start();
                }
            }

            _foldersToCheck.Clear();
        }

        #region Internal classes
        private abstract class CheckFolderInfo
        {
            public abstract void CheckFolder();
        }
        private class CheckFolderInfo<T> : CheckFolderInfo where T : Object
        {
            /// <summary>
            /// Path to folder that needs to be looked up. Relative to Resources folder
            /// </summary>
            private readonly string _folderPath;
            /// <summary>
            /// Category for the assets that will be found
            /// </summary>
            private readonly string _category;

            /// <summary>
            /// Calls ResourcesCache to check folder
            /// </summary>
            public override void CheckFolder()
            {
                ResourcesCache.CacheSection<T>(_folderPath, _category);
            }
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="folderPath">Path to folder that needs to be looked up. Relative to Resources folder</param>
            /// <param name="category">Category for the assets that will be found</param>
            public CheckFolderInfo(string folderPath, string category)
            {
                _folderPath = folderPath;
                _category = category;
            }
        }
        #endregion Internal classes

        #endregion
    }
}