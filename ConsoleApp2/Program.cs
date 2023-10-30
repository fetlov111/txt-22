using System;
using System.IO;
using System.Xml.Serialization;
using System.Text.Json;

public class Figure
{
    public string Name { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }

    public void CreateNewFile()
    {
        Console.Write("Введите название фигуры: ");
        Name = Console.ReadLine();

        Console.Write("Введите ширину: ");
        if (double.TryParse(Console.ReadLine(), out double width))
        {
            Width = width;
        }
        else
        {
            Console.WriteLine("Ошибка при вводе ширины.");
            Environment.Exit(0);
        }

        Console.Write("Введите высоту: ");
        if (double.TryParse(Console.ReadLine(), out double height))
        {
            Height = height;
        }
        else
        {
            Console.WriteLine("Ошибка при вводе высоты.");
            Environment.Exit(0);
        }

        SaveFile();
    }

    public void LoadFile(string filePath)
    {
        string fileExtension = Path.GetExtension(filePath).ToLower();

        if (fileExtension == ".txt")
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 3)
            {
                Name = lines[0];
                if (double.TryParse(lines[1], out double width))
                {
                    Width = width;
                }
                if (double.TryParse(lines[2], out double height))
                {
                    Height = height;
                }
            }
        }
        else if (fileExtension == ".json")
        {
            string json = File.ReadAllText(filePath);
            Figure loadedFigure = JsonSerializer.Deserialize<Figure>(json);
            if (loadedFigure != null)
            {
                Name = loadedFigure.Name;
                Width = loadedFigure.Width;
                Height = loadedFigure.Height;
            }
        }
        else if (fileExtension == ".xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Figure));
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                Figure loadedFigure = (Figure)serializer.Deserialize(fileStream);
                if (loadedFigure != null)
                {
                    Name = loadedFigure.Name;
                    Width = loadedFigure.Width;
                    Height = loadedFigure.Height;
                }
            }
        }
        else
        {
            Console.WriteLine("Неподдерживаемый формат файла.");
            Environment.Exit(0);
        }
    }

    public void SaveFile(string filePath)
    {
        string fileExtension = Path.GetExtension(filePath).ToLower();

        if (fileExtension == ".txt")
        {
            File.WriteAllText(filePath, $"{Name}\n{Width}\n{Height}");
        }
        else if (fileExtension == ".json")
        {
            string json = JsonSerializer.Serialize(this);
            File.WriteAllText(filePath, json);
        }
        else if (fileExtension == ".xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Figure));
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fileStream, this);
            }
        }
        else
        {
            Console.WriteLine("Неподдерживаемый формат файла.");
            Environment.Exit(0);
        }
    }

    public void DisplayMenu()
    {
        ConsoleKeyInfo key;
        do
        {
            Console.Clear();
            Console.WriteLine("Текстовый редактор");
            Console.WriteLine("Название: " + Name);
            Console.WriteLine("Ширина: " + Width);
            Console.WriteLine("Высота: " + Height);
            Console.WriteLine("F5 - Сохранить, Escape - Выйти");

            key = Console.ReadKey();

            if (key.Key == ConsoleKey.F5)
            {
                Console.Write("Введите путь для сохранения файла: ");
                string filePath = Console.ReadLine();
                SaveFile(filePath);
                Console.WriteLine("Файл сохранен.");
            }
        } while (key.Key != ConsoleKey.Escape);
    }
}

public class Program
{
    private static Figure figure = new Figure();

    public static void Main(string[] args)
    {
        Console.WriteLine("Текстовый редактор");
        Console.Write("Введите путь к файлу: ");
        string filePath = Console.ReadLine();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не существует. Создать новый? (Y/N)");
            var createNew = Console.ReadKey();
            if (createNew.Key == ConsoleKey.Y)
            {
                figure.CreateNewFile();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        figure.LoadFile(filePath);
        figure.DisplayMenu();
    }
}