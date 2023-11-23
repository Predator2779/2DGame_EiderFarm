using Building;
using Economy;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TriggerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSerial : MonoBehaviour
{
    [SerializeField] private Inventory _playerInventory;

    [SerializeField] private BuildMenu[] _gagaHouses;
    [SerializeField] private BuildMenu[] _cleaners;
    

    private string path = "/dataSaveFile.dat";

    SaveData data = new SaveData();

    private void Awake()
    {
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

        data.GagaHouses = new int[_gagaHouses.Length];
        
        for (int i = 0; i < _gagaHouses.Length; i++)
        {
            if (_gagaHouses[i].GetConstruction() != null)
            {
                data.GagaHouses[i] = _gagaHouses[i].GetConstruction().GetCurrentGrade();
            }
        }

        data.Cleaners = new int[_cleaners.Length];

        for (int i = 0; i < _cleaners.Length; i++)
        {
            if (_gagaHouses[i].GetConstruction() != null)
            {
                data.Cleaners[i] = _cleaners[i].GetConstruction().GetCurrentGrade();
            }
        }

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


            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
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

    private void BuildAndUpgrade(int[] dataArray, BuildMenu[] menus)
    {
        for (int i = 0; i < dataArray.Length; i++)
        {

            switch (dataArray[i])
            {
                default: continue;
                case 1:
                    menus[i].GetComponent<BuildTrigger>().SetPos();
                    menus[i].Build();
                    continue;
                case 2:
                    menus[i].GetComponent<BuildTrigger>().SetPos();
                    menus[i].Build();
                    menus[i].Upgrade();
                    continue;
                case 3:
                    menus[i].GetComponent<BuildTrigger>().SetPos();
                    menus[i].Build();
                    menus[i].Upgrade();
                    menus[i].Upgrade();
                    continue;
            }

        }
    }
}
