

using System;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

/// <summary>
/// 界面名字
/// 与窗口类名相同
/// </summary>
public enum EUIType
{
    UIMain,
    UIBag,
    UIItemDrop,//物品掉落
    UIItemUse,
    UIBloodBar,
    UIDamageNum,
}

/// <summary>
/// time:2019/3/24
/// author:Sun
/// description:UI窗口基类
/// </summary>
public class UIManager:Singleton<UIManager>{
		
	/// <summary>
	/// 所有Windows
	/// </summary>
	private Dictionary<EUIType, UIBase> _uIArray = new Dictionary<EUIType, UIBase>();
		

	public void Init()
	{
        FGuiManager.Instance.Initialize();
	}
		
	/// <summary>
	/// 获取窗口页面
	/// </summary>
	/// <param name="uiName"></param>
	/// <returns></returns>
	public UIBase GetWindow(EUIType uiName)
	{
		UIBase wind = null;
		foreach (EUIType name in _uIArray.Keys)
		{
			if (name == uiName)
			{
				wind = _uIArray[name];
				break;
			}
		}
		return wind;
	}

	/// <summary>
	/// 创建Ui实例
	/// </summary>
	/// <param name="uiName"></param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public UIBase CreateWindow(EUIType uiName)
	{
		UIBase wind = null;
		wind = Activator.CreateInstance(Type.GetType(uiName.ToString(),true)) as UIBase;
		if (wind==null)
		{
			throw new Exception("不存在"+uiName+"页面");
		}
		return wind;
	}
		
	/// <summary>
	/// 得到所有处于打开状态的窗口页面
	/// </summary>
	/// <returns></returns>
	public List<EUIType> GetAllOpenWindows()
	{
		List<EUIType> list = new List<EUIType>();
		foreach(EUIType uiName in _uIArray.Keys)
		{
			if (IsOpenWindow(uiName))
			{
				list.Add(uiName);
			}
		}
		return list;
	}
		
	/// <summary>
	/// 关闭所有打开的窗口
	/// </summary>
	/// <param name="isMode"></param>
	public void DeleteAllWindows()
	{
			
	}	
		
	/// <summary>
	/// 窗口是否处于打开状态
	/// </summary>
	/// <param name="uiName"></param>
	/// <returns></returns>
	public bool IsOpenWindow(EUIType uiName)
	{
		UIBase wind = GetWindow(uiName);
		if (wind != null)
		{
			return wind.isShowing;
		}
		return false;
	}

	/// <summary>
	/// 展示窗口
	/// </summary>
	/// <param name="baseUi"></param>
	public void ShowWind(EUIType winName, params object[] datas)
	{
		UIBase baseUi = GetWindow(winName);
		if (baseUi==null)
		{
			baseUi =CreateWindow(winName);
			_uIArray.Add(winName, baseUi);
		}
		baseUi.Show();
        baseUi.AfterOnShown(datas);
	}
		
	/// <summary>
	/// 隐藏窗口
	/// </summary>
	/// <param name="baseUi"></param>
	public void CloseWind(EUIType winName)
	{
		UIBase baseUi = GetWindow(winName);
		if (baseUi==null)
		{
			throw new Exception("该页面不存在！");
		}
		baseUi.Hide();
	}
}

