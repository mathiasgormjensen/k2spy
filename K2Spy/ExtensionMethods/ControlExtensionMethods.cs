using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.ExtensionMethods
{
    public enum OpenContextMenuStripContext
    {
        Mouse,
        Keyboard
    }

    public class OpenContextMenuStripEventArgs : EventArgs
    {
        public OpenContextMenuStripEventArgs(Point mouseScreenLocation)
            : this(OpenContextMenuStripContext.Mouse, mouseScreenLocation)
        {
        }

        public OpenContextMenuStripEventArgs(OpenContextMenuStripContext openContextMenuStripContext, Point? mouseScreenLocation = null)
        {
            this.Context = openContextMenuStripContext;
            this.MouseScreenLocation = mouseScreenLocation ?? Point.Empty; ;
        }

        public Point MouseScreenLocation { get; private set; }

        public OpenContextMenuStripContext Context { get; private set; }
    }

    public static class ControlExtensionMethods
    {
        private class BogusContextMenuStrip : System.Windows.Forms.ContextMenuStrip
        {
            private List<EventHandler<OpenContextMenuStripEventArgs>> m_Handlers = new List<EventHandler<OpenContextMenuStripEventArgs>>();
            private Control m_Source;
            private DateTime m_ContextMenuKeyTimestamp;
            private DateTime m_MouseClickTimestamp;

            public BogusContextMenuStrip(Control source)
            {
                this.m_Source = source;
                source.PreviewKeyDown += this.Source_PreviewKeyDown;
                source.MouseDown += this.Source_MouseDown;
                

                base.PreviewKeyDown += this.Source_PreviewKeyDown;
            }

            private void Source_MouseDown(object sender, MouseEventArgs e)
            {
                this.m_MouseClickTimestamp = DateTime.Now;
            }

            private void Source_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
            {
                //Console.WriteLine($"Source_PreviewKeyDown, {e.KeyValue}");
                if (e.KeyValue == 93)
                {
                    this.m_ContextMenuKeyTimestamp = DateTime.Now;
                }
            }

            public void Register(EventHandler<OpenContextMenuStripEventArgs> handler)
            {
                this.m_Handlers.Add(handler);
            }

            private static class ModalMenuFilter
            {
                private static System.Reflection.PropertyInfo m_ShowUnderlinesProperty;
                private static object m_Instance;

                static ModalMenuFilter()
                {
                    ModalMenuFilter.m_Instance = typeof(System.Windows.Forms.ToolStripManager).Assembly.GetType("System.Windows.Forms.ToolStripManager+ModalMenuFilter", true).InvokeMember("Instance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.GetProperty, null, null, null);
                    ModalMenuFilter.m_ShowUnderlinesProperty = ModalMenuFilter.m_Instance.GetType().GetProperty("ShowUnderlines");
                }

                public static bool ShowUnderlines
                {
                    get { return (bool)ModalMenuFilter.m_ShowUnderlinesProperty.GetValue(ModalMenuFilter.m_Instance); }
                    set { ModalMenuFilter.m_ShowUnderlinesProperty.SetValue(ModalMenuFilter.m_Instance, value); }
                }
            }

            protected override void OnOpening(CancelEventArgs e)
            {
                // TODO, we should be able to improve this
                DateTime now = DateTime.Now;
                bool byKeyboard = false;
                Console.WriteLine($"cms: {m_ContextMenuKeyTimestamp}");
                Console.WriteLine($"click: {m_MouseClickTimestamp}");
                Console.WriteLine($"now: {now}");
                if (ModalMenuFilter.ShowUnderlines)
                {
                    byKeyboard = true;
                    Console.WriteLine("SHOW UNDERLINES!!!");
                }
                else if (this.m_ContextMenuKeyTimestamp > this.m_MouseClickTimestamp)
                {
                    Console.WriteLine("Keyboard was pressed after mouse");
                    byKeyboard = this.m_ContextMenuKeyTimestamp.AddMilliseconds(300) > now;

                    if (!byKeyboard)
                    {
                        byKeyboard = ModalMenuFilter.ShowUnderlines;
                        if (byKeyboard)
                        {
                            Console.WriteLine("Showing underlines");
                        }
                        else
                        {
                            Console.WriteLine("Not showing underlines");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Keyboard was pressed recently");
                    }
                }
                else
                {
                    Console.WriteLine("Mouse was clicked after keyboard");
                }

                e.Cancel = true;

                OpenContextMenuStripEventArgs openContextMenuStripEventArgs = null;
                if (byKeyboard)
                    openContextMenuStripEventArgs = new OpenContextMenuStripEventArgs(OpenContextMenuStripContext.Keyboard);
                else
                    openContextMenuStripEventArgs = new OpenContextMenuStripEventArgs(OpenContextMenuStripContext.Mouse, Cursor.Position);

                this.m_Handlers.ForEach(key => key(this.m_Source, openContextMenuStripEventArgs));
            }

            protected override void Dispose(bool disposing)
            {
                this.m_Handlers.Clear();
                base.Dispose(disposing);
            }
        }

        public static void OnOpenContextMenuStrip(this System.Windows.Forms.Control that, EventHandler<OpenContextMenuStripEventArgs> handler)
        {
            BogusContextMenuStrip bogusContextMenuStrip = that.ContextMenuStrip as BogusContextMenuStrip;
            if (bogusContextMenuStrip == null)
            {
                bogusContextMenuStrip = new BogusContextMenuStrip(that);
                that.ContextMenuStrip = bogusContextMenuStrip;
            }

            bogusContextMenuStrip.Register(handler);
        }

        public static void ApplyDefaultFont(this System.Windows.Forms.Control that)
        {
            that.Font = Fonts.DefaultFont;
        }

        public static void SetDoubleBuffered(this System.Windows.Forms.Control that, bool state)
        {
            typeof(System.Windows.Forms.Control).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty, null, that, new object[] { state });
        }

        public static void InvokeIfRequired(this System.Windows.Forms.Control that, Action action)
        {
            if (that.InvokeRequired)
                that.Invoke(action);
            else
                action();
        }

        public static void BeginInvokeIfRequired(this System.Windows.Forms.Control that, Action action)
        {
            if (that.InvokeRequired)
                that.BeginInvoke(action);
            else
                action();
        }
    }
}
