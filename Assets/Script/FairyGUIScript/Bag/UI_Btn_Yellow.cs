/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Bag
{
	public partial class UI_Btn_Yellow : GButton
	{
		public Controller m_button;
		public GImage m_n0;
		public GTextField m_title;

		public const string URL = "ui://5krhxnalsurul";

		public static UI_Btn_Yellow CreateInstance()
		{
			return (UI_Btn_Yellow)UIPackage.CreateObject("Bag","Btn_Yellow");
		}

		public UI_Btn_Yellow()
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