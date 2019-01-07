﻿using UnityEngine;
using System.Collections.Generic;

public class SaveGame
{

    //Not using this because the save method doesnt allow it
    public class SavedStone {
        private string Name;
        private Vector3 Position;
        private Quaternion Rotation;

        public SavedStone(string n, Vector3 p, Quaternion r) {
            this.Name = n;
            this.Position = p;
            this.Rotation = r;
        }

        public string GetName() {
            return this.Name;
        }

        public Vector3 GetPosition() {
            return this.Position;
        }

        public Quaternion GetRotation() {
            return this.Rotation;
        }
    }

    //serialized
    public string PlayerName = "Player";
    public int XP = 0;
    public List<string> StonesNames = new List<string>();
    public List<Vector3> StonesPositions = new List<Vector3>();
    public List<Quaternion> StonesRotations = new List<Quaternion>();

    private static string _gameDataFileName = "data.json";

    private static SaveGame _instance;
    public static SaveGame Instance
    {
        get
        {
            if (_instance == null)
                Load();
            return _instance;
        }

    }

    public static void Save()
    {
        FileManager.Save(_gameDataFileName, _instance);
    }

    public static void Load()
    {
        _instance = FileManager.Load<SaveGame>(_gameDataFileName);
    }

    public void Clear() {
        this.StonesNames.Clear();
        this.StonesPositions.Clear();
        this.StonesRotations.Clear();
    }
}