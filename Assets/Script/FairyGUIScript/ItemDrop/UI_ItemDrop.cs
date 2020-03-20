/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ItemDrop
{
	public partial class UI_ItemDrop : GComponent
	{
		public UI_ItemName m_itemName;

		public const string URL = "ui://yum1lb3neocd0";

		public static UI_ItemDrop CreateInstance()
		{
			return (UI_ItemDrop)UIPackage.CreateObject("ItemDrop","ItemDrop");
		}

		public UI_ItemDrop()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_itemName = (UI_ItemName)this.GetChildAt(0);
		}
	}
}