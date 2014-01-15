using System;

namespace TheGenerationGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TheGenerationGame game = new TheGenerationGame())
            {
                game.Run();
            }
        }
    }
#endif
}

