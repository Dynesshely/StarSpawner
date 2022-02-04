using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conconcon_tri_buter
{
    public class LivelyMessage
    {
        public static string lively_message_template = "%type%(%scope%): %subject%";
        public static string[] possible_filename_parts = new string[32]
        {
            "Helper", "Windows", "Api", "Language", "Config", "Configuration",
            "Version", "Shell", "UI", "Thread", "System", "POST", "POSIX",
            "Debug", "Generator", "Produce", "Program", "AvaloniaUI", "Path",
            "Forests", "Clean", "Poof", "Load", "West", "Home", "Page", "Download",
            "HandyControl", "MaterialDesign", "ModernWpf", "Upload", "GET"
        };
        public static string[] possible_fileextension_parts = new string[30]
        {
            ".cs", ".csproj", ".js", ".sln", ".java", ".class", ".txt",
            ".md", ".apt", ".cpp", ".c", ".h", "", ".ts", ".html", ".css",
            ".min.js", ".min.css", ".min.cs", ".tag", ".a", ".obj", ".fs",
            ".model", ".ini", ".xml", ".xaml", ".png", ".jpg", ".zip"
        };
        public static Dictionary<string, string[]> types_subjects = new()
        {
            {
                "feat",
                new string[10]
                {
                    "added %filename%%extension%",
                    "more threads can async",
                    "menus added",
                    "",
                    "%filename%%extension% was generated",
                    "",
                    "",
                    "",
                    "",
                    ""
                }
            },
            {
                "fix",
                new string[10]
                {
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    ""
                }
            },
            {
                "docs",
                new string[10]
                {
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    ""
                }
            },
            {
                "style",
                new string[10]
                {
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    ""
                }
            },
            {
                "refactor",
                new string[10]
                {
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    ""
                }
            },
            {
                "test",
                new string[10]
                {
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    ""
                }
            },
            {
                "chore",
                new string[10]
                {
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    ""
                }
            },
        };
    }
}
