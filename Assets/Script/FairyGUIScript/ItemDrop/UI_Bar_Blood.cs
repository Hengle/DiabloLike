/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ItemDrop
{
	public partial class UI_Bar_Blood : GProgressBar
	{
		public GGraph m_n0;
		public GGraph m_bar;

		public const string URL = "ui://yum1lb3nhsc63m";

		public static UI_Bar_Blood CreateInstance()
		{
			return (UI_Bar_Blood)UIPackage.CreateObject("ItemDrop","Bar_Blood");
		}

		public UI_Bar_Blood()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_n0 = (GGraph)this.GetChildAt(0);
			m_bar = (GGraph)this.GetChildAt(1);
		}
	}
}