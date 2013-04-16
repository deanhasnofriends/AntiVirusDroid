using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AntiVirusDroid
{
    class Enemy
    {
        #region Declarations
        public Sprite EnemySprite;
        public float EnemySpeed = 60f;
        public Vector2 currentTargetSquare;
        public bool Destroyed = false;
        private int collisionRadius = 29;
        public int health = 100;
        string ani;
        public static int rounder = 1;
        public static int modifier;
        public static float savespeed;
        public bool hit = false;
        public bool frozen = false;


        
        #endregion

        #region Constructor
        public Enemy(Vector2 worldLocation, Texture2D texture, Rectangle initialFrame)
        {
            EnemySprite = new Sprite(worldLocation, texture, initialFrame, Vector2.Zero);

            int frameWidth = initialFrame.Width;
            int frameHeight = initialFrame.Height;
            EnemySprite.BoundingXPadding = 1;
            EnemySprite.BoundingYPadding = 1;

            EnemySprite.AnimateWhenStopped = true;
            for (int x = 1; x < EnemyManager.doff; x++)
            {
                EnemySprite.AddFrame(
                    new Rectangle(
                        initialFrame.X + (frameWidth * x),
                        initialFrame.Y,
                        frameWidth,
                        frameHeight));
            }

            EnemySprite.CollisionRadius = collisionRadius;

            
            if (rounder > 1)
            {
               modifier = rounder;

                EnemySpeed = (EnemySpeed + (12 * modifier));

                savespeed = EnemySpeed;

               /* if (EnemySpeed > 200)
                {
                    EnemySpeed = 190;
                }*/

                
            }

            

        }
        #endregion

        #region AI
        private Vector2 determineMoveDirection()
        {
            if (reachedTargetSquare())
            {
                currentTargetSquare = getNewTargetSquare();
            }

            Vector2 squareCenter = TileMap.GetSquareCenter(currentTargetSquare);

            return squareCenter - EnemySprite.WorldCenter;
        }

        private bool reachedTargetSquare()
        {
            return (Vector2.Distance(EnemySprite.WorldCenter, TileMap.GetSquareCenter(currentTargetSquare)) <= 2);
        }

        private Vector2 getNewTargetSquare()
        {
            List<Vector2> path = PathFinder.FindPath(TileMap.GetSquareAtPixel(EnemySprite.WorldCenter), TileMap.GetSquareAtPixel(Player.BaseSprite.WorldCenter));

            if (path.Count > 1)
            {
                return new Vector2(path[1].X, path[1].Y);
            }
            else
            {
                return TileMap.GetSquareAtPixel(Player.BaseSprite.WorldCenter);
            }
        }

        private string getcheck(Vector2 dir)
        {
            if(hit)
            {
                hit = false;
                return ("shot");
                
            }
            else if (frozen)
            {
                return ("shot");
            }
            else
            {
                return ("ene");
            }
        }

        #endregion

        #region Public Methods
        public void Update(GameTime gameTime)
        {
            if (!Destroyed)
            {
                Vector2 direction = determineMoveDirection();
                direction.Normalize();

                ani = getcheck(direction);

                EnemySprite.Velocity = direction * EnemySpeed;
                EnemySprite.RotateTo(direction);
                EnemySprite.UpdateEne(gameTime, ani);

                Vector2 directionToPlayer = Player.BaseSprite.WorldCenter - EnemySprite.WorldCenter;
                directionToPlayer.Normalize();

                if (health <= 0)
                {
                    Destroyed = true;
                }


            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Destroyed)
            {
                EnemySprite.Draw(spriteBatch);
            }
        }
        #endregion

    }
}
