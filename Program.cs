// ImplicitUsings Enabled
using System.Text.Json;

namespace TaskManagerCli
{
    class Program
    {
        private const string FilePath = "tasks.json";
        private static List<TaskItem> tasks = new();

        static void Main(string[] args)
        {
            LoadTasks();
            Menu();
        }

        static void Menu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Gerenciador de tarefas:");
                Console.WriteLine("1 - Criar tarefa");
                Console.WriteLine("2 - Listar tarefas");
                Console.WriteLine("3 - Marcar tarefa como concluída");
                Console.WriteLine("4 - Remover tarefa");
                Console.WriteLine("0 - Sair");
                Console.WriteLine();
                Console.Write("Escolha uma opção: ");

                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        CreateTask();
                        break;

                    case "2":
                        ListTasks();
                        break;

                    case "3":
                        CompleteTask();
                        break;

                    case "4":
                        RemoveTask();
                        break;

                    case "0":
                        SaveTasks();
                        return;

                    default:
                        ShowMessage("Opção inválida.");
                        break;
                }
            }
        }

        static void CreateTask()
        {
            Console.Clear();
            Console.WriteLine("Criar tarefa:");
            Console.Write("Digite o título da tarefa: ");

            string? title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                ShowMessage("O título da tarefa não pode ser vazio.");
                return;
            }

            int newId = tasks.Count == 0 ? 1 : tasks.Max(t => t.Id) + 1;

            tasks.Add(new TaskItem
            {
                Id = newId,
                Title = title.Trim(),
                IsCompleted = false
            });

            SaveTasks();
            ShowMessage("Tarefa criada com sucesso.");
        }

        static void ListTasks()
        {
            Console.Clear();
            Console.WriteLine("Lista de tarefas:");

            if (tasks.Count == 0)
            {
                Console.WriteLine("Nenhuma tarefa cadastrada.");
                Pause();
                return;
            }

            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "[X]" : "[ ]";
                Console.WriteLine($"{task.Id} - {status} {task.Title}");
            }

            Pause();
        }

        static void CompleteTask()
        {
            Console.Clear();
            Console.WriteLine("Marcar como concluída:");

            if (tasks.Count == 0)
            {
                ShowMessage("Não há tarefas cadastradas.");
                return;
            }

            ShowTasksInline();

            Console.WriteLine();
            Console.Write("Digite o ID da tarefa: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                ShowMessage("ID inválido.");
                return;
            }

            TaskItem? task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                ShowMessage("Tarefa não encontrada.");
                return;
            }

            if (task.IsCompleted)
            {
                ShowMessage("Essa tarefa já está concluída.");
                return;
            }

            task.IsCompleted = true;
            SaveTasks();
            ShowMessage("Tarefa marcada como concluída.");
        }

        static void RemoveTask()
        {
            Console.Clear();
            Console.WriteLine("Remover tarefa:");

            if (tasks.Count == 0)
            {
                ShowMessage("Não há tarefas cadastradas.");
                return;
            }

            ShowTasksInline();

            Console.WriteLine();
            Console.Write("Digite o ID da tarefa que deseja remover: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                ShowMessage("ID inválido.");
                return;
            }

            TaskItem? task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                ShowMessage("Tarefa não encontrada.");
                return;
            }

            tasks.Remove(task);
            SaveTasks();
            ShowMessage("Tarefa removida com sucesso.");
        }

        static void LoadTasks()
        {
            if (!File.Exists(FilePath))
            {
                tasks = new List<TaskItem>();
                return;
            }

            string json = File.ReadAllText(FilePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                tasks = new List<TaskItem>();
                return;
            }

            tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }

        static void SaveTasks()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(tasks, options);
            File.WriteAllText(FilePath, json);
        }

        static void ShowTasksInline()
        {
            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "[X]" : "[ ]";
                Console.WriteLine($"{task.Id} - {status} {task.Title}");
            }
        }

        static void ShowMessage(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Pause();
        }

        static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }

    class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}