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
if (!string.IsNullOrEmpty(output))
            {
                Console.WriteLine("\n--- ВЫВОД ПРОГРАММЫ (STDOUT) ---");
                Console.WriteLine(output.Trim());
            }

            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine("\n--- ОШИБКИ (STDERR) ---");
                Console.WriteLine(error.Trim());
            }

            string exitMessage = $"[{DateTime.Now}] Завершён. Код выхода: {exitCode}";
            File.AppendAllText(logPath, exitMessage + Environment.NewLine);

            if (!string.IsNullOrEmpty(output))
                File.AppendAllText(logPath, $"[STDOUT] {output.Trim()}{Environment.NewLine}");
            if (!string.IsNullOrEmpty(error))
                File.AppendAllText(logPath, $"[STDERR] {error.Trim()}{Environment.NewLine}");

            Console.WriteLine($"\nЛог сохранён в: {Path.GetFullPath(logPath)}");

            Environment.ExitCode = exitCode;
        }
        catch (Exception ex)
        {
            string errorMessage = $"[{DateTime.Now}] ОШИБКА: {ex.Message}";
            Console.WriteLine($"\n!!! {errorMessage}");

            if (!File.Exists(logPath))
            {
                File.WriteAllText(logPath, errorMessage + Environment.NewLine);
            }
            else
            {
                File.AppendAllText(logPath, errorMessage + Environment.NewLine);
            }

            Environment.ExitCode = 1;
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
