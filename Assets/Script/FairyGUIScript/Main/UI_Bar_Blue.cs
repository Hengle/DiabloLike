/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
	public partial class UI_Bar_Blue : GProgressBar
	{
		public GImage m_n0;
		public GImage m_bar;

		public const string URL = "ui://3lyppwh6hsc63";

		public static UI_Bar_Blue CreateInstance()
		{
			return (UI_Bar_Blue)UIPackage.CreateObject("Main","Bar_Blue");
		}

		public UI_Bar_Blue()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_n0 = (GImage)this.GetChildAt(0);
			m_bar = (GImage)this.GetChildAt(1);
		}
	}
}