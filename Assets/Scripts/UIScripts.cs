﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIScripts : MonoBehaviour {

    [SerializeField]
    public Transform spawnPoint;
    public bool scale;
   



    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    int incr = -1;
    public void SpawnStone(int stoneId) {
        Debug.Log(LoadObjectFromBundle.sceneStones.Count);
        if (scale)
        {
        	Debug.Log("scale");
            GameObject obj = Instantiate(LoadObjectFromBundle.sceneStones[stoneId - 1], spawnPoint.position, Quaternion.Euler(-90, 0, 0));
            Debug.Log(obj.name);
            obj.transform.localScale += new Vector3(150.0f, 150.0f, 150.0f);
        }
        else
        {
        	Debug.Log("no scale");
            GameObject g = LoadObjectFromBundle.sceneStones[stoneId - 1];
            Debug.Log(g.name);
            Instantiate(g, spawnPoint.position, Quaternion.Euler(-90, 0, 0));
        }
    }

    public void Load() {
        //Recover the values
        SaveGame.Load();

        //Set values
        object[] obj = FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;

            if (6 < g.name.Length)
            {
                if (g.name.Substring(0, 5).Equals("Stone"))
                {
                    int ind = SaveGame.Instance.StonesNames.IndexOf(g.scene.name+g.name);
                    if (ind != -1)
                    {
                        g.transform.position = SaveGame.Instance.StonesPositions[ind];
                        g.transform.rotation = SaveGame.Instance.StonesRotations[ind];
                    }
                    else {
                        Debug.Log("NULL");
                    }
                }
            }
        }
    }

    public void Save() {
        //Get the actual stone's values
        //Modify SaveGame.Instance.Stones
        SaveGame.Instance.Clear();

        object[] obj = FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj) {
            GameObject g = (GameObject)o;
            if (6 < g.name.Length)
            {
                if (g.name.Substring(0, 5).Equals("Stone"))
                {
                    SaveGame.Instance.StonesNames.Add(g.scene.name + g.name);
                    SaveGame.Instance.StonesPositions.Add(g.transform.position);
                    SaveGame.Instance.StonesRotations.Add(g.transform.rotation);
                }
            }
        }

        //Save values
        SaveGame.Save();
    }

}
