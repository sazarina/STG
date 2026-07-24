using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace STG.Engine.Debugging {
    public class Debug {
        public static bool isDebug = false;

        public static DebugWindowForm? instance;

        #region BaseFunctions

        public enum LogType {
            Error,
            Exception,
            Log,
        }

        public static string GetEnumString(LogType debugType) {
            var name = Enum.GetName(typeof(LogType), debugType);
            if (name == null) {
                return "Unknown";
            } else {
                return name;
            }
        }

        public static void Init(DebugWindowForm own) {
            Debug.instance = own;
        }

        internal static LogData Default => new LogData();

        #endregion BaseFunctions


        //public static void Log(LogData debugInfo, string tag, string message) {
        //    AddListView(debugInfo, LogType.Log, tag, message);
        //}

        internal static void Log(LogData debugInfo, string tag = "", string message = "") {
            AddListView(debugInfo, LogType.Log, tag, message);
        }


        public static void Log(string tag, string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0, [CallerMemberName] string memberName = "") {
            AddListView(LogData.Get(filePath, line, memberName), LogType.Log, tag, message);
        }

        public static void Log(object text, string tag = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0, [CallerMemberName] string memberName = "") {
            AddListView(LogData.Get(filePath, line, memberName), LogType.Log, tag, text==null ? "null" : text.ToString());
        }

        public static void Log(IEnumerable<object> objects,[CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0, [CallerMemberName] string memberName = "") {
            foreach (var obj in objects) {
                AddListView(LogData.Get(filePath, line, memberName), LogType.Log, "", obj==null ? "null" : obj.ToString());
            }
        }

        public static void Log(IEnumerable<string> texts, [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0, [CallerMemberName] string memberName = "") {
            foreach (var text in texts) {
                AddListView(LogData.Get(filePath, line, memberName), LogType.Log, "", text==null ? "null" : text);
            }
        }



        public static void LogException(string tag, Exception e, [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0, [CallerMemberName] string memberName = "") {
            AddListView(LogData.Get(filePath, line, memberName), LogType.Exception, tag, e.Message);
        }

        public static void LogError(string tag, string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0, [CallerMemberName] string memberName = "") {
            AddListView(LogData.Get(filePath, line, memberName), LogType.Error, tag, message);
        }


        static void AddListView(LogData info, LogType debugType, string tag, string? message) {
            info.debugType = debugType;
            string[] lst = { GetEnumString(debugType), tag, message==null ? "null" : message, info.fileRelativePath, info.member, info.line };

            if (instance == null) {
                throw new Exception("Debug instance is not initialized.");
            }

            instance.listView1.Items.Insert(0, new ListViewItem(lst));
            //instance.OriginItemCorection.Add(new ListViewItem(lst));
        }

    }
}