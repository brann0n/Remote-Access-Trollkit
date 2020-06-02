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
        static HeadMenuBtn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeadMenuBtn), new
            FrameworkPropertyMetadata(typeof(HeadMenuBtn)));
        }

        protected override void OnClick()
        {
            HeadMenu parent = (HeadMenu)Parent;
            parent.setActive(this);

            ContentControl trollView = (ContentControl)this.FindName("trollView");  
            trollView.Content = new WindowsTrolls();
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


    public class CloseBtn : Button
    {
        static CloseBtn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseBtn), new
            FrameworkPropertyMetadata(typeof(CloseBtn)));
        }

        protected override void OnClick()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }

    public class MinimizeBtn : Button
    {
        static MinimizeBtn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MinimizeBtn), new
            FrameworkPropertyMetadata(typeof(MinimizeBtn)));
        }

        protected override void OnClick()
        {
            WindowCollection windows = Application.Current.Windows;
            
            foreach(Window window in windows)
            {
                window.WindowState = WindowState.Minimized;
            }
        }
    }
}
