using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Input;

namespace Trollkit
{
    public class HeadMenuBtn : Button
    {
        Boolean isActive = false;

        static HeadMenuBtn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeadMenuBtn), new
            FrameworkPropertyMetadata(typeof(HeadMenuBtn)));
        }

        protected override void OnClick()
        {
            HeadMenu parent = (HeadMenu)Parent;
            parent.setActive(this);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            //Canvas.SetLeft(this, 75);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            //Canvas.SetLeft(this, 30);
        }
    }

    public class HeadMenu : Canvas
    {
        static HeadMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeadMenu), new
            FrameworkPropertyMetadata(typeof(HeadMenu)));
        }

        public void setActive(HeadMenuBtn active)
        {
            UIElementCollection menuChildren = Children;

            foreach( UIElement child in menuChildren)
            {
                Canvas.SetLeft(child, 30);
            }
            Canvas.SetLeft(active, 75);
        }
    }
}
