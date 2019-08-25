using System;
using System.Reflection;
using Exy;
using log4net;
using Newtonsoft.Json;

namespace Cylog {
    public sealed class LogCallerInfo {
        public String CallerType { get; set; }
        public String CallerMember { get; set; }
    }

    public static class LogExt {
        const String EmptyMessageReplacer = "-";
        static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void UseExternalLogger(ILog externalLogger) => logger = externalLogger;

        public static void Debug<T>(String caller, T obj) => Debug(caller, JsonConvert.SerializeObject(obj, Formatting.Indented));

        public static void Debug(String caller, String message = EmptyMessageReplacer)=> logger.Debug($"[{caller}] {message}");

        public static void Info<T>(String caller, T obj) => Info(caller, JsonConvert.SerializeObject(obj, Formatting.Indented));

        public static void Info(String caller, String message = EmptyMessageReplacer) => logger.Info($"[{caller}] {message}");

        public static void Warn<T>(String caller, T obj) => Warn(caller, JsonConvert.SerializeObject(obj, Formatting.Indented));

        public static void Warn(String caller, String message = EmptyMessageReplacer) => logger.Warn($"[{caller}] {message}");

        public static void Error<T>(String caller, T obj) => Error(caller, JsonConvert.SerializeObject(obj, Formatting.Indented));

        public static void Error(String caller, Exception ex) => Error(caller, ex.GetExceptionMessage());

        public static void Error(String caller, String message = EmptyMessageReplacer) => logger.Error($"[{caller}] {message}");

        // NOTE: One is not encouraged to fill `memberName` parameter as it'll be injected by the compiler service
        public static LogCallerInfo GetCallerInfo<T>(this T caller, [System.Runtime.CompilerServices.CallerMemberName] String memberName = "") =>
            new LogCallerInfo {
                CallerType = caller.GetType().Name,
                CallerMember = memberName
            };

        public static String GetFormattedCallerInfoString<T>(this T caller, [System.Runtime.CompilerServices.CallerMemberName] String memberName = "") =>
            $"{caller.GetType().Name}.{memberName}";
    }
}
