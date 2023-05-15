using System.Drawing;
using System.Windows.Forms;

namespace View
{
    class DarkColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(50, 90, 120);

        public override Color MenuItemBorder => Color.FromArgb(160, 160, 160);

        public override Color MenuItemPressedGradientBegin => Color.DimGray;

        public override Color MenuItemPressedGradientEnd => Color.DimGray;

        public override Color MenuItemSelectedGradientBegin => Color.DimGray;

        public override Color MenuItemSelectedGradientEnd => Color.DimGray;

        public override Color MenuBorder => Color.DimGray;

        public override Color MenuItemPressedGradientMiddle => Color.DimGray;

        public override Color ToolStripDropDownBackground => Color.DimGray;

        public override Color MenuStripGradientBegin => Color.DimGray;

        public override Color MenuStripGradientEnd => Color.DimGray;

        public override Color CheckBackground => Color.DimGray;

        public override Color CheckPressedBackground => Color.DimGray;

        public override Color CheckSelectedBackground => Color.DimGray;

        public override Color ImageMarginGradientBegin => Color.DimGray;

        public override Color ImageMarginGradientEnd => Color.DimGray;

        public override Color ImageMarginGradientMiddle => Color.DimGray;
    }
}
