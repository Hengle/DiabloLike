/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ItemDrop
{
	public partial class UI_BloodBar : GComponent
	{
		public GTextField m_txt_null;

		public const string URL = "ui://yum1lb3nhsc63k";

		public static UI_BloodBar CreateInstance()
		{
			return (UI_BloodBar)UIPackage.CreateObject("ItemDrop","BloodBar");
		}

		public UI_BloodBar()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_txt_null = (GTextField)this.GetChildAt(0);
		}
	}
}