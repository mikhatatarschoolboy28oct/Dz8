using System;
using System.Collections.Generic;

class Person
{
    public string Name;

    public Person(string name)
    {
        Name = name;
    }
}

class Report
{
    public string Text;
    public DateTime Date;
    public Person Executor;

    public Report(string text, Person executor)
    {
        Text = text;
        Date = DateTime.Now;
        Executor = executor;
    }
}

class WorkTask
{
    public string Description;
    public DateTime Deadline;
    public Person Initiator;
    public Person Executor;
    public string Status;
    public Report Report;

    public WorkTask(string description, DateTime deadline, Person initiator, Person executor)
    {
        Description = description;
        Deadline = deadline;
        Initiator = initiator;
        Executor = executor;
        Status = "Назначена";
        Report = null;
    }

    public void Take()
    {
        if (Status == "Назначена" && Executor != null)
        {
            Status = "В работе";
        }
    }

    public void DelegateTo(Person person)
    {
        if (Status == "Назначена")
        {
            Executor = person;
        }
    }

    public void Reject()
    {
        if (Status == "Назначена")
        {
            Executor = null;
        }
    }

    public void Reassign(Person person)
    {
        if (Executor == null)
        {
            Executor = person;
            Status = "Назначена";
        }
    }

    public void SendReport(string text)
    {
        if (Status == "В работе" && Report == null)
        {
            Report = new Report(text, Executor);
            Status = "На проверке";
        }
    }

    public void Approve()
    {
        if (Status == "На проверке")
        {
            Status = "Выполнена";
        }
    }

    public void SendBack()
    {
        if (Status == "На проверке")
        {
            Report = null;
            Status = "В работе";
        }
    }
}

class Project
{
    public string Description;
    public DateTime Deadline;
    public Person Customer;
    public Person TeamLead;
    public string Status;
    public List<WorkTask> Tasks = new List<WorkTask>();

    public Project(string description, DateTime deadline, Person customer, Person teamLead)
    {
        Description = description;
        Deadline = deadline;
        Customer = customer;
        TeamLead = teamLead;
        Status = "Проект";
    }

    public bool AddTask(Person who, string text, Person executor)
    {
        if (who != TeamLead || Status != "Проект")
        {
            return false;
        }

        WorkTask task = new WorkTask(text, Deadline, who, executor);
        Tasks.Add(task);
        return true;
    }

    public void Start()
    {
        if (Status == "Проект")
        {
            Status = "Исполнение";
        }
    }

    public void TryClose()
    {
        foreach (WorkTask task in Tasks)
        {
            if (task.Status != "Выполнена")
            {
                return;
            }
        }
        Status = "Закрыт";
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        string[] names = { "Тимур", "Рашид", "Ильхам", "Оркадий", "Володя",
                           "Ильшат", "Сергей", "Ляйсан", "Марат", "Дина" };
        Person[] team = new Person[10];
        for (int i = 0; i < 10; i++)
        {
            team[i] = new Person(names[i]);
        }

        Person customer = new Person("Заказчик");
        Person lead = team[0];

        Project project = new Project("Автоматизация бухгалтерии", DateTime.Now.AddDays(30), customer, lead);
        Console.WriteLine("Проект создан, статус: " + project.Status);

        for (int i = 0; i < 10; i++)
        {
            project.AddTask(lead, "Задача " + (i + 1), team[i]);
        }
        Console.WriteLine("Создано задач: " + project.Tasks.Count);

        Console.WriteLine("Чужой человек создает задачу: " + project.AddTask(team[1], "Левая", team[2]));

        project.Start();
        Console.WriteLine("Статус проекта: " + project.Status);
        Console.WriteLine("Создать задачу после старта: " + project.AddTask(lead, "Лишняя", team[1]));

        project.Tasks[1].DelegateTo(team[2]);
        Console.WriteLine("Задача 2 делегирована: " + project.Tasks[1].Executor.Name);

        project.Tasks[2].Reject();
        if (project.Tasks[2].Executor == null)
        {
            Console.WriteLine("Задача 3 отклонена, исполнителя нет");
        }
        project.Tasks[2].Reassign(team[4]);
        Console.WriteLine("Задача 3 назначена заново: " + project.Tasks[2].Executor.Name);

        for (int i = 0; i < project.Tasks.Count; i++)
        {
            WorkTask task = project.Tasks[i];

            task.Take();
            task.SendReport("Отчет по задаче " + (i + 1));

            if (i == 3)
            {
                task.SendBack();
                Console.WriteLine("Задача 4 возвращена на доработку, статус: " + task.Status);
                task.SendReport("Исправленный отчет по задаче 4");
            }

            task.Approve();
        }

        project.TryClose();

        Console.WriteLine();
        foreach (WorkTask task in project.Tasks)
        {
            Console.WriteLine(task.Description + " | " + task.Executor.Name + " | " + task.Status + " | " + task.Report.Text);
        }

        Console.WriteLine();
        Console.WriteLine("Статус проекта: " + project.Status);
    }
}