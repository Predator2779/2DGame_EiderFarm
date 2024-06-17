using Building;
using Economy;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Characters;
using General;
using TriggerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Economy.Farm_House;

[RequireComponent(typeof(Menu))]
public class SaveSerial : MonoBehaviour
{ 
    public bool IsHasSaves;

    [SerializeField] private TaskHandler _taskHandler;
    [SerializeField] private Inventory _playerInventory;
    [SerializeField] private Item[] _itemTypes;
    [SerializeField] private Menu _menu;
    [SerializeField] private Sprite[] _sprites;

    [Space][Header("Save Data")]
    [SerializeField] private Employee[] _employees;

    private BuildTrigger[] _gagaHouses;
    private BuildTrigger[] _cleaners;
    private BuildTrigger[] _clothMachines;
    private BuildTrigger[] _storages;
    private bool[] _flags;
    private List<ItemBunch> _items;
    private string _path = "/dataSaveFile.dat";
    public SaveData data = new();

    public void Initialize()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            LoadBool();
            if (!IsHasSaves) _menu.IfNotSaves();

            return;
        }

        if (_playerInventory != null) GetItems();

        if (_menu.IsNewGame()) ResetData();
        else LoadGame();
    }

    private void SaveBool(bool value) => data.isSaves = value;
    private void GetItems() => _items = _playerInventory.GetAllItems();

    public void SetBuildings(BuildingsPull pull)
    {
        _gagaHouses = pull.GagaHouses;
        _cleaners = pull.Cleaners;
        _clothMachines = pull.ClothMachines;
        _storages = pull.Storages;
    }

    private void SaveItems()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            switch (_items[i].GetItemName())
            {
                case GlobalConstants.Money: data.Money = _items[i].GetCount(); break;
                case GlobalConstants.CleanedFluff: data.CleanedFluff = _items[i].GetCount(); break;
                case GlobalConstants.UncleanedFluff: data.UncleanedFluff = _items[i].GetCount(); break;
                case GlobalConstants.Cloth: data.Cloth = _items[i].GetCount(); break;
                case GlobalConstants.Flag: data.Flag = _items[i].GetCount(); break;
            }
        }
    }

    private void SaveEmployees()
    {
        data.Employees = new bool[_employees.Length];
        for (int i = 0; i < _employees.Length; i++)
        {
            if (_employees[i].GetHired())
                data.Employees[i] = true;
        }
    }

    private void LoadEmployees()
    {
        for (int i = 0; i < _employees.Length; i++)
        {
            if (data.Employees[i])
                _employees[i].SpawnEmployee(GlobalConstants.PersonalSpawnPoint);
        }
    }

    private void LoadItems()
    {
        if (data.Money != 0)
            AddItems(GlobalConstants.Money, data.Money);
        if (data.CleanedFluff != 0)
            AddItems(GlobalConstants.CleanedFluff, data.CleanedFluff);
        if (data.UncleanedFluff != 0)
            AddItems(GlobalConstants.UncleanedFluff, data.UncleanedFluff);
        if (data.Cloth != 0)
            AddItems(GlobalConstants.Cloth, data.Cloth);
        if (data.Flag != 0)
            AddItems(GlobalConstants.Flag, data.Flag);
    }

    private void AddItems(string name, int count)
    {
        var item = _itemTypes.FirstOrDefault(item => item.GetName() == name);
        _playerInventory.AddItems(item, count, true);
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;

        file = !File.Exists(Application.persistentDataPath + _path) ?
                File.Create(Application.persistentDataPath + _path) :
                File.Open(Application.persistentDataPath + _path, FileMode.Open);

        SaveItems();
        data.GagaHouses = SaveDataGrades(_gagaHouses);
        data.Cleaners = SaveDataGrades(_cleaners);
        data.ClothMachines = SaveDataGrades(_clothMachines);
        data.Storages = SaveDataGrades(_storages);
        SaveFlags();

        SaveEmployees();

        _menu.SetSaves(true);

        SaveBool(_menu.IsHasSaves());

        bf.Serialize(file, data);
        file.Close();
    }

    private void LoadGame()
    {
        if (!File.Exists(Application.persistentDataPath + _path)) return;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + _path, FileMode.Open);

        data = (SaveData)bf.Deserialize(file);
        file.Close();

        ClearAndAdd();
        LoadFlags();

        LoadEmployees();

        BuildAndUpgrade(data.GagaHouses, _gagaHouses);
        BuildAndUpgrade(data.Cleaners, _cleaners);
        BuildAndUpgrade(data.ClothMachines, _clothMachines);
        BuildAndUpgrade(data.Storages, _storages);
    }

    public void LoadBool()
    {
        if (!File.Exists(Application.persistentDataPath + _path)) return;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + _path, FileMode.Open);

        data = (SaveData)bf.Deserialize(file);
        file.Close();

        IsHasSaves = data.isSaves;
    }

    private void ResetData()
    {
        _taskHandler.ResetAllTasks();

        if (!File.Exists(Application.persistentDataPath + _path)) return;

        File.Delete(Application.persistentDataPath + _path);

        data.Money = 0;
        data.CleanedFluff = 0;
        data.UncleanedFluff = 0;
        data.Cloth = 0;
        data.Flag = 0;

        data.Employees = new bool[0];
        data.GagaHouses = new int[0];
        data.Cleaners = new int[0];
        data.ClothMachines = new int[0];
        data.Storages = new int[0];

        data.Flags = new bool[0];
        data.flagSprites = new int[0];

        _menu.SetSaves(false);
        SaveBool(false);
    }

    private int[] SaveDataGrades(BuildTrigger[] menus)
    {
        int[] dataArray = new int[menus.Length];

        for (int i = 0; i < menus.Length; i++)
        {
            BuildMenu buildMenu = menus[i].GetBuildMenu();

            if (buildMenu.GetBuilding() != null)
                dataArray[i] = buildMenu.GetBuilding().GetCurrentGrade();
        }

        return dataArray;
    }

    private void SaveFlags()
    {
        data.Flags = new bool[_gagaHouses.Length];
        data.flagSprites = new int[data.Flags.Length];
        
        for (int i = 0; i < _gagaHouses.Length; i++)
            if (_gagaHouses[i].gameObject.GetComponent<Flag>().isFlagAdded)
            {
                data.Flags[i] = true;

                for (int j = 0; j < _sprites.Length; j++)
                    if (_sprites[j] == _gagaHouses[i].gameObject.GetComponent<Flag>().GetSprite())
                        data.flagSprites[i] = j + 1;
            }
    }

    private void LoadFlags()
    {
        for (int i = 0; i < _gagaHouses.Length; i++)
            if (data.Flags[i])
                _gagaHouses[i].gameObject.GetComponent<Flag>().isFlagAdded = true;
        
        SetFlags(_gagaHouses);
    }

    private void ClearAndAdd()
    {
        for (int i = 0; i < _playerInventory.GetAllItems().Count; i++)
            _playerInventory.GetAllItems()[i].ClearItems();

        LoadItems();
    }

    private void BuildAndUpgrade(int[] dataArray, BuildTrigger[] menus)
    {
        for (int i = 0; i < dataArray.Length; i++)
        {
            menus[i].Initialize();
            BuildMenu buildMenu = menus[i].GetBuildMenu();

            switch (dataArray[i])
            {
                default: continue;
                case 1:
                    menus[i].SetConstruction();
                    buildMenu.Build(true);
                    continue;
                case 2:
                    menus[i].SetConstruction();
                    buildMenu.Build(true);
                    buildMenu.Upgrade(true);
                    continue;
                case 3:
                    menus[i].SetConstruction();
                    buildMenu.Build(true);
                    buildMenu.Upgrade(true);
                    buildMenu.Upgrade(true);
                    continue;
            }
        }
    }

    private void SetFlags(BuildTrigger[] gagaHousesMenus)
    {
        for (int i = 0; i < _gagaHouses.Length; i++)
        {
            if (gagaHousesMenus[i].gameObject.GetComponent<Flag>().isFlagAdded)
                gagaHousesMenus[i].gameObject.GetComponent<Flag>().AddFlag(true);

            if (!data.Flags[i]) continue;
            for (int j = 0; j < _sprites.Length; j++)
                if (data.flagSprites[i] == j + 1)
                    gagaHousesMenus[i].gameObject.GetComponent<Flag>().SetSprite(_sprites[j]);
        }
    }

    public GameObject[] GetGagaHouses()
    {
        GameObject[] gb = new GameObject[_gagaHouses.Length];
        
        for (int i = 0; i < _gagaHouses.Length; i++)
            gb[i] = _gagaHouses[i].gameObject;
        
        return gb;
    }

    public void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            SaveGame();
    }
}