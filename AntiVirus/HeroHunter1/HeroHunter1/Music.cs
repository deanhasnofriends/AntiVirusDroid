using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

namespace AntiVirus
{
    class Music
    {
        
        static SoundEffect Freeze;
        static SoundEffect BlobDeath;
        static SoundEffect HeroStrike;
        static SoundEffect BlobStrike;
        static SoundEffect HeroDamaged;
        //static SoundEffect BlobDamaged;
        public static int sound_flag = 0;

        //making constructor
        public static void Initialize(
            SoundEffect freezer,
            SoundEffect blobdeath,
            SoundEffect herostrike,
            SoundEffect blobstrike,
            SoundEffect herodamaged
            /*SoundEffect blobdamaged*/)
        {
            Freeze = freezer;
            BlobDeath = blobdeath;
            HeroStrike = herostrike;
            BlobStrike = blobstrike;
            HeroDamaged = herodamaged;
           // BlobDamaged = blobdamaged;
        }

        SoundEffectInstance ByeBlob = BlobDeath.CreateInstance();

        //first method = plays sound effect for blob losing health
        public static void Boom()
        {
            if (Player.health > 0)
            {
                //BlobDamaged.Play();
            }


            
        }

        //method = plays sound effect for blob dealing damage
        public static void Haymaker()
        {
            BlobStrike.Play();

        }

        public static void freeze()
        {
            Freeze.Play();
        }

        public static void End()
        {
            if (sound_flag == 0)
            {
                BlobDeath.Play();
                sound_flag = 1;
            }

        }



    }
}
