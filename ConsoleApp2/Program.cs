using System;
using System.IO;
using System.Xml.Serialization;
using System.Text.Json;

public class Figure
{
    public string Name { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}

public class Program
{
    private static string filePath;
    private static Figure figure = new Figure();

    public static void Main(string[] args)
    {
        Console.WriteLine("Текстовый редактор");
        Console.Write("Введи путь к файлу: ");
        filePath = Console.ReadLine();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не существует. Создать новый? (Q/N)");
            var createNew = Console.ReadKey();
            if (createNew.Key == ConsoleKey.Q)
            {
                CreateNewFile();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        LoadFile();
        DisplayMenu();
    }

    private static void CreateNewFile()
    {
        Console.Write("Введи название фигуры: ");
        figure.Name = Console.ReadLine();

        Console.Write("Введи ширину: ");
        if (double.TryParse(Console.ReadLine(), out double width))
        {
            figure.Width = width;
        }
        else
        {
            Console.WriteLine("Ошибка при вводе ширины.");
            Environment.Exit(0);
        }

        Console.Write("Введи высоту: ");
        if (double.TryParse(Console.ReadLine(), out double height))
        {
            figure.Height = height;
        }
        else
        {
            Console.WriteLine("Ошибка при вводе высоты.");
            Environment.Exit(0);
        }

        SaveFile();
    }

    private static void LoadFile()
    {
        string fileExtension = Path.GetExtension(filePath).ToLower();


        if (fileExtension == ".txt")
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 3)
            {
                figure.Name = lines[0];
                if (double.TryParse(lines[1], out double width))
                {
                    figure.Width = width;
                }
                if (double.TryParse(lines[2], out double height))
                {
                    figure.Height = height;
                }
            }
        }
        else if (fileExtension == ".json")
        {
            string json = File.ReadAllText(filePath);
            figure = JsonSerializer.Deserialize<Figure>(json);
        }
        else if (fileExtension == ".xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Figure));
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                figure = (Figure)serializer.Deserialize(fileStream);
            }
        }
    }

    private static void SaveFile()
    {
        string fileExtension = Path.GetExtension(filePath).ToLower();


        if (fileExtension == ".txt")
        {
            File.WriteAllText(filePath, $"{figure.Name}\n{figure.Width}\n{figure.Height}");
        }
        else if (fileExtension == ".json")
        {
            string json = JsonSerializer.Serialize(figure);
            File.WriteAllText(filePath, json);
        }
        else if (fileExtension == ".xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Figure));
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fileStream, figure);
            }
        }
    }

    private static void DisplayMenu()
    {
        ConsoleKeyInfo key;
        do
        {
            Console.Clear();
            Console.WriteLine("Текстовый редактор");
            Console.WriteLine("Название: " + figure.Name);
            Console.WriteLine("Ширина: " + figure.Width);
            Console.WriteLine("Высота: " + figure.Height);
            Console.WriteLine("F1 - Сохранить, Escape - Выйти");

            key = Console.ReadKey();

            if (key.Key == ConsoleKey.F1)
            {
                SaveFile();
                Console.WriteLine("Файл сохранен.");
            }
        } while (key.Key != ConsoleKey.Escape);
    }
}