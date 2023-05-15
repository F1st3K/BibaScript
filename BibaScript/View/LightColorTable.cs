using System.Drawing;
using System.Windows.Forms;

namespace View
{
    class LightColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(150, 190, 220);

        public override Color MenuItemBorder => Color.FromArgb(160, 160, 160);

        public override Color MenuItemPressedGradientBegin => Color.White;

        public override Color MenuItemPressedGradientEnd => Color.White;

        public override Color MenuItemSelectedGradientBegin => Color.White;

        public override Color MenuItemSelectedGradientEnd => Color.White;

        public override Color MenuBorder => Color.White;

        public override Color MenuItemPressedGradientMiddle => Color.White;

        public override Color ToolStripDropDownBackground => Color.White;

        public override Color MenuStripGradientBegin => Color.White;

        public override Color MenuStripGradientEnd => Color.White;

        public override Color CheckBackground => Color.White;

        public override Color CheckPressedBackground => Color.White;

        public override Color CheckSelectedBackground => Color.White;

        public override Color ImageMarginGradientBegin => Color.White;

        public override Color ImageMarginGradientEnd => Color.White;

        public override Color ImageMarginGradientMiddle => Color.White;
    }
}
