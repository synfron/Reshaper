using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReshaperScript.Core.Functions;

namespace ReshaperScript.Core
{
    public interface IScriptEngineAdapter : IDisposable
    {
        void EmbedHostObject(string name, object obj);

        object Evaluate(string code);

        void Execute(string code);
    }
}
