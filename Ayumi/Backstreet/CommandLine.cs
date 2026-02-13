using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Arvy;
using Vily;

namespace Backstreet {
    public class CommandLine {
        readonly String cmdApp;
        readonly String workingDir;

        public CommandLine(String cmdApp, String workingDir) {
            this.cmdApp = cmdApp ?? throw new ArgumentNullException(nameof(cmdApp));
            this.workingDir = workingDir ?? throw new ArgumentNullException(nameof(workingDir));
        }

        public IList<String> Execute(String command, params CommandLineArgument[] args) {
            StreamWriter consoleWriter = null;
            StreamReader consoleReader = null;

            try {
                String processArgs = BuildProcessArgs(command, args);
                var process = new Process {
                    StartInfo = new ProcessStartInfo(cmdApp) {
                        UseShellExecute = false,
                        ErrorDialog = false,
                        CreateNoWindow = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        StandardOutputEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage),
                        StandardErrorEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage),

                        WorkingDirectory = workingDir,
                        Arguments = processArgs
                    },
                    EnableRaisingEvents = true
                };
                process.Start();

                consoleWriter = process.StandardInput;
                consoleReader = process.StandardOutput;
                String output = consoleReader.ReadToEnd();

                process.Dispose();

                return output
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                    .ToList();
            }
            finally {
                consoleWriter?.Close();
                consoleReader?.Close();
            }
        }

        String BuildProcessArgs(String command, IEnumerable<CommandLineArgument> args) {
            String cmdAppCommand = command.Trim();
            if (!String.IsNullOrEmpty(cmdAppCommand)) {
                ActionResponseViewModel response = ValidateCommand(cmdAppCommand);
                if (response.ResponseType == ActionResponseViewModel.Error)
                    throw new InvalidOperationException(response.Message);

                return cmdAppCommand + " " + BuildCmdAppCommandArgs(args).Trim();
            }

            return BuildCmdAppCommandArgs(args).Trim();
        }

        String BuildCmdAppCommandArgs(IEnumerable<CommandLineArgument> args) {
            var validated = new Validated<CommandLineArgument> {
                Messages = args
                    .Select(ValidateCommandArgument)
                    .ToList()
            };

            if (validated.ContainsFail) {
                var msgBuilder = new StringBuilder()
                    .AppendLine("Please correct the command arguments below:")
                    .AppendLine();

                foreach (ActionResponseViewModel response in validated.Messages)
                    msgBuilder.AppendLine(response.Message);

                throw new InvalidOperationException(msgBuilder.ToString());
            }

            return String.Join(" ", args.Select(arg => arg.ToString()));
        }

        readonly Regex CmdAppCommandValidator = new Regex(@"^[a-zA-Z0-9\._\-]+$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        ActionResponseViewModel ValidateCommand(String command) {
            Boolean valid = CmdAppCommandValidator.IsMatch(command);
            return new ActionResponseViewModel {
                Message = valid ? String.Empty : $"{command} is not valid command.",
                ResponseType = valid ? ActionResponseViewModel.Success : ActionResponseViewModel.Error
            };
        }

        /*
        -u
        -U
        --use-database
        --Use-Database
        :usedatabase
        :UseDatabase
        useDatabase:
        :--cancel
        -cancel
        --cancel
        anomalydata
        AnomalyData
        --multi-valued:a,b,c,d
        --single-value:a
        --multi-valued=a,b,c,d
        --single-value=a
        */
        readonly Regex CmdAppCommandArgumentValidator = new Regex(@"^\-{1,2}[a-zA-Z0-9\._\-:=,]+$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        ActionResponseViewModel ValidateCommandArgument(CommandLineArgument arg) {
            Boolean valid = CmdAppCommandArgumentValidator.IsMatch(arg.Name);
            return new ActionResponseViewModel {
                Message = valid ? String.Empty : $"{arg.Name} is not valid command argument.",
                ResponseType = valid ? ActionResponseViewModel.Success : ActionResponseViewModel.Error
            };
        }
    }
}