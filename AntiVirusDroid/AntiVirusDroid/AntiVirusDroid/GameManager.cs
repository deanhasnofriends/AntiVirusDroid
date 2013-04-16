using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AntiVirusDroid
{
    static class GameManager
    {
        #region Declarations
        public static int Score = 0;
        public static int CurrentWave = 0;
        public static int BaseTerminalCount = 8;
        public static int MaxTerminalCount = 15;
        public static int CurrentTerminalCount = 8;
        public static Vector2 PlayerStartLoc = new Vector2(32, 32);
        #endregion

        #region Public Methods
        public static void StartNewWave()
        {
            CurrentWave++;
            if (CurrentTerminalCount < MaxTerminalCount)
            {
                CurrentTerminalCount++;
            }

            Player.BaseSprite.WorldLocation = PlayerStartLoc;
            Player.health = 100;
            Player.mana = 100;
            Camera.Position = Vector2.Zero;
            WeaponManager.Shots.Clear();
            EnemyManager.Enemies.Clear();
            TileMap.GenerateRandomMap();
            GoalManager.GenerateComputers(CurrentTerminalCount);
        }

        public static void StartNewGame()
        {
            CurrentWave = 0;
            Score = 0;
            StartNewWave();
        }
        #endregion

    }
}
