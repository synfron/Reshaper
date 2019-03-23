using MsieJavaScriptEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReshaperScript.Core
{
    public class MsieJsEngineAdapter : IScriptEngineAdapter
    {
        private readonly MsieJsEngine _engine;

        public MsieJsEngineAdapter(MsieJsEngine engine)
        {
            _engine = engine;
        }

        public void Dispose()
        {
            _engine.Dispose();
        }

        public void EmbedHostObject(string name, object obj)
        {
            _engine.EmbedHostObject(name, obj);
        }

        public object Evaluate(string code)
        {
            return _engine.Evaluate(code);
        }

        public void Execute(string code)
        {
            _engine.Execute(code);
        }
    }
}
