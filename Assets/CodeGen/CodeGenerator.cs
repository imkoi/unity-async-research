#if UNITY_EDITOR

using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CodeGen
{
    public static class CodeGenerator
    {
        [MenuItem("Code Generator/Awaitable")]
        public static void GenerateAwaitable()
        {
            var usages = new string[]
            {
                "using System.Threading;",
                "using UnityEngine;"
            };
            var className = "AwaitableGenerated";

            Generate("/CodeGen/Awaitable/AwaitableGenerated.cs",
                usages,
                className,
                "Awaitable",
                "await Awaitable.NextFrameAsync();",
                250);
        }
        
        [MenuItem("Code Generator/Task")]
        public static void GenerateTask()
        {
            var usages = new string[]
            {
                "using System.Threading;",
                "using System.Threading.Tasks;"
            };
            var className = "TaskGenerated";

            Generate("/CodeGen/Task/TaskGenerated.cs",
                usages,
                className,
                "Task",
                "await Task.Yield();",
                250);
        }
        
        [MenuItem("Code Generator/UniTask")]
        public static void GenerateUniTask()
        {
            var usages = new string[]
            {
                "using System.Threading;",
                "using Cysharp.Threading.Tasks;"
            };
            var className = "UniTaskGenerated";

            Generate("/CodeGen/UniTask/UniTaskGenerated.cs",
                usages,
                className,
                "UniTask",
                "await UniTask.Yield();",
                250);
        }

        private static void Generate(
            string path,
            string[] usages,
            string className,
            string taskName,
            string delayMethodName,
            int count)
        {
            var stringBuilder = new StringBuilder();

            GenerateUsages(usages, stringBuilder);
            
            GenerateOpenClass(className, stringBuilder);
            
            for (int i = 1; i <= count; i++)
            {
                GenerateMethod(taskName, $"Method{i}", delayMethodName, stringBuilder);

                if (i + 1 <= count)
                {
                    stringBuilder.AppendLine();
                }
            }

            GenerateCloseClass(stringBuilder);

            File.WriteAllText(Application.dataPath + path, stringBuilder.ToString());
            
            AssetDatabase.Refresh();
        }

        private static void GenerateUsages(string[] usages, StringBuilder stringBuilder)
        {
            foreach (var usage in usages)
            {
                stringBuilder.AppendLine(usage);
            }
            
            stringBuilder.AppendLine();
        }

        private static void GenerateOpenClass(string className, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine($"public class {className}");
            stringBuilder.AppendLine("{");
        }
        
        private static void GenerateCloseClass(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("}");
        }

        private static void GenerateMethod(string taskName, string methodName, string delayMethod, StringBuilder stringBuilder)
        {
            const string tab = "    ";
            
            stringBuilder.AppendLine(tab + $"public async {taskName}<int> {methodName}(CancellationToken cancellationToken)");
            stringBuilder.AppendLine(tab + "{");

            var callCount = int.Parse(methodName.Substring("Method".Length));

            for (int i = 1; i < callCount; i++)
            {
                stringBuilder.AppendLine(tab + tab + $"await Method{i}(cancellationToken);");
            }

            if (callCount > 2)
            {
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine(tab + tab + delayMethod);
            
            stringBuilder.AppendLine();

            stringBuilder.AppendLine(tab + tab + "return 0;");

            stringBuilder.AppendLine(tab + "}");
        }
    }
}

#endif
