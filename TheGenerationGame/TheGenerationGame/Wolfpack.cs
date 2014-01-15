using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGenerationGame
{
    class Wolfpack
    {
        private static Random r = new Random();

        public static Flock KillSheep(Flock SheepFlock, out int numberkilled)
        {
            //Work out what percent of sheep we will kill this generation
            
            int numberSheepToKill = 0;
            float percentOfSheepToKill = 0;
            do
            {
                percentOfSheepToKill = (float)r.NextDouble();
            } 
            while(percentOfSheepToKill > 0.6f);

            numberSheepToKill = (int)(percentOfSheepToKill * SheepFlock.Count);

            numberkilled = numberSheepToKill;

            return KillNumberOfSheep(numberSheepToKill, SheepFlock);
        }

        private static Flock KillNumberOfSheep(int numberSheepToKill, Flock SheepFlock)
        {
            int successfulKills = 0;
            int huntingSkillThisGeneration = r.Next(10, 100);

            float liklihoodRequiredForKill = 0;
            liklihoodRequiredForKill = r.Next(0, 100);

            for (int i = 0; i < SheepFlock.Count; i++)
            {
                Sheep sheep = SheepFlock[i];
                if (successfulKills == numberSheepToKill)
                {
                    break;
                }

                if(sheep.Colour.G == 0)
                {
                    sheep.Colour.G = 1;
                }

                if (sheep.Colour.R == 0)
                {
                    sheep.Colour.R = 1;
                }

                if (sheep.Colour.B == 0)
                {
                    sheep.Colour.B = 1;
                }

                float percentageGreeness = sheep.Colour.G / ((sheep.Colour.R + sheep.Colour.G + sheep.Colour.B) / 100);
                
                float likelyhoodOfKill = huntingSkillThisGeneration + (-1 * percentageGreeness);

                if (likelyhoodOfKill > liklihoodRequiredForKill)
                {
                    //Kill sheep
                    SheepFlock.Remove(sheep);

                    successfulKills++;
                    i++;
                }
            }

            if(successfulKills != numberSheepToKill)
            {
                KillNumberOfSheep(numberSheepToKill - successfulKills, SheepFlock);
            }

            return SheepFlock;
        }
    }
}
