using System;
#if FAIRYGUI_TOLUA
using LuaInterface;
#endif

namespace FairyGUI
{
    /// <summary>
    /// 
    /// </summary>
    class EventBridge
    {
        public EventDispatcher owner;

        EventCallback0 _callback0;
        EventCallback1 _callback1;
        EventCallback1 _captureCallback;
        internal bool _dispatching;

        //add by dong  --修改FGUI RunTime代码，按钮点击响应时间检测

        string strType = null;
        float executTime = float.MinValue;

        bool canContinueHit = false;   //是否能连续点击

        public float ExecutTime
        {
            set { executTime = value; }
            get { return executTime; }
        }
        //end
        public EventBridge(EventDispatcher owner,string strType)
		{
            this.strType = strType;

            this.owner = owner;
		}

		public void AddCapture(EventCallback1 callback,bool canContinueHit= false)
		{
			_captureCallback -= callback;
			_captureCallback += callback;
            this.canContinueHit = canContinueHit;

        }

		public void RemoveCapture(EventCallback1 callback)
		{
			_captureCallback -= callback;
		}

		public void Add(EventCallback1 callback, bool canContinueHit=false)
		{
			_callback1 -= callback;
			_callback1 += callback;
            this.canContinueHit = canContinueHit;
		}

		public void Remove(EventCallback1 callback)
		{
			_callback1 -= callback;
		}

		public void Add(EventCallback0 callback,bool canContinueHit= false)
		{
			_callback0 -= callback;
			_callback0 += callback;
            this.canContinueHit = canContinueHit;
        }

		public void Remove(EventCallback0 callback)
		{
			_callback0 -= callback;
		}

#if FAIRYGUI_TOLUA
		public void Add(LuaFunction func, LuaTable self)
		{
			EventCallback1 callback = (EventCallback1)DelegateTraits<EventCallback1>.Create(func, self);
			_callback1 -= callback;
			_callback1 += callback;
		}

		public void Add(LuaFunction func, GComponent self)
		{
			if (self._peerTable == null)
				throw new Exception("self is not connected to lua.");

			Add(func, self._peerTable);
		}

		public void Remove(LuaFunction func, LuaTable self)
		{
			LuaState state = func.GetLuaState();
			LuaDelegate target;
			if (self != null)
				target = state.GetLuaDelegate(func, self);
			else
				target = state.GetLuaDelegate(func);

			Delegate[] ds = _callback1.GetInvocationList();

			for (int i = 0; i < ds.Length; i++)
			{
				LuaDelegate ld = ds[i].Target as LuaDelegate;
				if (ld != null && ld.Equals(target))
				{
					_callback1 = (EventCallback1)Delegate.Remove(_callback1, ds[i]);
					//DelayDispose的处理并不安全，原因在如果Remove后立刻Add，那么DelayDispose会误删除，先注释掉，等待tolua改进
					//state.DelayDispose(ld.func);
					//if (ld.self != null)
					//	state.DelayDispose(ld.self);
					break;
				}
			}
		}

		public void Remove(LuaFunction func, GComponent self)
		{
			if (self._peerTable == null)
				throw new Exception("self is not connected to lua.");

			Remove(func, self._peerTable);
		}
#endif

		public bool isEmpty
		{
			get { return _callback1 == null && _callback0 == null && _captureCallback == null; }
		}

		public void Clear()
		{
#if FAIRYGUI_TOLUA
			//DelayDispose的处理并不安全，原因在如果Remove后立刻Add，那么DelayDispose会误删除，先注释掉，等待tolua改进
			//if (_callback1 != null)
			//{
			//	Delegate[] ds = _callback1.GetInvocationList();
			//	for (int i = 0; i < ds.Length; i++)
			//	{
			//		LuaDelegate ld = ds[i].Target as LuaDelegate;
			//		if (ld != null)
			//		{
			//			LuaState state = ld.func.GetLuaState();
			//			state.DelayDispose(ld.func);
			//			if (ld.self != null)
			//				state.DelayDispose(ld.self);
			//		}
			//	}
			//}
#endif
			_callback1 = null;
			_callback0 = null;
			_captureCallback = null;
		}

		public void CallInternal(EventContext context)
		{
            ////FGUI修改 -默认点击时间，不能连续点击
            //if (strType == "onClick" && !canContinueHit)
            //{
            //    if (UnityEngine.Time.realtimeSinceStartup - executTime < .5f)
            //    {
            //        if (_callback0!=null)
            //           Debug.LogWarning(" Warning Target:"+_callback0.Target.ToString()+" Not Click Now! LastClick:"+ executTime);
            //        return;
            //    }
            //    executTime = UnityEngine.Time.realtimeSinceStartup;
            //}
            ////end

            _dispatching = true;
			context.sender = owner;

            try
			{
				if (_callback1 != null)
					_callback1(context);
				if (_callback0 != null)
					_callback0();
			}
			finally
			{
				_dispatching = false;
			}
		}
		public void CallCaptureInternal(EventContext context)
		{
			if (_captureCallback == null)
				return;

            ////FGUI修改 -默认点击时间，不能连续点击
            //if (strType == "onClick"&&!canContinueHit)
            //{
            //    if (UnityEngine.Time.realtimeSinceStartup - executTime < .5f)
            //    {
            //        if (_captureCallback != null)
            //            Debug.LogWarning(" Warning Target:" + _captureCallback.Target.ToString() + " Not Click Now! LastClick:" + executTime);
            //        return;
            //    }
            //    executTime = UnityEngine.Time.realtimeSinceStartup;
            //}
            ////end

            _dispatching = true;
			context.sender = owner;
			try
			{
				_captureCallback(context);
			}
			finally
			{
				_dispatching = false;
			}
		}
	}
}
