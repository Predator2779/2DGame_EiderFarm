using System;
using System.Linq;
using General;
using TriggerScripts;
using UnityEngine;
using EventHandler = General.EventHandler;

namespace Building
{
    public class BuildingsPull : MonoBehaviour
    {
        public BuildTrigger[] _gagaHouses;
        public BuildTrigger[] _cleaners;
        public BuildTrigger[] _clothMachines;
        public BuildTrigger[] _storages;

        private static BuildingsPull _instance;

        private void Start()
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
                    AddRemove(commandType, buildTrigger, ref _gagaHouses);
                    break;
                case GlobalTypes.TypeBuildings.FluffCleaner:
                    AddRemove(commandType, buildTrigger, ref _cleaners);
                    break;
                case GlobalTypes.TypeBuildings.ClothMachine:
                    AddRemove(commandType, buildTrigger, ref _clothMachines);
                    break;
                case GlobalTypes.TypeBuildings.Storage:
                    AddRemove(commandType, buildTrigger, ref _storages);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildType), buildType, null);
            }
        }
        
        private void AddRemove(
                CommandType type,
                BuildTrigger building,
                ref BuildTrigger[] buildings)
        {
            bool contains = buildings.Contains(building);
            
            switch (type)
            {
                case CommandType.Add:
                    if (!contains) buildings.ToList().Add(building);
                    break;
                case CommandType.Remove:
                    if (contains) buildings.ToList().Remove(building);
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