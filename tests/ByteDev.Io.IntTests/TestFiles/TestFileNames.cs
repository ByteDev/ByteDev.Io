namespace ByteDev.Io.IntTests.TestFiles
{
    internal static class TestFileNames
    {
        private const string BasePath = @"TestFiles\";

        public static string ExistingEmbeddedFile = "EmbeddedResource1.txt";
        public static string ExistingContentFile = "ContentFile1.txt";
        public static string NotExistingEmbeddedFile = "SomeFileNotExist.txt";

        internal static class Binary
        {
            public const string Png = BasePath + "icon.png";
        }
    }
}