using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TheGenerationGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TheGenerationGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private int generationNumber = 0;
        private int numberKilled = 0;

        private Flock sheepFlock;
        private const int NUM_STARTING_ANIMALS = 10;

        private Texture2D blankTexture;
        private SpriteFont generationFont;

        private int windowWidthSize;
        private int windowHeightSize;
        private const int HORIZONTAL_MARGIN = 10;
        private const int VERTICAL_MARGIN = 50;

        public TheGenerationGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            windowHeightSize = GraphicsDevice.Viewport.Height;
            windowWidthSize = GraphicsDevice.Viewport.Width;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            sheepFlock = new Flock { };

            //Create Random number generator used to make random animal colours
            Random r = new Random();

            //Produce list of animals
            for (int i = 0; i < NUM_STARTING_ANIMALS; i++)
            {
                sheepFlock.Add(new Sheep(new Color((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble())));
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            generationFont = this.Content.Load<SpriteFont>("GenerationFont");
            Sheep.Outline = this.Content.Load<Texture2D>("SheepOutline");
            Sheep.Inside = this.Content.Load<Texture2D>("SheepInsides");
            blankTexture = this.Content.Load<Texture2D>("BlankTexture");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>


        MouseState lastMouseState;

        public enum Action
        {
            Kill,
            Breed
        }

        Action LastThingDone = Action.Kill;
        private int numberBorn;

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            //Get Width and Height of Window
            windowHeightSize = GraphicsDevice.Viewport.Height;
            windowWidthSize = GraphicsDevice.Viewport.Width;

            //Set animal size
            int numberRowsColumns = (int)Math.Round(Math.Sqrt(sheepFlock.Count) + 0.5);

            Sheep.Width = (windowWidthSize - (HORIZONTAL_MARGIN * 2)) / numberRowsColumns;
            Sheep.Height = (windowHeightSize - (VERTICAL_MARGIN * 2)) / numberRowsColumns;

            //Set animal positions
            float newXPosition = HORIZONTAL_MARGIN;
            float newYPosition = VERTICAL_MARGIN;

            for(int i = 0; i < sheepFlock.Count; i++)
            {
                sheepFlock[i].Position.X = newXPosition;
                sheepFlock[i].Position.Y = newYPosition;

                newXPosition = newXPosition + Sheep.Width;

                if (newXPosition + Sheep.Width > (windowWidthSize - HORIZONTAL_MARGIN))
                {
                    newXPosition = HORIZONTAL_MARGIN;
                    newYPosition = newYPosition + Sheep.Height;
                }
            }

            MouseState currentMouseState = Mouse.GetState();
            if (lastMouseState != null && this.IsActive)
            {
                if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (LastThingDone == Action.Breed)
                    {
                        sheepFlock = Wolfpack.KillSheep(sheepFlock, out numberKilled);
                        LastThingDone = Action.Kill;
                        generationNumber++;
                    }
                    else
                    {
                        int numberBeforeBreed = sheepFlock.Count;
                        sheepFlock.BreedRandomSheep();
                        int numberAfterBreed = sheepFlock.Count;
                        numberBorn = numberAfterBreed - numberBeforeBreed;
                        LastThingDone = Action.Breed;
                        generationNumber++;
                    }
                }
            }
            lastMouseState = currentMouseState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            this.spriteBatch.Draw(blankTexture, new Rectangle(0, 0, (int)windowWidthSize, (int)windowHeightSize), new Color(0, 0.9f, 0));
            this.spriteBatch.DrawString(generationFont, "Turn: " + generationNumber, new Vector2(10, 10), new Color(1, 1, 1));

            if (generationNumber != 0)
            {
                if (LastThingDone == Action.Kill)
                {
                    this.spriteBatch.DrawString(generationFont, "Number Killed This Turn: " + numberKilled, new Vector2(190, 10), new Color(1, 1, 1));
                }
                else
                {
                    this.spriteBatch.DrawString(generationFont, "Number New Borns This Turn: " + numberBorn, new Vector2(190, 10), new Color(1, 1, 1));
                }
            }

            this.spriteBatch.DrawString(generationFont, "Number Sheep: " + sheepFlock.Count, new Vector2(600, 10), new Color(1, 1, 1));

            foreach(Sheep animal in sheepFlock)
            {
                animal.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
