namespace ToDoListNew;

class Program
{
    static void Main(string[] args)
    {
        // var myStruct = new TodoItemStruct { Id = 1, Title = "Оригинал" };
        // ModifyStruct(myStruct);
        // Console.WriteLine(myStruct.Title);

        // var log1 = new TaskLogEntry(1, "Добавить", DateTime.Now);
        // var log2 = new TaskLogEntry(1, "Добавить", log1.Timestamp);
        // Console.WriteLine(log1 == log2);

        List<TodoItem> tasks = new List<TodoItem>();
        List<TaskLogEntry> logs = new List<TaskLogEntry>();
        int currentId = 0;
        bool isRunning = true;
        while (isRunning)
        {
            ShowMenu();
            if (int.TryParse(Console.ReadLine(), out int menuItem))
            {
                switch (menuItem)
                {
                    case 1:
                        if (AddTask(tasks, logs, currentId))
                        {
                            currentId++;
                        }
                        break;
                    case 2:
                        ShowTasks(tasks);
                        break;
                    case 3:
                        MarkTask(tasks);
                        break;
                    case 4:
                        DeleteTask(tasks, logs);
                        break;
                    case 5:
                        ReopenTask(tasks);
                        break;
                    case 6:
                        RenameTask(tasks, logs);
                        break;
                    case 7:
                        ShowReminders(tasks);
                        break;
                    case 8:
                        CancelTask(tasks);
                        break;
                    case 9:
                        ShowLogs(logs);
                        break;
                    case 10:
                        Console.WriteLine("До свидания");
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Такой пункт меню отсутствует");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Введите число");
            }
        }
    }
    static void ShowMenu()
    {
        Console.WriteLine("Выберите пункт меню");
        Console.WriteLine("");
        Console.WriteLine("1. Добавить задачу");
        Console.WriteLine("2. Показать задачи");
        Console.WriteLine("3. Отметить задачу выполненной");
        Console.WriteLine("4. Удалить задачу");
        Console.WriteLine("5. Вернуть в работу");
        Console.WriteLine("6. Переименовать задачу");
        Console.WriteLine("7. Напоминания");
        Console.WriteLine("8. Отменить задачу");
        Console.WriteLine("9. Показать логи");
        Console.WriteLine("10. Выйти");
        Console.WriteLine("");
        Console.WriteLine("");
    }
    static bool AddTask(List<TodoItem> tasks, List<TaskLogEntry> logs, int currentId)
    {
        Console.WriteLine("Введите название задачи");
        string input = Console.ReadLine() ?? "";
        Console.WriteLine("Выберите тип задачи");
        Console.WriteLine("1. обычная");
        Console.WriteLine("2. с дедлайном");
        if (int.TryParse(Console.ReadLine(), out int typeTask))
        {
            switch (typeTask)
            {
                case 1:
                    try
                    {
                        TodoItem task = new TodoItem(currentId, input);
                        tasks.Add(task);
                        TaskLogEntry log = new TaskLogEntry(currentId, "Добавить задачу", DateTime.Now);
                        logs.Add(log);
                        Console.WriteLine("Задача добавлена!");
                        return true;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                        return false;
                    }
                case 2:
                    try
                    {
                        Console.WriteLine("Введите дату дедлайна в формате dd.mm.yyyy");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                        {
                            DeadlineTask task = new DeadlineTask(currentId, input, date);
                            tasks.Add(task);
                            TaskLogEntry log = new TaskLogEntry(currentId, "Добавить задачу с дедлайном", DateTime.Now);
                            logs.Add(log);
                            Console.WriteLine("Задача с дедлайном добавлена!");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Дата не распознана, постарайтесь получше");
                            return false;
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                        return false;
                    }
                default:
                    Console.WriteLine("Такой тип задачи не существует в программе");
                    return false;
            }
        }
        else
        {
            Console.WriteLine("Введите число");
            return false;
        }
    }
    static void ShowTasks(List<TodoItem> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список пуст");
        }
        else
        {
            foreach (TodoItem task in tasks)
            {
                Console.WriteLine(task.Describe());
            }
        }
    }
    static void MarkTask(List<TodoItem> tasks)
    {
        Console.WriteLine("Введите Id задачи");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            TodoItem? task = FindTaskById(tasks, id);
            if (task == null)
            {
                Console.WriteLine("Задача с таким Id не найдена");
                return;
            }
            task.MarkDone();
            Console.WriteLine("Задача отмечена выполненной");

        }
        else
        {
            Console.WriteLine("Ошибка: введите число");
        }
    }
    static void DeleteTask(List<TodoItem> tasks, List<TaskLogEntry> logs)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список пуст");
        }
        else
        {
            Console.WriteLine("Введите Id задачи");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                TodoItem? task = FindTaskById(tasks, id);
                if (task == null)
                {
                    Console.WriteLine("Задача с таким Id не найдена");
                    return;
                }
                tasks.Remove(task);
                TaskLogEntry log = new TaskLogEntry(task.Id, "Удалить задачу", DateTime.Now);
                logs.Add(log);
                Console.WriteLine("Задача удалена");

            }
            else
            {
                Console.WriteLine("Ошибка: введите число");
            }
        }
    }
    static void ReopenTask(List<TodoItem> tasks)
    {
        Console.WriteLine("Введите Id задачи");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            TodoItem? task = FindTaskById(tasks, id);
            if (task == null)
            {
                Console.WriteLine("Задача с таким Id не найдена");
                return;
            }
            task.Reopen();
            Console.WriteLine("Задача возвращена в работу");
        }
        else
        {
            Console.WriteLine("Ошибка: введите число");
        }
    }
    static void RenameTask(List<TodoItem> tasks, List<TaskLogEntry> logs)
    {
        Console.WriteLine("Введите Id задачи для переименования");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            TodoItem? task = FindTaskById(tasks, id);
            if (task == null)
            {
                Console.WriteLine("Задача с таким Id не найдена");
                return;
            }
            try
            {
                Console.WriteLine("Введите новое название задачи");
                string input = Console.ReadLine() ?? "";
                task.Rename(input);
                TaskLogEntry log = new TaskLogEntry(task.Id, "Переименовать задачу", DateTime.Now);
                logs.Add(log);
                Console.WriteLine("Задача переименована");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Введите число");
        }
    }
    static void ShowReminders(List<TodoItem> tasks)
    {
        bool found = false;
        foreach (TodoItem task in tasks)
        {
            if (task is INotifiable notifiable)
            {
                Console.WriteLine(notifiable.GetReminder());
                found = true;
            }
        }
        if (!found) Console.WriteLine("Напоминаний нет");
    }
    static void CancelTask(List<TodoItem> tasks)
    {
        Console.WriteLine("Введите Id задачи");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            TodoItem? task = FindTaskById(tasks, id);
            if (task == null)
            {
                Console.WriteLine("Задача с таким Id не найдена");
                return;
            }
            task.Cancel();
            Console.WriteLine("Задача отменена");

        }
        else
        {
            Console.WriteLine("Ошибка: введите число");
        }
    }
    static void ShowLogs(List<TaskLogEntry> logs)
    {
        if (logs.Count == 0)
        {
            Console.WriteLine("Список пуст");
        }
        else
        {
            foreach (TaskLogEntry log in logs)
            {
                Console.WriteLine($"Id задачи: {log.TaskId}, Действие: {log.Action}, Время: {log.Timestamp}");
            }
        }
    }
    static TodoItem? FindTaskById(List<TodoItem> tasks, int id)
    {
        foreach (TodoItem task in tasks)
        {
            if (task.Id == id)
            {
                return task;
            }
        }
        return null;
    }
    static void ModifyStruct(TodoItemStruct item)
    {
        item.Title = "Изменено";
    }
}
class TodoItem
{
    public int Id { get; }
    public string Title { get; private set; }
    public TaskStatus Status { get; private set; }

