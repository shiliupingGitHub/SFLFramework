using System;
using System.Collections.Generic;
using NotImplementedException = System.NotImplementedException;

namespace GGame.Support
{
    [Support]
    public class ProcedureManager : SingleTon<ProcedureManager>
    {
        Dictionary<Type, IProcedure> _procedures = new Dictionary<Type, IProcedure>();
        public override void OnInit()
        {
            var assembly = typeof(IProcedure).Assembly;
            var types = assembly.GetTypes();
            var baseProcedureType = typeof(IProcedure);

            foreach (var type in types)
            {
                if (baseProcedureType.IsAssignableFrom(type) && !type.IsAbstract)
                {
                    var procedure = Activator.CreateInstance(type) as IProcedure;
                    _procedures[type] = procedure;
                }
            }
        }

        public void Enter<T, W>(W w) where T:IProcedure
        {
            var type = typeof(T);
            
            _procedures[type].Enter(w);
        }
        
    }
}