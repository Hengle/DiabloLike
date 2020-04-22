/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
	public partial class UI_Btn_Main : GButton
	{
		public Controller m_button;
		public GImage m_n0;
		public GTextField m_title;
		public GLoader m_icon;
		public GTextField m_txt_num;

		public const string URL = "ui://3lyppwh6hsc67";

		public static UI_Btn_Main CreateInstance()
		{
			return (UI_Btn_Main)UIPackage.CreateObject("Main","Btn_Main");
		}

		public UI_Btn_Main()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_button = this.GetControllerAt(0);
			m_n0 = (GImage)this.GetChildAt(0);
			m_title = (GTextField)this.GetChildAt(1);
			m_icon = (GLoader)this.GetChildAt(2);
			m_txt_num = (GTextField)this.GetChildAt(3);
		}
	}
}