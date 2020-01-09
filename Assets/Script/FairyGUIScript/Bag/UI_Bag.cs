/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Bag
{
	public partial class UI_Bag : GComponent
	{
		public GGraph m_gra_bg;
		public GImage m_n0;
		public GList m_list_item;
		public UI_Btn_Close m_btn_close;
		public GImage m_n3;
		public UI_Comp_Item m_head;
		public UI_Comp_Item m_neck;
		public UI_Comp_Item m_chest;
		public UI_Comp_Item m_yao;
		public UI_Comp_Item m_leg;
		public UI_Comp_Item m_shoe;
		public UI_Comp_Item m_weapon;
		public UI_Comp_Item m_ring_1;
		public UI_Comp_Item m_ring_2;
		public GGroup m_gro_center;

		public const string URL = "ui://5krhxnallwcn0";

		public static UI_Bag CreateInstance()
		{
			return (UI_Bag)UIPackage.CreateObject("Bag","Bag");
		}

		public UI_Bag()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_gra_bg = (GGraph)this.GetChildAt(0);
			m_n0 = (GImage)this.GetChildAt(1);
			m_list_item = (GList)this.GetChildAt(2);
			m_btn_close = (UI_Btn_Close)this.GetChildAt(3);
			m_n3 = (GImage)this.GetChildAt(4);
			m_head = (UI_Comp_Item)this.GetChildAt(5);
			m_neck = (UI_Comp_Item)this.GetChildAt(6);
			m_chest = (UI_Comp_Item)this.GetChildAt(7);
			m_yao = (UI_Comp_Item)this.GetChildAt(8);
			m_leg = (UI_Comp_Item)this.GetChildAt(9);
			m_shoe = (UI_Comp_Item)this.GetChildAt(10);
			m_weapon = (UI_Comp_Item)this.GetChildAt(11);
			m_ring_1 = (UI_Comp_Item)this.GetChildAt(12);
			m_ring_2 = (UI_Comp_Item)this.GetChildAt(13);
			m_gro_center = (GGroup)this.GetChildAt(14);
		}
	}
}