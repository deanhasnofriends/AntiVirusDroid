using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AntiVirus
{
    static class Player
    {
        #region Declarations
        public static Sprite BaseSprite;
        public static Sprite TurretSprite;
        public static Vector2 shootfrom;
        private static float storer;

        private static Vector2 baseAngle = Vector2.Zero;
        public static Vector2 turretAngle = Vector2.Zero;
        private static float playerSpeed = 300f;
        public static int health = 100;
        public static int mana = 100;
        public static int strength = 10;
        public static int intelegence = 10;
        public static string checker2 = "";
        public static string checker;
        private static int collisionRadius = 65;
        public static int tentrange = 40;
        private static int spin;
        private static int used = 0;
        public static int shield = 0;

        private static bool shieldflag = false;
        public static bool helt = false;
        private static bool res = false;
        private static float freezerange = 250;
        private static bool speedflag = false;
        private static bool freezeflag = false;
        
        




        private static float timeElapsed = 0;
        public static float timeElapsed1 = 0;
        public static float timeElapsed2 = 0;
        public static float timeElapsed3 = 0;

        private static Rectangle scrollArea = new Rectangle(150, 100, 500, 400);

        #endregion

        #region Properties
        public static Vector2 PathingNodePosition
        {
            get
            {
                return TileMap.GetSquareAtPixel(BaseSprite.WorldCenter);
            }
        }
        #endregion

        #region Initialization
        public static void Initialize(
            Texture2D texture,
            Rectangle baseInitialFrame,
            int baseFrameCount,
            Rectangle turretInitialFrame,
            int turretFrameCount,
            Vector2 worldLocation)
        {
            int frameWidth = baseInitialFrame.Width;
            int frameHeight = baseInitialFrame.Height;


            BaseSprite = new Sprite(
                worldLocation,
                texture,
                baseInitialFrame,
                Vector2.Zero);

            BaseSprite.CollisionRadius = collisionRadius;

            //GAME PAD
            previousGamePadState = GamePad.GetState(PlayerIndex.One);

            previousKeyPadState = Keyboard.GetState(PlayerIndex.One);

            BaseSprite.BoundingXPadding = 4;
            BaseSprite.BoundingYPadding = 4;
            BaseSprite.AnimateWhenStopped = true;
            for (int x = 1; x < baseFrameCount; x++)
            {
                BaseSprite.AddFrame(
                    new Rectangle(
                        baseInitialFrame.X + (frameHeight * x),
                        baseInitialFrame.Y,
                        frameWidth,
                        frameHeight));
            }


            TurretSprite = new Sprite(
                worldLocation,
                texture,
                turretInitialFrame,
                Vector2.Zero);

            TurretSprite.Animate = false;

            for (int x = 1; x < turretFrameCount; x++)
            {
                BaseSprite.AddFrame(
                    new Rectangle(
                    turretInitialFrame.X + (frameHeight * x),
                    turretInitialFrame.Y,
                    frameWidth,
                    frameHeight));
            }

        }
        #endregion

        #region Input Handling
        

        private static Vector2 handleKeyboardMovement(KeyboardState keyState)
        {
            checker = "Move";


            Vector2 keyMovement = Vector2.Zero;
            if (keyState.IsKeyDown(Keys.W))
            {
                keyMovement.Y--;
                checker = "Move";
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                keyMovement.X--;
                checker = "Move";
            }

            if (keyState.IsKeyDown(Keys.S))
            {
                keyMovement.Y++;
                checker = "Move";
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                keyMovement.X++;
                checker = "Move";
            }

            return keyMovement;
        }

        private static Vector2 handleKeyboardShots(KeyboardState keyState)
        {
            Vector2 keyShots = Vector2.Zero;

            if (keyState.IsKeyDown(Keys.NumPad1))
                keyShots = new Vector2(-1, 1);       
                                                 
            if (keyState.IsKeyDown(Keys.NumPad2)|| keyState.IsKeyDown(Keys.Down))
                keyShots = new Vector2(0, 1);

            if (keyState.IsKeyDown(Keys.NumPad3))
                keyShots = new Vector2(1, 1);

            if (keyState.IsKeyDown(Keys.NumPad4) ||keyState.IsKeyDown(Keys.Left))
                keyShots = new Vector2(-1, 0);

            if (keyState.IsKeyDown(Keys.NumPad6) || keyState.IsKeyDown(Keys.Right))
                keyShots = new Vector2(1, 0);

            if (keyState.IsKeyDown(Keys.NumPad7))
                keyShots = new Vector2(-1, -1);

            if (keyState.IsKeyDown(Keys.NumPad8) || keyState.IsKeyDown(Keys.Up))
                keyShots = new Vector2(0, -1);

            if (keyState.IsKeyDown(Keys.NumPad9))
                keyShots = new Vector2(1, -1);

            return keyShots;


        }

        private static Vector2 handleGamePadMovement(GamePadState gamepadState)
        {
            return new Vector2(gamepadState.ThumbSticks.Left.X, -gamepadState.ThumbSticks.Left.Y);
        }


        private static Vector2 handleGamePadShots(GamePadState gamepadState)
        {
            return new Vector2(gamepadState.ThumbSticks.Right.X, -gamepadState.ThumbSticks.Right.Y);
        }

        public static GamePadState previousGamePadState;
        public static KeyboardState previousKeyPadState;

        private static void handleInput(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 moveAngle = Vector2.Zero;
            Vector2 fireAngle = Vector2.Zero;
            
            freezerange = 250;
            bool meleeflag = true;
            
            moveAngle += handleKeyboardMovement(Keyboard.GetState(PlayerIndex.One));
            moveAngle += handleGamePadMovement(GamePad.GetState(PlayerIndex.One));


            if (moveAngle.X > 0)
            {
                checker = "Move";

            }
            if (moveAngle.X < 0)
            {
                checker = "Move";

            }
            if (moveAngle.Y > 0)
            {
                checker = "Move";

            }
            if (moveAngle.Y < 0)
            {
                checker = "Move";

            }

            // Get the current gamepad state.
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            KeyboardState keyState = Keyboard.GetState(PlayerIndex.One);

            //ATTACK IMPLEMENTATION

            //Freeze attack
            if ((gamePadState.Buttons.Y == ButtonState.Pressed /*&& previousGamePadState.Buttons.Y == ButtonState.Pressed)*/ || (keyState.IsKeyDown(Keys.O) /*&& previousKeyPadState.IsKeyDown(Keys.O)*/)) )
            {
                if (mana < 50)
                {

                }
                else
                {
                    freezeflag = true;

                    storer = Enemy.savespeed;
                    
                    mana = mana - 50;
                    Music.freeze();
                   

                }


            }

            
        

            //Move Boost
            if ( (gamePadState.Buttons.X == ButtonState.Pressed  || keyState.IsKeyDown(Keys.I)  && (mana > 50) ) )
            {

                speedflag = true;
                mana = mana - 50 ;
                Music.Haymaker();

                

            }

            if ((gamePadState.Buttons.A == ButtonState.Pressed || keyState.IsKeyDown(Keys.P) && (mana > 50)))
            {
                if (mana >= 75)
                {
                    shieldflag = true;
                    mana = mana - 75;
                    shield = 75;
                }

            }

            






            fireAngle += handleKeyboardShots(Keyboard.GetState());
            fireAngle += handleGamePadShots(GamePad.GetState(PlayerIndex.One));

            if (moveAngle != Vector2.Zero)
            {
                moveAngle.Normalize();
                baseAngle = moveAngle;
                moveAngle = checkTileObstacles(elapsed, moveAngle);
            }

            if (fireAngle != Vector2.Zero)
            {

               


                fireAngle.Normalize();

                
                turretAngle = fireAngle;
                
                

                
                    if (WeaponManager.CanFireWeapon)
                    {
                        shootfrom = new Vector2(0, 0);


                        if (turretAngle.X > 0 && turretAngle.Y < 0)
                        {
                            shootfrom = new Vector2(60, -55);
                        }
                        if (turretAngle.X < 0 && turretAngle.Y < 0)
                        {
                            shootfrom = new Vector2(-30, -35);
                        }
                        if (turretAngle.X < 0 && turretAngle.Y > 0)
                        {
                            shootfrom = new Vector2(-35, 35);
                        }
                        if (turretAngle.X > 0 && turretAngle.Y > 0)
                        {
                            shootfrom = new Vector2(60, 35);
                        }



                        if (turretAngle.X < -0.87)
                        {
                            shootfrom = new Vector2(-40, 0);
                        }
                        if (turretAngle.X > 0.87)
                        {
                            shootfrom = new Vector2(80, 0);
                        }
                        if (turretAngle.Y > 0.87)
                        {
                            shootfrom = new Vector2(10, 70);
                        }
                        if (turretAngle.Y < -0.87)
                        {
                            shootfrom = new Vector2(10, -62);
                        }

                        


                        Music.Haymaker();
                        WeaponManager.FireWeapon(
                            TurretSprite.WorldLocation + shootfrom, 
                            fireAngle * WeaponManager.WeaponSpeed);
                    }
                
            }
            else
            {
                turretAngle = baseAngle;
            }

            
            TurretSprite.RotateTo(turretAngle);
            BaseSprite.Velocity = moveAngle * playerSpeed;

            BaseSprite.RotateTo(baseAngle);

            repositionCamera(gameTime, moveAngle);
        }
        #endregion

        #region Movement Limitations
        private static void clampToWorld()
        {
            float currentX = BaseSprite.WorldLocation.X;
            float currentY = BaseSprite.WorldLocation.Y;

            currentX = MathHelper.Clamp(
                currentX,
                0,
                Camera.WorldRectangle.Right - BaseSprite.FrameWidth);

            currentY = MathHelper.Clamp(
                currentY,
                0,
                Camera.WorldRectangle.Bottom - BaseSprite.FrameHeight);

            BaseSprite.WorldLocation = new Vector2(currentX, currentY);
        }

        private static void repositionCamera(
            GameTime gameTime,
            Vector2 moveAngle)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float moveScale = playerSpeed * elapsed;

            if ((BaseSprite.ScreenRectangle.X < scrollArea.X) &&
                (moveAngle.X < 0))
            {
                Camera.Move(new Vector2(moveAngle.X, 0) * moveScale);
            }

            if ((BaseSprite.ScreenRectangle.Right > scrollArea.Right) &&
                (moveAngle.X > 0))
            {
                Camera.Move(new Vector2(moveAngle.X, 0) * moveScale);
            }

            if ((BaseSprite.ScreenRectangle.Y < scrollArea.Y) &&
                (moveAngle.Y < 0))
            {
                Camera.Move(new Vector2(0, moveAngle.Y) * moveScale);
            }

            if ((BaseSprite.ScreenRectangle.Bottom > scrollArea.Bottom) &&
                (moveAngle.Y > 0))
            {
                Camera.Move(new Vector2(0, moveAngle.Y) * moveScale);
            }
        }

        private static Vector2 checkTileObstacles(
            float elapsedTime,
            Vector2 moveAngle)
        {
            Vector2 newHorizontalLocation = BaseSprite.WorldLocation +
                (new Vector2(moveAngle.X, 0) * (playerSpeed * elapsedTime));

            Vector2 newVerticalLocation = BaseSprite.WorldLocation +
                (new Vector2(0, moveAngle.Y) * (playerSpeed * elapsedTime));

            Rectangle newHorizontalRect = new Rectangle(
                (int)newHorizontalLocation.X,
                (int)BaseSprite.WorldLocation.Y,
                BaseSprite.FrameWidth,
                BaseSprite.FrameHeight);

            Rectangle newVerticalRect = new Rectangle(
                (int)BaseSprite.WorldLocation.X,
                (int)newVerticalLocation.Y,
                BaseSprite.FrameWidth,
                BaseSprite.FrameHeight);

            int horizLeftPixel = 0;
            int horizRightPixel = 0;

            int vertTopPixel = 0;
            int vertBottomPixel = 0;

            if (moveAngle.X < 0)
            {
                horizLeftPixel = (int)newHorizontalRect.Left;
                horizRightPixel = (int)BaseSprite.WorldRectangle.Left;
            }

            if (moveAngle.X > 0)
            {
                horizLeftPixel = (int)BaseSprite.WorldRectangle.Right;
                horizRightPixel = (int)newHorizontalRect.Right;
            }

            if (moveAngle.Y < 0)
            {
                vertTopPixel = (int)newVerticalRect.Top;
                vertBottomPixel = (int)BaseSprite.WorldRectangle.Top;
            }

            if (moveAngle.Y > 0)
            {
                vertTopPixel = (int)BaseSprite.WorldRectangle.Bottom;
                vertBottomPixel = (int)newVerticalRect.Bottom;
            }

            if (moveAngle.X != 0)
            {
                for (int x = horizLeftPixel; x < horizRightPixel; x++)
                {
                    for (int y = 0; y < BaseSprite.FrameHeight; y++)
                    {
                        if (TileMap.IsWallTileByPixel(new Vector2(x, newHorizontalLocation.Y + y)))
                        {
                            moveAngle.X = 0;
                            break;
                        }
                    }
                    if (moveAngle.X == 0)
                    {
                        break;
                    }
                }
            }

            if (moveAngle.Y != 0)
            {
                for (int y = vertTopPixel; y < vertBottomPixel; y++)
                {
                    for (int x = 0; x < BaseSprite.FrameWidth; x++)
                    {
                        if (TileMap.IsWallTileByPixel(new Vector2(newVerticalLocation.X + x, y)))
                        {
                            moveAngle.Y = 0;
                            break;
                        }
                    }
                    if (moveAngle.Y == 0)
                    {
                        break;
                    }
                }
            }

            return moveAngle;
        }

        #endregion

        #region Update and Draw

        public static void Update(GameTime gameTime)
        {
            if (baseAngle.X > 0 && baseAngle.Y < 1)
            {
                TurretSprite.WorldLocation = BaseSprite.WorldLocation + new Vector2(16, 58);
            }
            if (baseAngle.X < 0)
            {
                TurretSprite.WorldLocation = BaseSprite.WorldLocation + new Vector2(60, 58);
            }
            if(baseAngle.Y < 1 && baseAngle.X == 0)
            {
                TurretSprite.WorldLocation = BaseSprite.WorldLocation + new Vector2(35, 82);
            }
            if (baseAngle.Y > .5)
            {
                TurretSprite.WorldLocation = BaseSprite.WorldLocation + new Vector2(35, 40);
            }




            if (shieldflag)
            {
                timeElapsed3 += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if ((timeElapsed3 < 10f && shieldflag == true) && (shield > 0))
                {
                    helt = true;
                    checker = "Spin";
                }
                else
                {

                    shieldflag = false;
                    timeElapsed3 = 0.0f;
                    helt = false;
                    shield = 0;

                }
            }

            handleInput(gameTime);
            if (spin == 0)
            {
                BaseSprite.Update2(gameTime, checker);
                spin = 0;
                checker2 = "";
            }
            
            clampToWorld();

            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (freezeflag == true)
            {
                timeElapsed2 += (float)gameTime.ElapsedGameTime.TotalSeconds;
                foreach (Enemy enemy in EnemyManager.Enemies)
                {
                    if (Vector2.Distance(Player.BaseSprite.WorldCenter, enemy.EnemySprite.WorldLocation) <= freezerange)
                    {
                        if (timeElapsed2 < 1.8f && freezeflag == true)
                        {
                            
                            enemy.EnemySpeed = 0;
                            enemy.frozen = true;
                        }
                        
                    }
                    if (timeElapsed2 >= 1.8f && freezeflag == true)
                    {
                        enemy.frozen = false;
                        if (Enemy.rounder == 1)
                        {
                            enemy.EnemySpeed = 60f;
                        }
                        else
                        {
                            enemy.EnemySpeed = storer;
                        }
                         res = true;
                        
                    }
                }

                if (res)
                {
                    freezeflag = false;
                    timeElapsed2 = 0.0f;
                    res = false;
                }


            }


            if (speedflag == true)
            {
                timeElapsed1 += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timeElapsed1 < 1f && speedflag == true)
                {
                    playerSpeed = 900;
                }
                else
                {

                    speedflag = false;
                    timeElapsed1 = 0.0f;
                    playerSpeed = 300;
                }
            }

            

            if (timeElapsed >= 0.1)
            {
                mana = mana + 1;

                timeElapsed = 0;
            }



        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            BaseSprite.Draw(spriteBatch);
            TurretSprite.Draw2(spriteBatch);

        }
        #endregion

        

    }
}
