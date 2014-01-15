using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGenerationGame
{
    public class Flock : List<Sheep>
    {
        public void BreedRandomSheep()
        {
            //Shuffle for randomness
            Shuffle<Sheep>(this);

            List<Sheep> Lambs = new List<Sheep> { };
            for(int i = 0; i < this.Count / 2; i++)
            {
                if (this.Count > this.Count / 2 + i)
                {
                    Sheep lamb = this[i].Breed(this[this.Count / 2 + i]);
                    Lambs.Add(lamb);
                }
            }

            this.AddRange(Lambs);
        }

        private static void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public class Sheep
    {
        public Vector2 Position;
        public static int Width;
        public static int Height;

        public static Texture2D Outline;
        public static Texture2D Inside;

        public Color Colour;
        

        public Sheep(Color startingColour)
        {
            Colour = startingColour;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Inside, new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height), Colour);
            sb.Draw(Outline, new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height), new Color(1,1,1));
        }

        public Sheep Breed(Sheep otherAnimal)
        {
            //Find midpoint of each var
            int red = (this.Colour.R + otherAnimal.Colour.R) / 2;
            int green = (this.Colour.G + otherAnimal.Colour.G) / 2;
            int blue = (this.Colour.B + otherAnimal.Colour.B) / 2;

            Color sheepColour = new Color(red, green, blue, 255);

            return new Sheep(sheepColour);
        }
    }
}
