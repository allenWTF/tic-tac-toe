using System;
using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Recorder of time that was spent in a game
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class GameTimer : MonoBehaviour
    {
        private TMP_Text timer;
        private Stopwatch stopwatch;

        private void Awake()
        {
            timer = GetComponent<TMP_Text>();
            stopwatch = new Stopwatch();
            Play();
            StartCoroutine(UpdateTimer());
        }

        /// <summary>
        /// Start timer
        /// </summary>
        public void Play()
        {
            stopwatch.Start();
        }

        /// <summary>
        /// Pause timer
        /// </summary>
        public void Pause()
        {
            stopwatch.Stop();
        }

        public float GetCurrentTime()
        {
            return stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Update UI representation of timer
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                var timespan = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
                timer.text = timespan.ToString(@"mm\:ss");
            }
        }
    }
}