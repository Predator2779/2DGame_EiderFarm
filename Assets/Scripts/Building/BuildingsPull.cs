using System;
using System.Collections.Generic;
using System.Linq;
using General;
using TriggerScripts;
using UnityEngine;
using EventHandler = General.EventHandler;

namespace Building
{
    public class BuildingsPull : MonoBehaviour
    {
        [SerializeField] private BuildTrigger[] _gagaHouses;
        [SerializeField] private BuildTrigger[] _cleaners;
        [SerializeField] private BuildTrigger[] _clothMachines;
        [SerializeField] private BuildTrigger[] _storages;

        public BuildTrigger[] GagaHouses { get => _gagaHouses; }
        public BuildTrigger[] Cleaners { get => _cleaners; }
        public BuildTrigger[] ClothMachines { get => _clothMachines; }
        public BuildTrigger[] Storages { get => _storages; }
        
        
        private static BuildingsPull _instance;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance == this) Destroy(gameObject);
            
            EventHandler.OnAddedBuildPull.AddListener(AddBuilding);
            EventHandler.OnRemovedBuildPull.AddListener(RemoveBuilding);
        }

        private void AddBuilding(BuildTrigger buildTrigger, GlobalTypes.TypeBuildings type)
        {
            Handle(CommandType.Add, buildTrigger, type);
        }

        private void RemoveBuilding(BuildTrigger buildTrigger, GlobalTypes.TypeBuildings type)
        {
            Handle(CommandType.Remove, buildTrigger, type);
        }

        private void Handle(
                CommandType commandType,
                BuildTrigger buildTrigger,
                GlobalTypes.TypeBuildings buildType)
        {
            switch (buildType)
            {
                case GlobalTypes.TypeBuildings.GagaHouse:
                    ChangeArray(commandType, buildTrigger, ref _gagaHouses);
                    break;
                case GlobalTypes.TypeBuildings.FluffCleaner:
                    ChangeArray(commandType, buildTrigger, ref _cleaners);
                    break;
                case GlobalTypes.TypeBuildings.ClothMachine:
                    ChangeArray(commandType, buildTrigger, ref _clothMachines);
                    break;
                case GlobalTypes.TypeBuildings.Storage:
                    ChangeArray(commandType, buildTrigger, ref _storages);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildType), buildType, null);
            }
        }
        
        private void ChangeArray(
                CommandType type,
                BuildTrigger building,
                ref BuildTrigger[] buildings)
        {
            List<BuildTrigger> buildingsList = buildings.ToList();
            bool contains = buildings.Contains(building);
            
            switch (type)
            {
                case CommandType.Add:
                    if (!contains)
                    {
                        buildingsList.Add(building);
                        buildings = buildingsList.ToArray();
                    }
                    break;
                case CommandType.Remove:
                    if (contains)
                    {
                        buildingsList.Remove(building);
                        buildings = buildingsList.ToArray();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void OnApplicationQuit()
        {
            EventHandler.OnAddedBuildPull.RemoveListener(AddBuilding);
            EventHandler.OnRemovedBuildPull.RemoveListener(RemoveBuilding);
        }

        private enum CommandType
        {
            Add,
            Remove
        }
    }
}