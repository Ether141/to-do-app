using ToDoAppClient.Models;
using System.Collections.Generic;
using System.Linq;

namespace ToDoAppClient.Core
{
    public class ToDoListsManager
    {
        private readonly List<ToDoModel> allLists = new List<ToDoModel>();

        public ToDoModel[] AllLists => allLists.ToArray();

        public void DownloadLists()
        {
            ToDoModel model = new ToDoModel(0, "Lista zakupów - 9.06.2022");
            allLists.Add(model);
            model.ToDoEntries.Add(new ToDoEntry("Jabłka 3x", false));
            model.ToDoEntries.Add(new ToDoEntry("Mleko 2l", true));
            model.ToDoEntries.Add(new ToDoEntry("Mleko 2l", false));
            model.ToDoEntries.Add(new ToDoEntry("Chleb", true));
            model.ToDoEntries.Add(new ToDoEntry("Cebula 1kg", true));
            model.ToDoEntries.Add(new ToDoEntry("Papryka 3x", true));
            model.ToDoEntries.Add(new ToDoEntry("Jabłka 3x", false));
            model.ToDoEntries.Add(new ToDoEntry("Mleko 2l", true));
            model.ToDoEntries.Add(new ToDoEntry("Mleko 2l", false));
            model.ToDoEntries.Add(new ToDoEntry("Chleb", true));
            model.ToDoEntries.Add(new ToDoEntry("Cebula 1kg", true));
            model.ToDoEntries.Add(new ToDoEntry("Papryka 3x", true));
            model.ToDoEntries.Add(new ToDoEntry("Jabłka 3x", false));
            model.ToDoEntries.Add(new ToDoEntry("Mleko 2l", true));
            model.ToDoEntries.Add(new ToDoEntry("Mleko 2l", false));
            model.ToDoEntries.Add(new ToDoEntry("Chleb", true));
            model.ToDoEntries.Add(new ToDoEntry("Cebula 1kg", true));
            model.ToDoEntries.Add(new ToDoEntry("Papryka 3x", true));
            model.ToDoEntries.Add(new ToDoEntry("1", true));
            model.ToDoEntries.Add(new ToDoEntry("2", true));
            model.ToDoEntries.Add(new ToDoEntry("3", true));
            model.ToDoEntries.Add(new ToDoEntry("4", true));

            model = new ToDoModel(1, "Zadania dla grafików");
            allLists.Add(model);
            model.ToDoEntries.Add(new ToDoEntry("Zrobić jakieś tam przyciski", true));
            model.ToDoEntries.Add(new ToDoEntry("Zrobić coś tam jeszcze", false));
            model.ToDoEntries.Add(new ToDoEntry("I dodatkowo to", true));

            model = new ToDoModel(2, "Zadania dla programistów");
            allLists.Add(model);

            model = new ToDoModel(3, "Zadania dla sound designerów");
            allLists.Add(model);

            model = new ToDoModel(4, "Zadania dla level designerów");
            allLists.Add(model);

            model = new ToDoModel(5, "Zadania dla quest designerów");
            allLists.Add(model);
        }

        public void ClearLists() => allLists.Clear();

        public void AddToDoList(ToDoModel list)
        {
            if (ContainsListWithId(list.Id))
                return;

            allLists.Add(list);
        }

        public void UpdateList(int listId, ToDoModel updatedList)
        {
            for (int i = 0; i < allLists.Count; i++)
            {
                if (allLists[i].Id == listId)
                {
                    allLists[i].Name = updatedList.Name;
                    allLists[i].ToDoEntries = updatedList.ToDoEntries;
                    break;
                }
            }
        }

        public void RemoveList(int listId)
        {
            if (!ContainsListWithId(listId))
                return;
            allLists.RemoveAll(x => x.Id == listId);
        }

        public bool ContainsListWithId(int id) => allLists.Any(list => list.Id == id);
    }
}
