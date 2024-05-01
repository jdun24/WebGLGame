using System;
using UnityEngine;


namespace SFX
{
    namespace Music
    {
        public enum Asteroid
        {
            [ResourcePath("Audio/Music/main_menu")]
            MainMenu,

            [ResourcePath("Audio/Music/big_js_face_in_space")]
            BigJ,

            [ResourcePath("Audio/Music/space_church")]
            SpaceChurch
        }

        public enum Cave
        {
            [ResourcePath("Audio/Music/cave_ambience")]
            Ambience,

            [ResourcePath("Audio/Music/cave_braver")]
            Braver
        }
    }

    public enum Cave
    {
        [ResourcePath("Audio/SFX/cave_enter")]
        Enter,

        [ResourcePath("Audio/SFX/cave_exit")]
        Exit,

        [ResourcePath("Audio/SFX/resource_exhausted")]
        MineResource,
    }

    public enum Player
    {
        [ResourcePath("Audio/SFX/player_walk")]
        Walk,

        [ResourcePath("Audio/SFX/player_jump")]
        Jump,
        [ResourcePath("Audio/SFX/player_work")]
        Work,
    }

    public enum Satellite
    {
        [ResourcePath("Audio/SFX/satellite_spawn")]
        Spawn,

        [ResourcePath("Audio/SFX/satellite_scan")]
        Scan,
    }

    public enum MiniGame
    {
        [ResourcePath("Audio/SFX/minigame_won")]
        Won,

        [ResourcePath("Audio/SFX/minigame_error")]
        Error,

        [ResourcePath("Audio/SFX/minigame_switch")]
        Switch,

        [ResourcePath("Audio/SFX/minigame_pullcord")]
        PullCord,
    }

    public enum UI
    {
        [ResourcePath("Audio/SFX/ui_click")]
        Click,
    }

    namespace Robot
    {
        public enum Status
        {
            [ResourcePath("Audio/SFX/robot_init")]
            Initialize,

            [ResourcePath("Audio/SFX/robot_dead")]
            Dead,
        }

        public enum Pickup
        {
            [ResourcePath("Audio/SFX/robot_happy")]
            Happy,

            [ResourcePath("Audio/SFX/robot_excited")]
            Excited,
        }

        public enum Dropped
        {
            [ResourcePath("Audio/SFX/robot_sad")]
            Sad,
        }
    }

    namespace Extractor
    {
        public enum Status
        {
            [ResourcePath("Audio/SFX/extractor_humming")]
            Humming,
        }
    }

    public class ResourcePathAttribute : Attribute
    {
        public string resourcePath { get; private set; }

        public ResourcePathAttribute(string path)
        {
            resourcePath = path;
        }
    }
}
