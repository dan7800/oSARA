using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidCodeAnalyzer
{
    class CommitFile
    {
        string path = string.Empty;
        string operation = string.Empty;

        public string Path { get => path;}
        public string Operation { get => operation;}

        public CommitFile(string Path, string Operation)
        {
            path = Path;
            operation = Operation;
        }
    }
}
