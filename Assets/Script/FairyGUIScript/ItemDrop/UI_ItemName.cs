/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ItemDrop
{
	public partial class UI_ItemName : GComponent
	{
		public GGraph m_gra_bg;
		public GTextField m_txt_name;

		public const string URL = "ui://yum1lb3neocd1";

		public static UI_ItemName CreateInstance()
		{
			return (UI_ItemName)UIPackage.CreateObject("ItemDrop","ItemName");
		}

		public UI_ItemName()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_gra_bg = (GGraph)this.GetChildAt(0);
			m_txt_name = (GTextField)this.GetChildAt(1);
		}
	}
}