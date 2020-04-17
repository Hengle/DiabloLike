/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
	public partial class UI_Bar_Blue_V : GProgressBar
	{
		public GImage m_n0;
		public GImage m_bar_v;

		public const string URL = "ui://3lyppwh6hsc6a";

		public static UI_Bar_Blue_V CreateInstance()
		{
			return (UI_Bar_Blue_V)UIPackage.CreateObject("Main","Bar_Blue_V");
		}

		public UI_Bar_Blue_V()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_n0 = (GImage)this.GetChildAt(0);
			m_bar_v = (GImage)this.GetChildAt(1);
		}
	}
}