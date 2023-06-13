using System;

namespace K2Spy.ExtensionMethods
{
    public static class ListViewItemExtensionMethods
    {
        public static void ImageIndexFrom(this System.Windows.Forms.ListViewItem that, K2SpyTreeNode imageKeySource)
        {
            EventHandler imageKeyChangedHandler = (sender, e) =>
            {
                System.Windows.Forms.ImageList imageList = imageKeySource.TreeView?.ImageList ?? that.ImageList;
                int index = 0;
                if (imageList?.Images.ContainsKey(imageKeySource.ImageKey) == true)
                    index = imageList.Images.IndexOfKey(imageKeySource.ImageKey);
                that.ImageIndex = index;
                that.ListView?.Invalidate();
            };
            imageKeySource.ImageKeyChanged += imageKeyChangedHandler;
            imageKeyChangedHandler(imageKeySource, EventArgs.Empty);
            //return disposer;
        }
    }
}
