/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Common
{
	public partial class UI_Btn_Blue : GButton
	{
		public Controller m_button;
		public GImage m_n0;
		public GTextField m_title;

		public const string URL = "ui://3p5wu2qnhsc61";

		public static UI_Btn_Blue CreateInstance()
		{
			return (UI_Btn_Blue)UIPackage.CreateObject("Common","Btn_Blue");
		}

		public UI_Btn_Blue()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_button = this.GetControllerAt(0);
			m_n0 = (GImage)this.GetChildAt(0);
			m_title = (GTextField)this.GetChildAt(1);
		}
	}
}