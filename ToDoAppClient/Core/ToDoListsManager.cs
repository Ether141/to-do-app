using ToDoAppClient.Models;
using System.Collections.Generic;

namespace ToDoAppClient.Core
{
    public class ToDoListsManager
    {
        private readonly List<ToDoModel> allLists = new List<ToDoModel>();

        public ToDoModel[] AllLists => allLists.ToArray();

        public void DownloadLists()
        {
            ToDoModel model = new ToDoModel("Lista zakupów - 9.06.2022");
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

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);
            model.ToDoEntries.Add(new ToDoEntry("Zrobić jakieś tam przyciski", true));
            model.ToDoEntries.Add(new ToDoEntry("Zrobić coś tam jeszcze", false));
            model.ToDoEntries.Add(new ToDoEntry("I dodatkowo to", true));

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);

            model = new ToDoModel("Zadania dla grafików");
            allLists.Add(model);
        }

        public void ClearLists() => allLists.Clear();
    }
}
