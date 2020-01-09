/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Bag
{
	public partial class UI_ItemUse : GComponent
	{
		public UI_Comp_ItemInfo m_comp_1;

		public const string URL = "ui://5krhxnalsuruj";

		public static UI_ItemUse CreateInstance()
		{
			return (UI_ItemUse)UIPackage.CreateObject("Bag","ItemUse");
		}

		public UI_ItemUse()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_comp_1 = (UI_Comp_ItemInfo)this.GetChildAt(0);
		}
	}
}