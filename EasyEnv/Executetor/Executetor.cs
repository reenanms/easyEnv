using System.Diagnostics;
using System.IO;
using System.Text;

namespace EasyEnv
{
    public class Executetor
    {
        private class ExitCode
        {
            public const int Sucess = 0;
        }
        private string processArguments;
        private StringBuilder stringBuilder;
        private Process process;
        private string[] processCommands;

        public Executetor(string program, string workingDirectory)
        {
            this.Program = program;
            this.WorkingDirectory = !string.IsNullOrEmpty(workingDirectory) ? workingDirectory : Directory.GetCurrentDirectory();
        }

        public string Program { get; private set; }
        public string WorkingDirectory { get; private set; }
        public INotifier Notifier { get; set; }

        public string Run(params string[] arguments)
        {
            return Run(arguments, new string[] { });
        }

        public string Run(string[] arguments, string[] commands)
        {
            processArguments = string.Join(" ", arguments);
            processCommands = commands;
            return runProcess();
        }

        private string runProcess()
        {
            generateProcess();
            startProcess();

            writeCommand();
            process.WaitForExit();
            validateProcessExitCode();

            return stringBuilder.ToString();
        }

        private void startProcess()
        {
            stringBuilder = new StringBuilder();

            if (!process.Start())
                throw new ExecutorErrorException(Program);

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }

        private void writeCommand()
        {
            foreach (var command in processCommands)
                writeInput(process.StandardInput, command);
        }

        private void buildMessage(string rawMessage)
        {
            if (!string.IsNullOrEmpty(rawMessage))
            {
                Notifier?.Notify(rawMessage);
                stringBuilder.AppendLine(rawMessage);
            }
        }

        private void validateProcessExitCode()
        {
            if (process.ExitCode != ExitCode.Sucess)
                throw new ExecutorErrorException(Program);
        }

        private void generateProcess()
        {
            var processInfo = new ProcessStartInfo()
            {
                FileName = Program,
                WorkingDirectory = WorkingDirectory,
                Arguments = processArguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                CreateNoWindow = true
            };

            process = new Process()
            {
                StartInfo = processInfo,
                EnableRaisingEvents = false,
            };

            process.OutputDataReceived += (s, e) => buildMessage(e.Data);
            process.ErrorDataReceived += (s, e) => buildMessage(e.Data);
        }

        private static void writeInput(StreamWriter input, string command)
        {
            input.WriteLine(command);
            input.Flush();
            input.Close();
        }
    }
}
