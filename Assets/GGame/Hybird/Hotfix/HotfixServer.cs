using System;
using System.Reflection;
using GGame.Core;

namespace GGame.Hybird
{
    public class HotfixServer : SingleTon<HotfixServer> , IDisposable , IInit
    {
        private const string dllPath = "hotfix_dll";
        private const string pdbPath = "hotfix_pdb";
        public Type[] HotfixType { get; set; }
#if ILRuntime
        private AppDomain _domain;
        private MemoryStream _dllStream;
        private MemoryStream _pdbStream;
#endif

        
        public void Dispose()
        {
#if ILRuntime
            _domain = null;
            _dllStream.Dispose();
            _pdbStream.Dispose();
#endif
        }
#if ILRuntime
        void OnILInitialize()
        {
            _domain?.DelegateManager.RegisterFunctionDelegate<int, GGame.Support.Frame>();

            _domain?.RegisterCrossBindingAdaptor(new FrameAdapter());
            
            _domain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
            {
                return new UnityEngine.Events.UnityAction(() =>
                {
                    ((Action)act)();
                });
            });


        }
#endif
        public void Init()
        {
            var dllBytes = GResourceServer.Instance.LoadBytes(dllPath);
            var pdbBytes = GResourceServer.Instance.LoadBytes(pdbPath);
            var hotfixAssembly = Assembly.Load(dllBytes, pdbBytes);
#if !ILRuntime
            var hotfixProgramType = hotfixAssembly.GetType("GGame.Hotfix.Program");
            var mainMethod = hotfixProgramType.GetMethod("Main");

            HotfixType = hotfixAssembly.GetTypes();
            mainMethod.Invoke(null, null);
#else
            
            _domain = new AppDomain();
           _dllStream = new MemoryStream(dllBytes);
           _pdbStream = new MemoryStream(pdbBytes);
           _domain.LoadAssembly(_dllStream, _pdbStream, new PdbReaderProvider());
           OnILInitialize();
           
           List<Type> ts = new List<Type>();

           foreach (var iType in _domain.LoadedTypes)
           {
               ts.Add(iType.Value.ReflectionType);
           }

           HotfixType = ts.ToArray();
           _domain.Invoke("GGame.Hotfix.Program", "Main", null);
#endif
        }
    }
}