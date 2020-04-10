/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
	public partial class UI_Bar_Blood : GProgressBar
	{
		public GImage m_n0;
		public GImage m_bar;

		public const string URL = "ui://3lyppwh6hsc62";

		public static UI_Bar_Blood CreateInstance()
		{
			return (UI_Bar_Blood)UIPackage.CreateObject("Main","Bar_Blood");
		}

		public UI_Bar_Blood()
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