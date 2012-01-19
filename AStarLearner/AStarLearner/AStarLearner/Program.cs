using System;

namespace AStarLearner
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            /* 
            //Uncomment this to run the actual implementation
            using (Game1 game = new Game1())
            {
               //  game.Run();
            } 
            */

            // Define your own test case solution like this:
            using (VictorTestDriver test = new VictorTestDriver())
            {
               //test.Run();
            }

            using (MainGame test = new MainGame())
            {
                test.Run();
            }

			//  using (RyanTestDriver test = new RyanTestDriver()) { test. Run(); }
			
        }
    }
#endif
}

