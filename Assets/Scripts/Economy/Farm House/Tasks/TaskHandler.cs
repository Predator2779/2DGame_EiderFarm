using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EventHandler = General.EventHandler;

namespace Economy.Farm_House
{
    public class TaskHandler : DisplayMenu
    {
        [SerializeField] private Task[] _tasks;
        [SerializeField] private TaskCell _taskCellPrefab;

        private void Awake()
        {
            EventHandler.OnTaskStageChanged.AddListener(Draw);
        }

        public override void Draw()
        {
            ClearContent();

            if (_isHouseMenu)
            {
                DrawTasks(GetTasks(TaskStage.NotStarted));
                return;
            }

            DrawTasks(GetTasks(TaskStage.Progressing));
        }

        private Task[] GetTasks(TaskStage stage) => _tasks.Where(task => task.GetStage() == stage).ToArray();

        private void DrawTasks(Task[] tasks)
        {
            foreach (var task in tasks)
                SetCell(Instantiate(_taskCellPrefab, _content), task);
        }

        private void SetCell(TaskCell cell, Task task) => cell.SetCell(task);
    }
}