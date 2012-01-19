using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStarLearner
{

    /// <summary>
    /// This class stores all relevant game score information
    /// </summary>
    class UserScore
    {
        private string name;            // User name 
        private int userID;             // User ID 
        private double score;           // Current On-going game score. Update high score if higher. 
        private double highScore;       // Highest score achieve
        private int highestCombo;       // Max combo scores achieved. 
        private int maxLevel;           // Max level that the user has achieved 
        private double gameDuration;    // Stores the duration played by user
 
    }

    class GameScoringSystem
    {
       
        // Dictionary that maps username + userid to his score

        // Write and save the score to a file. 

    }



}
