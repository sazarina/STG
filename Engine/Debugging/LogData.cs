using System;
using System.IO;
using static STG.Engine.Debugging.Debug;

namespace STG.Engine.Debugging {
        internal class LogData {
            public LogType debugType;

            public string message;
            public string member;
            public string fileRelativePath;
            public string line;

            internal static LogData Get(string filePath, int line, string memberName) {
                Uri BaseUri = new Uri(Path.GetFullPath("../../"));
                Uri uri = new Uri(filePath);
                var relativePath = BaseUri.MakeRelativeUri(uri).ToString();

                return new LogData() {
                    fileRelativePath = relativePath,
                    line = line.ToString(),
                    member = memberName,
                };
            }
    }
}
