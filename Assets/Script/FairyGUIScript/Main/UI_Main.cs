/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Main
{
	public partial class UI_Main : GComponent
	{
		public UI_Bar_Exp m_bar_exp;
		public UI_Bar_Blood m_bar_blood;
		public UI_Bar_Blue m_bar_blue;
		public GComponent m_btn1;
		public UI_Btn_Main m_btn2;
		public UI_Btn_Main m_btn3;
		public UI_Btn_Main m_btn4;
		public UI_Btn_Main m_btnLeft;
		public UI_Btn_Main m_btnRight;
		public UI_Btn_Main m_btnWeapon;
		public GList m_listSkill;
		public GList m_listWeapon;
		public GGroup m_group_down;
		public GGraph m_graph_bg;
		public GButton m_btnLv1;
		public GButton m_btnLv2;
		public GButton m_btnLv3;
		public GTextField m_txtLv;
		public UI_Bar_Blood m_barEnemy;
		public GTextField m_txtEnemyName;
		public GGroup m_group_top;
		public GGraph m_graph_bgDied;
		public GButton m_btnRespawn;
		public GTextField m_txtDied;

		public const string URL = "ui://3lyppwh6hsc60";

		public static UI_Main CreateInstance()
		{
			return (UI_Main)UIPackage.CreateObject("Main","Main");
		}

		public UI_Main()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_bar_exp = (UI_Bar_Exp)this.GetChildAt(0);
			m_bar_blood = (UI_Bar_Blood)this.GetChildAt(1);
			m_bar_blue = (UI_Bar_Blue)this.GetChildAt(2);
			m_btn1 = (GComponent)this.GetChildAt(3);
			m_btn2 = (UI_Btn_Main)this.GetChildAt(4);
			m_btn3 = (UI_Btn_Main)this.GetChildAt(5);
			m_btn4 = (UI_Btn_Main)this.GetChildAt(6);
			m_btnLeft = (UI_Btn_Main)this.GetChildAt(7);
			m_btnRight = (UI_Btn_Main)this.GetChildAt(8);
			m_btnWeapon = (UI_Btn_Main)this.GetChildAt(9);
			m_listSkill = (GList)this.GetChildAt(10);
			m_listWeapon = (GList)this.GetChildAt(11);
			m_group_down = (GGroup)this.GetChildAt(12);
			m_graph_bg = (GGraph)this.GetChildAt(13);
			m_btnLv1 = (GButton)this.GetChildAt(14);
			m_btnLv2 = (GButton)this.GetChildAt(15);
			m_btnLv3 = (GButton)this.GetChildAt(16);
			m_txtLv = (GTextField)this.GetChildAt(17);
			m_barEnemy = (UI_Bar_Blood)this.GetChildAt(18);
			m_txtEnemyName = (GTextField)this.GetChildAt(19);
			m_group_top = (GGroup)this.GetChildAt(20);
			m_graph_bgDied = (GGraph)this.GetChildAt(21);
			m_btnRespawn = (GButton)this.GetChildAt(22);
			m_txtDied = (GTextField)this.GetChildAt(23);
		}
	}
}