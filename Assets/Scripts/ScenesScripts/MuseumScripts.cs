﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class MuseumScripts : MonoBehaviour {

    [SerializeField]
    public Transform spawnPoint;
    public GameObject showButton;
    public GameObject hideButton;
    public Canvas addStoneMenu;
    public Canvas editStoneMenu;
    public Canvas saveDialog;
    public Canvas loadDialog;
    public Canvas overwriteDialog;
    public Canvas availableFiles;
    public InputField saveInputField;
    public InputField loadInputField;

    // Use this for initialization
    void Start()
    {
        hideButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*0 al 7 EchmiadzinAlly
    8 al 13 Museum
    14 al 26 Noradus
    27 al 45 Noravank
    46 al 58 WallStones*/

    float GetStoneScaleById(int stoneId)
    {
        float[] scales = new float[] { 1.0f, 1.0f, 1.0f, 0.55f, 1.0f };
        int index = 0;
        if (stoneId <= 7)
        {
            index = 0;
        }
        else if (stoneId <= 13)
        {
            index = 1;
        }
        else if (stoneId <= 26)
        {
            index = 2;
        }
        else if (stoneId <= 45)
        {
            index = 3;
        }
        else if (stoneId <= 58)
        {
            index = 4;
        }

        return scales[index];
    }

    int GetStoneId(int number, string name)
    {
        int id = 0;
        if (name.Equals("EchmiadzinAlly"))
        {
            id = number;
        }
        else if (name.Equals("museum"))
        {
            id = number + 7;
        }
        else if (name.Equals("Noradus"))
        {
            id = number + 13;
        }
        else if (name.Equals("Noravank"))
        {
            id = number + 26;
        }
        else if (name.Equals("wallStones"))
        {
            id = number + 45;
        }

        return id + 1;
    }

    public void SpawnStone(int stoneId)
    {
        SpawnStoneWithPositionAndRotation(stoneId, spawnPoint.position, Quaternion.Euler(-90, 0, 0));
    }

    public void SpawnStoneWithPositionAndRotation(int stoneId, Vector3 sp, Quaternion rt)
    {
        float scale = GetStoneScaleById(stoneId);
        GameObject obj = Instantiate(LoadObjectFromBundle.sceneStones[stoneId - 1], sp, rt);
        obj.transform.localScale *= scale;
    }

    public void ShowMenus()
    {
        addStoneMenu.enabled = true;
        showButton.SetActive(false);
        hideButton.SetActive(true);
    }

    public void HideMenus()
    {
        addStoneMenu.enabled = false;
        showButton.SetActive(true);
        hideButton.SetActive(false);
    }

    public void ShowSaveDialog()
    {
        saveDialog.enabled = true;
    }

    public void ShowLoadDialog()
    {
        loadDialog.enabled = true;
    }

    public void ShowAvailableFiles()
    {
        availableFiles.enabled = true;

        List<string> jsonFiles = new List<string>();
        foreach (string file in System.IO.Directory.GetFiles(Application.persistentDataPath))
        {
            if (file.Contains(".json"))
            {
                string[] f = file.Split('\\');
                string fa = f[f.Length - 1];
                jsonFiles.Add(fa);
            }
        }

        string ans = "";
        foreach (string x in jsonFiles)
        {
            ans = ans + x.Split('.')[0] + "\n";
        }

        Text t = availableFiles.GetComponent<Canvas>().GetComponentInChildren<Text>();
        t.text = ans;
        
    }

    public void Load(Boolean cancel)
    {
        if (cancel)
        {
            loadDialog.enabled = false;
            availableFiles.enabled = false;
            return;
        }

        //Recover the values
        SaveGame.Load(loadInputField.text);

        //Set values
        object[] obj = FindObjectsOfType(typeof(GameObject));
        List<string> done = new List<string>();

        //If stone is in the scene
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
                        done.Add(g.scene.name + g.name);
                    }
                    else
                    {
                        Debug.Log("NULL");
                    }
                }
                else if (g.name.Contains("Clone"))
                {
                    int ind = SaveGame.Instance.StonesNames.IndexOf(g.name);
                    if (ind != -1)
                    {
                        g.transform.position = SaveGame.Instance.StonesPositions[ind];
                        g.transform.rotation = SaveGame.Instance.StonesRotations[ind];
                        done.Add(g.name);
                    }
                    else
                    {
                        Debug.Log("NULL");
                    }
                }
            }
        }

        //If not
        foreach (string name in SaveGame.Instance.StonesNames)
        {
            if (!done.Contains(name))
            {
                // name is "scene_StoneNumber" 
                // ej: "museum_Stone01(Clone)" or just "museum_Stone01"
                string[] firstSplit = name.Split('_');
                string[] secondSplit = firstSplit[1].Split('(');
                string number = secondSplit[0].Substring(5);
                try
                {
                    int result = Int32.Parse(number);
                    int i = GetStoneId(result, firstSplit[0]);
                    int ind = SaveGame.Instance.StonesNames.IndexOf(name);
                    if (ind != -1)
                    {
                        Vector3 sp = SaveGame.Instance.StonesPositions[ind];
                        Quaternion rt = SaveGame.Instance.StonesRotations[ind];
                        SpawnStoneWithPositionAndRotation(i, sp, rt);
                    }
                    else
                    {
                        Debug.Log("NULL");
                    }
                }
                catch (FormatException)
                {
                    Debug.Log("ERROR");
                    continue;
                }
            }            
        }

        loadDialog.enabled = false;
        availableFiles.enabled = false;
        loadInputField.text = "";
    }

    public void Save(Boolean cancel)
    {
        if (cancel)
        {
            saveDialog.enabled = false;
            return;
        }
        //Get the actual stone's values
        //Modify SaveGame.Instance.Stones

        string filePath = Path.Combine(Application.persistentDataPath, saveInputField.text + ".json");

        if (File.Exists(filePath))
        {
            overwriteDialog.enabled = true;
            saveDialog.enabled = false;
            return;
        }

        SaveGame.Instance.Clear();

        object[] obj = FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj) {
            GameObject g = (GameObject)o;
            if (6 < g.name.Length)
            {
                if (g.name.Contains("Clone"))
                {
                    SaveGame.Instance.StonesNames.Add(g.name);
                    SaveGame.Instance.StonesPositions.Add(g.transform.position);
                    SaveGame.Instance.StonesRotations.Add(g.transform.rotation);
                }

                if (g.name.Substring(0, 5).Equals("Stone"))
                {
                    SaveGame.Instance.StonesNames.Add(g.scene.name + g.name);
                    SaveGame.Instance.StonesPositions.Add(g.transform.position);
                    SaveGame.Instance.StonesRotations.Add(g.transform.rotation);
                }
            }
        }

        //Save values
        SaveGame.Save(saveInputField.text);

        saveDialog.enabled = false;
        saveInputField.text = "";
    }

    public void RawSave()
    {
        SaveGame.Instance.Clear();

        object[] obj = FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if (6 < g.name.Length)
            {
                if (g.name.Contains("Clone"))
                {
                    SaveGame.Instance.StonesNames.Add(g.name);
                    SaveGame.Instance.StonesPositions.Add(g.transform.position);
                    SaveGame.Instance.StonesRotations.Add(g.transform.rotation);
                }

                if (g.name.Substring(0, 5).Equals("Stone"))
                {
                    SaveGame.Instance.StonesNames.Add(g.scene.name + g.name);
                    SaveGame.Instance.StonesPositions.Add(g.transform.position);
                    SaveGame.Instance.StonesRotations.Add(g.transform.rotation);
                }
            }
        }

        //Save values
        SaveGame.Save(saveInputField.text);

        overwriteDialog.enabled = false;
        saveInputField.text = "";
    }

}
