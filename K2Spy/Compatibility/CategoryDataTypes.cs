namespace K2Spy.Compatibility
{
    public static class CategoryDataTypes
    {
        public static SourceCode.Categories.Client.CategoryServer.dataType Convert(CategoryDataType categoryDataType)
        {
            return (SourceCode.Categories.Client.CategoryServer.dataType)(int)categoryDataType;
        }

        public static CategoryDataType Convert(SourceCode.Categories.Client.CategoryServer.dataType dataType)
        {
            return (CategoryDataType)(int)dataType;
        }
    }
}
