using System;
using System.IO;
using System.Reflection;
using ILRuntime.Mono.Cecil.Mdb;
using ILRuntime.Mono.Cecil.Pdb;
using ILRuntime.Runtime.Enviorment;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using NotImplementedException = System.NotImplementedException;

namespace GGame.Support
{
    public class HotfixManager : SingleTon<HotfixManager> , IDisposable
    {
        private const string dllPath = "Hotfix/SFLHotfix.dll";
        private const string pdbPath = "Hotfix/SFLHotfix.pdb";
#if ILRuntime
        private AppDomain _domain;
        private MemoryStream _dllStream;
        private MemoryStream _pdbStream;
#endif
        public override void OnInit()
        {
            var dllBytes = ResourceManager.Instance.LoadBytes(dllPath);
            var pdbBytes = ResourceManager.Instance.LoadBytes(pdbPath);
            var hotfixAssembly = Assembly.Load(dllBytes, pdbBytes);
#if !ILRuntime
            var hotfixProgramType = hotfixAssembly.GetType("GGame.Hotfix.Program");
            var mainMethod = hotfixProgramType.GetMethod("Main");

            mainMethod.Invoke(null, null);
#else
            
            _domain = new AppDomain();
           _dllStream = new MemoryStream(dllBytes);
           _pdbStream = new MemoryStream(pdbBytes);
           _domain.LoadAssembly(_dllStream, _pdbStream, new PdbReaderProvider());

           _domain.Invoke("GGame.Hotfix.Program", "Main", null);
#endif
        }


        public void Dispose()
        {
            _domain = null;
            _dllStream.Dispose();
            _pdbStream.Dispose();
        }
    }
}