    public TodoItem(int id, string title)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Id должен быть больше нуля!");
        }
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Название задачи не может быть пустым!");
        }
        Id = id;
        Title = title;
        Status = TaskStatus.New;
    }
    public void MarkDone()
    {
        Status = TaskStatus.Completed;
    }
    public void Reopen()
    {
        Status = TaskStatus.InProgress;
    }
    public void Cancel()
    {
        Status = TaskStatus.Cancelled;
    }
    public void Rename(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            throw new ArgumentException("Название новой задачи не может быть пустым!");
        }
        Title = newTitle;
    }
    public virtual string Describe()
    {
        string statusText = Status switch
        {
            TaskStatus.New => "[ ]",
            TaskStatus.InProgress => "[▶]",
            TaskStatus.Completed => "[✔]",
            TaskStatus.Cancelled => "[✖]",
            _ => "[?]"
        };
        return $"{Id}. {Title} {statusText}";
    }
}
class DeadlineTask : TodoItem, INotifiable
{
    public DateTime DueDate { get; }

    public DeadlineTask(int id, string title, DateTime dueDate) : base(id, title)
    {
        DueDate = dueDate;
    }

    public override string Describe()
    {
        string parentDescription = base.Describe();
        string overdue = (Status != TaskStatus.Completed && Status != TaskStatus.Cancelled && DueDate.Date < DateTime.Now.Date) ? " ПРОСРОЧЕНО!" : "";
        return $"{parentDescription} (до {DueDate:dd.MM.yyyy}){overdue}";
    }

    public string GetReminder()
    {
        if (Status == TaskStatus.Completed)
        {
            return $"Задача \"{Title}\" уже выполнена";
        }
        if (Status == TaskStatus.Cancelled)
        {
            return $"Задача \"{Title}\" отменена";
        }
        if (DueDate.Date < DateTime.Now.Date)
        {
            return $"Задача \"{Title}\" просрочена с {DueDate:dd.MM.yyyy}";
        }

        int days = (DueDate.Date - DateTime.Now.Date).Days;

        if (days == 0)
        {
            return $"Задача \"{Title}\" — дедлайн сегодня";
        }

        return $"До дедлайна задачи \"{Title}\" осталось {days} дн.";
    }
}
interface INotifiable
{
    string GetReminder();
}
enum TaskStatus
{
    New,
    InProgress,
    Completed,
    Cancelled
}
record TaskLogEntry(
    int TaskId,
    string Action,
    DateTime Timestamp
);
struct TodoItemStruct
{
    public int Id;
    public string Title;

}
