/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Bag
{
	public partial class UI_Comp_Item : GComponent
	{
		public GLoader m_load_quality;
		public GLoader m_load_item;

		public const string URL = "ui://5krhxnalsurui";

		public static UI_Comp_Item CreateInstance()
		{
			return (UI_Comp_Item)UIPackage.CreateObject("Bag","Comp_Item");
		}

		public UI_Comp_Item()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_load_quality = (GLoader)this.GetChildAt(0);
			m_load_item = (GLoader)this.GetChildAt(1);
		}
	}
}