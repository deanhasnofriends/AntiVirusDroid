using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;


namespace AntiVirusDroid
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D dav1;
        Texture2D spriteSheet;
        Texture2D titleScreen;
        SpriteFont pericles14;
        Texture2D Ui;
        Texture2D HealthBar;
        Texture2D charac;
        Texture2D ene;
        Texture2D victoryScreen;
        Texture2D thumbstick;
        
        Texture2D endpic;

        SoundEffect freez;
        SoundEffect BlobDeath;
        SoundEffect BlobStrike;
        SoundEffect HeroDamaged;
       // SoundEffect BlobDamaged;

        Song music;
        bool songstart = false;

        enum GameStates { TitleScreen, Playing, Victory, WaveComplete, GameOver };
        GameStates gameState = GameStates.TitleScreen;

        float gameOverTimer = 0.0f;
        float gameOverDelay = 6.0f;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.graphics.PreferredBackBufferWidth = GraphicsDevice.Viewport.Width;
            this.graphics.PreferredBackBufferHeight = GraphicsDevice.Viewport.Height; 
            graphics.PreferMultiSampling = true;
            graphics.IsFullScreen = true;
            this.graphics.ApplyChanges();
            base.Initialize();

        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            BlobDeath = Content.Load<SoundEffect>("Death");
            BlobStrike = Content.Load<SoundEffect>("shot");
            HeroDamaged = Content.Load<SoundEffect>("shot");
          //  BlobDamaged = Content.Load<SoundEffect>("blobdamaged");
            freez = Content.Load<SoundEffect>("Ice");

            

            music = Content.Load<Song>("Main_Song");
            MediaPlayer.IsRepeating = true;

            //end = Content.Load<Song>("Death");
            MediaPlayer.IsRepeating = true;


            dav1 = Content.Load<Texture2D>(@"gate_sheet");
            ene = Content.Load<Texture2D>(@"viral_sheet");
            charac = Content.Load<Texture2D>(@"tank_sheet1");
            spriteSheet = Content.Load<Texture2D>(@"SpriteSheet");
            titleScreen = Content.Load<Texture2D>(@"TitleScreen");
            pericles14 = Content.Load<SpriteFont>(@"Pericles14");
            Ui = Content.Load<Texture2D>(@"bar");
            HealthBar = Content.Load<Texture2D>(@"HealthBar");
            thumbstick = Content.Load<Texture2D>("thumbstick");
            //endpic = Content.Load<Texture2D>("BLOBYO");
            victoryScreen = Content.Load<Texture2D>("vict");
            //ContentManager CLoader = new ContentManager(Services);
           

            Camera.WorldRectangle = new Rectangle(10, 10, 1000, 1000);
            Camera.ViewPortWidth = GraphicsDevice.Viewport.Width;
            Camera.ViewPortHeight = GraphicsDevice.Viewport.Height;

            Music.Initialize(
                   freez,
                   BlobDeath,
                   BlobDeath,
                   BlobStrike,
                   HeroDamaged);


            TileMap.Initialize(spriteSheet);

            Player.Initialize(
                charac,
                new Rectangle(0, 0, 145, 145),
                8,
                new Rectangle(0, 145, 72, 31),
                4,
                new Vector2(50, 50));

            //EffectsManager.Initialize(
            //    spriteSheet,
            //    new Rectangle(0, 288, 2, 2),
            //    new Rectangle(0, 256, 32, 32),
            //    3);

            WeaponManager.Texture = spriteSheet;

            GoalManager.Initialize(
                dav1,
                new Rectangle(0, 0, 63, 78),
                new Rectangle(63, 0, 63, 78),
                1,
                3);


            EnemyManager.Initialize(
                ene,
                new Rectangle(0, 0, 84, 48),
                3);

            




        }


        protected override void UnloadContent()
        {

        }

        private void checkPlayerDeath()
        {
            foreach (Enemy enemy in EnemyManager.Enemies)
            {
                if (enemy.EnemySprite.IsCircleColliding(
                    Player.BaseSprite.WorldCenter,
                    Player.BaseSprite.CollisionRadius))
                {
                    if (Player.helt == false)
                    {
                        Player.health = Player.health - 1;
                        Player.checker2 = "hit";
                        Music.Boom();
                    }
                    else
                    {
                        Player.shield = Player.shield - 1;
                    }

                    if (Player.health <= 0)
                    {
                        gameState = GameStates.GameOver;
                    }
                }
            }
        }


        protected override void Update(GameTime gameTime)
        {

            // update our virtual thumbsticks
            VirtualThumbsticks.Update();


            if (!songstart)
            {
                MediaPlayer.Play(music);
                songstart = true;

            }

            

            switch (gameState)
            {
                case GameStates.TitleScreen:

                    TouchCollection touchCollection = TouchPanel.GetState();
                    foreach (TouchLocation tl in touchCollection)
                    {
                            if ((tl.State == TouchLocationState.Pressed)
                                    || (tl.State == TouchLocationState.Moved))
                            {
                                GameManager.StartNewGame();
                                gameState = GameStates.Playing;
                                Player.health = 100;
                                Music.sound_flag = 0;
                            }

                     }

             

                    

                    break;

                case GameStates.Playing:
                    Player.Update(gameTime);
                    WeaponManager.Update(gameTime);
                    EnemyManager.Update(gameTime);
                    GoalManager.Update(gameTime);

                  

                    if (GoalManager.ActiveTerminals == 0 && EnemyManager.Enemies.Count == 0)
                    {
                        if (Enemy.rounder > 5)
                        {
                            gameState = GameStates.Victory;
                        }
                        else
                        {
                            gameState = GameStates.WaveComplete;
                        }
                    }

                    

                    //Force the health to remain between 0 and 100
                    Player.health = (int)MathHelper.Clamp(Player.health, 0, 100);

                    Player.mana = (int)MathHelper.Clamp(Player.mana, 0, 100);

                    break;

                case GameStates.WaveComplete:
                    {
                        GameManager.StartNewWave();
                        Enemy.rounder++;
                    }

                    gameState = GameStates.Playing;



                    break;

                //case GameStates.Victory:
                //    if ((GamePad.GetState(PlayerIndex.One).Buttons.Start ==
                //        ButtonState.Pressed) ||
                //        (Keyboard.GetState().IsKeyDown(Keys.Space)))
                //    {

                //        gameState = GameStates.Playing;
                //    }

                //    break;

                case GameStates.GameOver:
                    gameOverTimer +=
                        (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (gameOverTimer > gameOverDelay)
                    {
                        gameState = GameStates.TitleScreen;
                        gameOverTimer = 0.0f;
                    }
                    break;
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (VirtualThumbsticks.LeftThumbstickCenter.HasValue)
            {
                spriteBatch.Draw(
                    thumbstick,
                    VirtualThumbsticks.LeftThumbstickCenter.Value - new Vector2(thumbstick.Width / 2f, thumbstick.Height / 2f),
                    Color.Green);
            }

            if (VirtualThumbsticks.RightThumbstickCenter.HasValue)
            {
                spriteBatch.Draw(
                    thumbstick,
                    VirtualThumbsticks.RightThumbstickCenter.Value - new Vector2(thumbstick.Width / 2f, thumbstick.Height / 2f),
                    Color.Blue);
            }


                if (gameState == GameStates.TitleScreen)
                {
                    spriteBatch.Draw(
                        titleScreen,
                        new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                        Color.White);
                }

                if (gameState == GameStates.Victory)
                {
                    spriteBatch.Draw(
                        victoryScreen,
                        new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
                        Color.White);
                }

            if ((gameState == GameStates.Playing) ||
                (gameState == GameStates.WaveComplete) ||
                (gameState == GameStates.GameOver))
            {
                TileMap.Draw(spriteBatch);
                WeaponManager.Draw(spriteBatch);
                Player.Draw(spriteBatch);
                EnemyManager.Draw(spriteBatch);
                GoalManager.Draw(spriteBatch);


                checkPlayerDeath();

               // spriteBatch.DrawString(
               //pericles14,
               //"Score: " + GameManager.Score.ToString(),
               //new Vector2(30, 5),
               //Color.White);

               // spriteBatch.DrawString(
               //    pericles14,
               //    "Shield Level: " + Player.shield.ToString(),
               //    new Vector2(200, 5),
               //    Color.White);

               // spriteBatch.DrawString(
               //    pericles14,
               //    "Round: " + Enemy.rounder.ToString(),
               //    new Vector2(380, 5),
               //    Color.White);

               // spriteBatch.DrawString(
               //    pericles14,
               //    "Cracks Remaining: " + GoalManager.ActiveTerminals.ToString(),
               //    new Vector2(530, 5),
               //    Color.White);

                //spriteBatch.Draw(
                //       Ui,
                //       new Rectangle(0, 580, 1366, 200),
                //       Color.White);

                ////Draw the negative space for the health bar
                //spriteBatch.Draw(HealthBar, new Rectangle(90, 592, 235, 65), new Rectangle(0, 45, HealthBar.Width, 44), Color.Gray);


                ////Healthbar Health
                //spriteBatch.Draw(HealthBar, new Rectangle(90, 592, (int)(HealthBar.Width * ((double)Player.health / 200)), 65),

                // new Rectangle(0, 45, HealthBar.Width, 44), Color.Red);

                ////Draw the box around the health bar
                //spriteBatch.Draw(HealthBar, new Rectangle(90, 592, 235, 65), new Rectangle(0, 0, HealthBar.Width, 44), Color.White);


                ////Draw the negative space for the progress bar
                //spriteBatch.Draw(HealthBar, new Rectangle(90, 687, 235, 65), new Rectangle(0, 45, HealthBar.Width, 44), Color.Gray);


                ////Mana Bar
                //spriteBatch.Draw(HealthBar, new Rectangle(90, 687, (int)(HealthBar.Width * ((double)Player.mana / 200)), 65),

                // new Rectangle(0, 45, HealthBar.Width, 44), Color.Yellow);

                ////Draw the box around the health bar
                //spriteBatch.Draw(HealthBar, new Rectangle(90, 687, 235, 65), new Rectangle(0, 0, HealthBar.Width, 44), Color.White);






            }

            if (gameState == GameStates.WaveComplete)
            {
                spriteBatch.DrawString(
                    pericles14,
                    "Wave Complete!",
                    new Vector2(300, 300),
                    Color.White);

                
            }

            if (gameState == GameStates.GameOver)
            {

                Enemy.rounder = 1;
                spriteBatch.DrawString(
                    pericles14,
                    "G A M E O V E R!",
                    new Vector2(663, 300),
                    Color.White);
                    songstart = false;
                     Music.End();

                     
            }



            spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
