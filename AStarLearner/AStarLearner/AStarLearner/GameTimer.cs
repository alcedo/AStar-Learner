using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaHelpers.GameEngine
{

    public delegate void TimerFinishedEventHandler(object sender, EventArgs e);


    /// <summary>
    /// A timer that functions by the framerate of the game
    /// </summary>
    public class GameTimer
    {
        #region Variable Declarations
        /// <summary>
        /// The current frame the timer is on
        /// </summary>
        public int CurrentTime;
        /// <summary>
        /// The target elapsed time(in frames) to reach
        /// </summary>
        public int TargetTime;
        /// <summary>
        /// Whether the timer is paused
        /// </summary>
        public bool Paused = true;
        /// <summary>
        /// This event is raised when the timer has reached the target number of frames
        /// </summary>
        public event TimerFinishedEventHandler TimerFinished;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new GameTimer
        /// </summary>
        /// <param name="targetTime">The elapsed time(in frames) to reach</param>
        public GameTimer(int targetTime)
        {
            //Set the values
            TargetTime = targetTime;
            Paused = false;
        }
        #endregion

        #region Update Method
        /// <summary>
        /// Updates the timer
        /// </summary>
        public void Update()
        {
            //Updates the current time if the timer isn't paused
            if (Paused == false)
            {
                CurrentTime++;
            }
            //Raises the TimerFinished event if the timer is finished, then resets the timer
            if (CurrentTime == TargetTime)
            {
                TimerFinished(this, EventArgs.Empty);
                CurrentTime = 0;
                Paused = true;
            }
        }
        #endregion
    }
}
