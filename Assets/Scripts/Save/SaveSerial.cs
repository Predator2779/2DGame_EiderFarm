using Building;
using Economy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TriggerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Menu))]
public class SaveSerial : MonoBehaviour
{
    [SerializeField] private Inventory _playerInventory;

    [SerializeField] private GameObject[] _gagaHouses;
    [SerializeField] private GameObject[] _cleaners;
    [SerializeField] private GameObject[] _clothMachines;

    [SerializeField] private Menu _menu;
    

    private string path = "/dataSaveFile.dat";

    SaveData data = new SaveData();

    private void Awake()
    {
        if (_menu.GetResetValue())
            ResetData();
        else
        LoadGame();
    }
    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (!File.Exists(Application.persistentDataPath + path))
            file = File.Create(Application.persistentDataPath + path);
        else
            file = File.Open(Application.persistentDataPath + path, FileMode.Open);

        data.Money = _playerInventory.GetAllItems()[0].GetCount();
        data.CleanedFluff = _playerInventory.GetAllItems()[1].GetCount();
        data.UncleanedFluff = _playerInventory.GetAllItems()[2].GetCount();
        data.Item = _playerInventory.GetAllItems()[3].GetCount();

        data.GagaHouses = SaveDataGrades(_gagaHouses);
        data.Cleaners = SaveDataGrades(_cleaners);
        data.ClothMachines = SaveDataGrades(_clothMachines);

        bf.Serialize(file, data);
        file.Close();

        
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + path, FileMode.Open);

            data = (SaveData)bf.Deserialize(file);
            file.Close();

            ClearAndAdd();
            BuildAndUpgrade(data.GagaHouses, _gagaHouses);
            BuildAndUpgrade(data.Cleaners, _cleaners);
            BuildAndUpgrade(data.ClothMachines, _clothMachines);
        }
    }

    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath
          + path))
        {
            File.Delete(Application.persistentDataPath
              + path);
            data.Money = 0;
            data.CleanedFluff = 0;
            data.UncleanedFluff = 0;
            data.Item = 0;

            data.GagaHouses = new int[0];
            data.Cleaners = new int[0];
            data.ClothMachines = new int[0];


        }
    }

    private int[] SaveDataGrades(GameObject[] menus)
    {
        int [] dataArray = new int[menus.Length];

        for (int i = 0; i < menus.Length; i++)
        {
            BuildMenu buildMenu = menus[i].GetComponent<BuildTrigger>().GetBuildMenu();
            if (buildMenu.GetConstruction() != null)
            {
                dataArray[i] = buildMenu.GetConstruction().GetCurrentGrade();
            }
        }
        return dataArray;
    }
    private void ClearAndAdd()
    {
        _playerInventory.GetAllItems()[0].ClearItems();
        _playerInventory.GetAllItems()[1].ClearItems();
        _playerInventory.GetAllItems()[2].ClearItems();
        _playerInventory.GetAllItems()[3].ClearItems();

        _playerInventory.GetAllItems()[0].AddItems(data.Money);
        _playerInventory.GetAllItems()[1].AddItems(data.CleanedFluff);
        _playerInventory.GetAllItems()[2].AddItems(data.UncleanedFluff);
        _playerInventory.GetAllItems()[3].AddItems(data.Item);
    }

    private void BuildAndUpgrade(int[] dataArray, GameObject[] menus)
    {
        for (int i = 0; i < dataArray.Length; i++)
        {
            BuildMenu buildMenu = menus[i].GetComponent<BuildTrigger>().GetBuildMenu();
            switch (dataArray[i])
            {
                
                default: continue;
                case 1:
                    menus[i].GetComponent<BuildTrigger>().SetConstruction();
                    buildMenu.Build();
                    continue;
                case 2:
                    menus[i].GetComponent<BuildTrigger>().SetConstruction();
                    buildMenu.Build();
                    buildMenu.Upgrade();
                    continue;
                case 3:
                    menus[i].GetComponent<BuildTrigger>().SetConstruction();
                    buildMenu.Build();
                    buildMenu.Upgrade();
                    buildMenu.Upgrade();
                    continue;
            }

        }
    }
}
