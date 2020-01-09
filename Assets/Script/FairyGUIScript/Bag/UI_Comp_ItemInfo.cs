/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Bag
{
	public partial class UI_Comp_ItemInfo : GComponent
	{
		public GImage m_n0;
		public UI_Comp_Item m_comp_item;
		public GTextField m_txt_name;
		public GTextField m_txt_property;
		public UI_Btn_Blue m_btn_cancel;
		public UI_Btn_Yellow m_btn_OK;

		public const string URL = "ui://5krhxnalsuruk";

		public static UI_Comp_ItemInfo CreateInstance()
		{
			return (UI_Comp_ItemInfo)UIPackage.CreateObject("Bag","Comp_ItemInfo");
		}

		public UI_Comp_ItemInfo()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_n0 = (GImage)this.GetChildAt(0);
			m_comp_item = (UI_Comp_Item)this.GetChildAt(1);
			m_txt_name = (GTextField)this.GetChildAt(2);
			m_txt_property = (GTextField)this.GetChildAt(3);
			m_btn_cancel = (UI_Btn_Blue)this.GetChildAt(4);
			m_btn_OK = (UI_Btn_Yellow)this.GetChildAt(5);
		}
	}
}