/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
	public partial class UI_Btn_List : GButton
	{
		public Controller m_button;
		public GImage m_n0;
		public GLoader m_icon;

		public const string URL = "ui://3lyppwh6hsc68";

		public static UI_Btn_List CreateInstance()
		{
			return (UI_Btn_List)UIPackage.CreateObject("Main","Btn_List");
		}

		public UI_Btn_List()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_button = this.GetControllerAt(0);
			m_n0 = (GImage)this.GetChildAt(0);
			m_icon = (GLoader)this.GetChildAt(1);
		}
	}
}