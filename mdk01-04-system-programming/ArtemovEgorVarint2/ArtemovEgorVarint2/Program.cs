using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string fileName;
        string arguments;

        if (args.Length == 0)
        {
            Console.WriteLine("=== Утилита запуска внешних процессов ===");
            Console.Write("Введите путь к программе: ");
            fileName = Console.ReadLine();

            Console.Write("Введите аргументы (или нажмите Enter, если их нет): ");
            arguments = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(fileName))
            {
                Console.WriteLine("Путь к программе не указан!");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
                return;
            }
        }
        else
        {
            fileName = args[0];
            arguments = string.Join(" ", args, 1, args.Length - 1);
        }

        string logPath = "process_log.txt";

        Console.WriteLine($"\n[INFO] Путь к логу: {Path.GetFullPath(logPath)}");

        try
        {
            Console.WriteLine($"\nЗапуск: {fileName} {arguments}");
            Console.WriteLine("Ожидание завершения процесса...\n");

            string startMessage = $"[{DateTime.Now}] Запуск: {fileName} {arguments}";
            File.AppendAllText(logPath, startMessage + Environment.NewLine);
            Console.WriteLine($"[LOG] Запись о запуске добавлена в лог");

            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            int exitCode = process.ExitCode;

            Console.WriteLine($"=== ПРОЦЕСС ЗАВЕРШЁН ===");
            Console.WriteLine($"Код выхода: {exitCode} (0x{exitCode:X})");
            Console.WriteLine($"Время завершения: {DateTime.Now}");

}
    }
