/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Bag
{
	public partial class UI_Btn_Close : GButton
	{
		public Controller m_button;
		public GImage m_n3;

		public const string URL = "ui://5krhxnalsuruh";

		public static UI_Btn_Close CreateInstance()
		{
			return (UI_Btn_Close)UIPackage.CreateObject("Bag","Btn_Close");
		}

		public UI_Btn_Close()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_button = this.GetControllerAt(0);
			m_n3 = (GImage)this.GetChildAt(0);
		}
	}
}