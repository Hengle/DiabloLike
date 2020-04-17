/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ItemDrop
{
	public partial class UI_Comp_txt : GComponent
	{
		public GTextField m_txt_num;
		public Transition m_txtTransition;

		public const string URL = "ui://yum1lb3nhsc63n";

		public static UI_Comp_txt CreateInstance()
		{
			return (UI_Comp_txt)UIPackage.CreateObject("ItemDrop","Comp_txt");
		}

		public UI_Comp_txt()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_txt_num = (GTextField)this.GetChildAt(0);
			m_txtTransition = this.GetTransitionAt(0);
		}
	}
}