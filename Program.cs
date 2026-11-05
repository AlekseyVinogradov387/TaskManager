namespace ToDoListNew;

class Program
{
    static void Main(string[] args)
    {
        List<TodoItem> tasks = new List<TodoItem>();
        int currentId = 1;
        bool isRunning = true;
        while (isRunning)
        {
            ShowMenu();
            if (int.TryParse(Console.ReadLine(), out int menuItem))
            {
                switch (menuItem)
                {
                    case 1:
                        AddTask(tasks, currentId);
                        currentId++;
                        break;
                    case 2:
                        ShowTasks(tasks);
                        break;
                    case 3:
                        MarkTask(tasks);
                        break;
                    case 4:
                        DeleteTask(tasks);
                        break;
                    case 5:
                        ReopenTask(tasks);
                        break;
                    case 6:
                        RenameTask(tasks);
                        break;
                    case 7:
                        ShowReminders(tasks);
                        break;
                    case 8:
                        Console.WriteLine("Спасибо! Удачи Вам! И хорошего настроения!");
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
        Console.WriteLine("8. Выйти");
        Console.WriteLine("");
        Console.WriteLine("");
    }
    static void AddTask(List<TodoItem> tasks, int currentId)
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
                        Console.WriteLine("Задача добавлена!");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    break;
                case 2:
                    try
                    {
                        Console.WriteLine("Введите дату дедлайна в формате dd.mm.yyyy");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                        {
                            DeadlineTask task = new DeadlineTask(currentId, input, date);
                            tasks.Add(task);
                            Console.WriteLine("Задача с дедлайном добавлена!");
                        }
                        else
                        {
                            Console.WriteLine("Дата не распознана, постарайтесь получше");
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    break;
                default:
                    Console.WriteLine("Такой тип задачи не существует в программе");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Введите число");
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
    static void DeleteTask(List<TodoItem> tasks)
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
    static void RenameTask(List<TodoItem> tasks)
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
}

class TodoItem
{
    public int Id { get; }
    public string Title { get; private set; }
    public bool IsCompleted { get; private set; }

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
        IsCompleted = false;
    }

    public void MarkDone()
    {
        IsCompleted = true;
    }

    public void Reopen()
    {
        IsCompleted = false;
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
        string status = IsCompleted ? "[✔]" : "[ ]";
        return $"{Id}. {Title} {status}";
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
        string overdue = (!IsCompleted && DueDate.Date < DateTime.Now.Date) ? " ПРОСРОЧЕНО!" : "";
        return $"{parentDescription} (до {DueDate:dd.MM.yyyy}){overdue}";
    }

    public string GetReminder()
    {
        if (IsCompleted)
        {
            return $"Задача \"{Title}\" уже выполнена.";
        }
        if (DueDate.Date < DateTime.Now.Date)
        {
            return $"Задача \"{Title}\" просрочена с {DueDate:dd.MM.yyyy}!";
        }

        int days = (DueDate.Date - DateTime.Now.Date).Days;

        if (days == 0)
        {
            return $"Задача \"{Title}\" — дедлайн сегодня!";
        }

        return $"До дедлайна задачи \"{Title}\" осталось {days} дн.";
    }
}
interface INotifiable
{
    string GetReminder();
}
