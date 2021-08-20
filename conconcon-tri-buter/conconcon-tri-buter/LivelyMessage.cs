using System;
using System.Collections.Generic;
using System.Linq;

namespace conconcon_tri_buter
{
    public class LivelyMessage
    {
        /// <summary>
        /// 拟真消息模板
        /// </summary>
        public static string lively_message_template = "%type%%scope%:%subject%";

        /// <summary>
        /// 可能的文件名
        /// </summary>
        public static string[] possible_filename_parts = new string[35]
        {
            "Helper", "Windows", "Api", "Language", "Config", "Configuration",
            "Version", "Shell", "UI", "Thread", "System", "POST", "POSIX",
            "Debug", "Generator", "Produce", "Program", "AvaloniaUI", "Path",
            "Forests", "Clean", "Poof", "Load", "West", "Home", "Page", "Download",
            "HandyControl", "MaterialDesign", "ModernWpf", "Upload", "GET",
            "LICENSE", "README", "index"
        };

        /// <summary>
        /// 可能的文件扩展名
        /// </summary>
        public static string[] possible_fileextension_parts = new string[34]
        {
            ".cs", ".csproj", ".js", ".sln", ".java", ".class", ".txt",
            ".md", ".apt", ".cpp", ".c", ".h", "", ".ts", ".html", ".css",
            ".min.js", ".min.css", ".min.cs", ".tag", ".a", ".obj", ".fs",
            ".model", ".ini", ".xml", ".xaml", ".png", ".jpg", ".zip",
            ".docx", ".xls", ".config", ".pptx"
        };

        /// <summary>
        /// type-subject 键值对
        /// 可能的标题词组
        /// </summary>
        public static Dictionary<string, string[]> types_subjects = new()
        {
            {
                "feat",
                new string[35]
                {
                    "added", "more", "menus", "fundation",
                    "multi-language", "release", "link", "image",
                    "support", "import", "threads", "attach",
                    "models", "publish", "config", "magic",
                    "async", "socket", "web", "fronted", "feature",
                    "server", "client", "bridge", "controls",
                    "UI", "draw", "pull", "function", "generate",
                    "machine", "ID", "export", "using", "library"
                }
            },
            {
                "fix",
                new string[11]
                {
                    "bugs", "windows api error", "founded", "after test",
                    "errors decreased", "warnings decreased",
                    "memory", "error", "cpu", "IO", "tree"
                }
            },
            {
                "docs",
                new string[12]
                {
                    "edited", "wording", "details", "added", "improved", "docs",
                    "support", "language", "email", "QQ", "phone", "nice"
                }
            },
            {
                "style",
                new string[10]
                {
                    "lines", "prettier", "beautify", "just", "in",
                    "message", "twice", "cubic", "tab", "align"
                }
            },
            {
                "refactor",
                new string[10]
                {
                    "indirect layer", "object", "class", "new",
                    "public", "private", "bloated", "shit",
                    "code", "nice"
                }
            },
            {
                "test",
                new string[10]
                {
                    "UI", "thread", "test", "nothing", "just", "ad",
                    "new feature", "wonderful", "failed", "address"
                }
            },
            {
                "chore",
                new string[10]
                {
                    "make", "cmake", ".gitignore", "updated", "GIT",
                    "LICENSE", "README", "apt", "npm", "pip"
                }
            },
        };

        /// <summary>
        /// 生成随机下标类
        /// </summary>
        /// <param name="tmp">下标源</param>
        /// <returns>下标</returns>
        private static int RandIndex(string[] tmp) => rand.Next(0, tmp.Length - 1);

        /// <summary>
        /// 返回随机项
        /// </summary>
        /// <param name="src">项源</param>
        /// <returns>项</returns>
        private static string RandItem(string[] src) => src[RandIndex(src)];

        /// <summary>
        /// 返回随机文件名
        /// </summary>
        /// <returns>随机文件名</returns>
        private static string RandFileName() => RandItem(possible_filename_parts) +
            RandItem(possible_fileextension_parts);

        /// <summary>
        /// 随机类, 以当前秒针数作种子
        /// </summary>
        private static Random rand = new(DateTime.Now.Second);

        /// <summary>
        /// 返回一个拟真消息
        /// </summary>
        /// <returns>拟真消息</returns>
        public static string GetLivelyMessage()
        {
            string template = new(lively_message_template);
            bool isFileNameInit = rand.Next(0, 1) == 0; // 是否在 Subject 中加入随机文件名
            bool isScopeInit = rand.Next(0, 1) == 0; // 是否拥有 Scope

            #region 生成 scope
            if (isScopeInit)
                template = template.Replace("%scope%", $"({RandFileName()})");
            else template = template.Replace("%scope%", "");
            #endregion

            #region 生成 type
            string[] keys = types_subjects.Keys.ToArray();
            string type = keys[RandIndex(keys)];
            template = template.Replace("%type%", type);
            #endregion

            #region 生成 subject
            string[] words_src = types_subjects[type];
            int words_count = rand.Next(2, words_src.Length - 1);
            string subject = "";
            for (int i = 1; i <= words_count; ++i)
                subject += $" {RandItem(words_src)}" + (rand.Next(0, 7) >= 6 ? " %fn%" : "");
            if (isFileNameInit)
                while(subject.IndexOf("%fn%") != -1)
                    subject = subject.Replace("%fn%", $"{RandFileName()}");
            template = template.Replace("%subject%", subject);
            #endregion

            return template;
        }
    }
}
