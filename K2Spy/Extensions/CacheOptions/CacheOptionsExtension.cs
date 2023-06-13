using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K2Spy.Extensions.CacheOptions
{
    internal class CacheOptionsExtension : Model.IOptionsExtension, Model.IOptionsTitleExtension
    {
        public string DisplayName => "Cache options";

        public string OptionsTitle => "Caching";

        public void CommitOptions(Control optionsControl)
        {
            ((CacheOptionsControl)optionsControl).Commit();
        }

        public Control CreateOptionsControl(K2SpyContext k2SpyContext)
        {
            return new CacheOptionsControl(k2SpyContext);
        }

        public bool IsOptionsControlDirty(Control optionsControl)
        {
            return ((CacheOptionsControl)optionsControl).IsDirty;
        }
    }
}
