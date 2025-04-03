using Rookies_ASP.NETCoreAPI.Common;
using System.Globalization;
using CsvHelper;
using Task = Rookies_ASP.NETCoreAPI.Infrastructure.Models.Task;


namespace Rookies_ASP.NETCoreAPI.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        public static IEnumerable<Task> _tasks = new List<Task>();
        public TaskRepository()
        {
            _tasks = InitialTaskData();
        }
        public IEnumerable<Task> GetTasks()
        {
            return _tasks;
        }
        public Task? GetTask(Guid id)
        {
            return _tasks.FirstOrDefault(task => task.Id == id);
        }
        public int Add(Task task)
        {
            var taskList = _tasks.ToList();
            taskList.Add(task);
            if (taskList.Count > _tasks.Count())
            {
                _tasks = taskList;
                SaveTasksToCsv(_tasks);
                return ConstantsStatus.Success;
            }
            else return ConstantsStatus.Failed;
        }
        public int Update(Guid id, Task task)
        {
            var updateTask = _tasks.FirstOrDefault(task => task.Id == id);
            if (updateTask != null)
            {
                updateTask.Title = task.Title;
                updateTask.Status = task.Status;
                SaveTasksToCsv(_tasks);
                return ConstantsStatus.Success;
            }
            else return ConstantsStatus.Failed;
        }
        public int Delete(Guid id)
        {
            var deleteTask = _tasks.FirstOrDefault(task => task.Id == id);
            if (deleteTask != null)
            {
                var taskList = _tasks.ToList();
                if (taskList.Remove(deleteTask))
                {
                    _tasks = taskList;
                    SaveTasksToCsv(_tasks);
                    return ConstantsStatus.Success;
                }
                else return ConstantsStatus.Failed;
            }
            else return ConstantsStatus.Failed;
        }         
        public int BulkAdd(List<Task> tasks)
        {
            var taskList = _tasks.ToList();
            taskList.AddRange(tasks);
            if (taskList.Count > _tasks.Count())
            {
                _tasks = taskList;
                SaveTasksToCsv(_tasks);
                return ConstantsStatus.Success;
            }
            else return ConstantsStatus.Failed;
        }
        public int BulkDelete(List<Guid> ids)
        {
            var taskList = _tasks.ToList();
            int numberOfRecordsRemoved = taskList.RemoveAll(task => ids.Contains(task.Id));
            if (numberOfRecordsRemoved == 0)
                return ConstantsStatusBulkDelete.NothingRemoved;
            else
            {
                _tasks = taskList;
                SaveTasksToCsv(_tasks);
                if (numberOfRecordsRemoved == ids.Count())
                    return ConstantsStatusBulkDelete.AllRemoved;
                else
                    return ConstantsStatusBulkDelete.OnlyValidRemoved;
            }
        }
        private IEnumerable<Task> InitialTaskData()
        {
            #region Data Setting
            //IEnumerable<Task> tasks = new List<Task>
            //{
            //    new Task
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = "Minh Ngo",
            //        Status = true,
            //    },
            //    new Task
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = "Anh Hieu",
            //        Status = false,
            //    },
            //    new Task
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = "Vu Duy",
            //        Status = true,
            //    },
            //    new Task
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = "Van Viet",
            //        Status = false,
            //    },
            //    new Task
            //    {
            //        Id = Guid.NewGuid(),
            //        Title = "Thanh Hung",
            //        Status = false,
            //    },
            //};
            //return tasks;
            #endregion
            try
            {
                using var reader = new StreamReader("./Data/DataApi.csv");
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var tasks = csv.GetRecords<Task>().ToList();
                return tasks;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data from CSV: {ex.Message}");
                return new List<Task>();
            }
        }
        private void SaveTasksToCsv(IEnumerable<Task> tasksToSave)
        {
            try
            {
                using var writer = new StreamWriter("./Data/DataApi.csv");
                using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csvWriter.WriteRecords(tasksToSave);
                Console.WriteLine($"Successfully saved {tasksToSave.Count()} tasks to CSV.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tasks to CSV: {ex.Message}");
            }
        }
    }
}
