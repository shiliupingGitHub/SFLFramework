using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace GGame.Hybird
{
    public class FrameAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get { return typeof(Frame); }
        }

        public override Type AdaptorType
        {
            get { return typeof(Adaptor); }
        }
        
        public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
        {
           return new Adaptor(appdomain, instance);
        }

        
        class Adaptor : Frame,CrossBindingAdaptorType
        {
            
            ILTypeInstance _instance;
            AppDomain _appdomain;
            private IMethod _onShowMethod;
            private IMethod _onHideMethod;
            private IMethod _onInitMethod;
            private IMethod _onDestroyMethod;
            public ILTypeInstance ILInstance { get { return _instance; } }
            
            public Adaptor(AppDomain appdomain, ILTypeInstance instance)
            {
                this._appdomain = appdomain;
                this._instance = instance;
            }

            public  void OnShow(System.Object o)
            {
                if(null == _onShowMethod)
                    _onShowMethod = _instance.Type.GetMethod("OnShow", 1);

                _appdomain.Invoke(_onShowMethod, _instance, o);
            }

            public  void OnHide()
            {
                if(null == _onHideMethod)
                    _onHideMethod = _instance.Type.GetMethod("OnHide", 0);

                _appdomain.Invoke(_onHideMethod, _instance, null);
            }

            public void OnInit()
            {
                if(null == _onInitMethod)
                    _onInitMethod = _instance.Type.GetMethod("OnInit", 0);

                _appdomain.Invoke(_onInitMethod, _instance, null);
            }

            public void OnDestroy()
            {
                if(null == _onDestroyMethod)
                    _onDestroyMethod = _instance.Type.GetMethod("OnDestroy", 0);

                _appdomain.Invoke(_onDestroyMethod, _instance, null);
            }

            public override string ToString()
            {
                IMethod m = _appdomain.ObjectType.GetMethod("ToString", 0);
                m = _instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    return _instance.ToString();
                }
                else
                    return _instance.Type.FullName;
            }
        }
    }

}


