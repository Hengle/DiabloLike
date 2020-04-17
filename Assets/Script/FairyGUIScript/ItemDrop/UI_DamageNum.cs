/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ItemDrop
{
	public partial class UI_DamageNum : GComponent
	{
		public GTextField m_txt_null;

		public const string URL = "ui://yum1lb3nhsc63j";

		public static UI_DamageNum CreateInstance()
		{
			return (UI_DamageNum)UIPackage.CreateObject("ItemDrop","DamageNum");
		}

		public UI_DamageNum()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_txt_null = (GTextField)this.GetChildAt(0);
		}
	}
}