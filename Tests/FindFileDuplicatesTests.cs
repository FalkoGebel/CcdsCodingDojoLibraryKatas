namespace Tests
{
    [TestClass]
    public class FindFileDuplicatesTests
    {
        private static string _folderpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/FindFileDuplicatesKata/";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Directory.CreateDirectory(_folderpath);
            Directory.CreateDirectory(_folderpath + "Subfolder1");
            Directory.CreateDirectory(_folderpath + "Subfolder2");
            Directory.CreateDirectory(_folderpath + "Subfolder3");
            File.WriteAllText(_folderpath + "Subfolder1/file1.txt", "Text to add to the file");
            File.WriteAllText(_folderpath + "Subfolder1/file2.txt", "Text to add to the file");
            File.WriteAllText(_folderpath + "Subfolder1/file3.txt", "Text to add to the file");
            File.WriteAllText(_folderpath + "Subfolder2/file1.txt", "Text to add to the file");
            File.WriteAllText(_folderpath + "Subfolder2/file2.txt", "Text to add to the file");
            File.WriteAllText(_folderpath + "Subfolder2/file3.txt", "Text to add to the file");
            File.WriteAllText(_folderpath + "Subfolder3/file1.txt", "Text to add to the file");
            File.WriteAllText(_folderpath + "Subfolder3/file2.txt", "Text to add to the file");
            File.WriteAllText(_folderpath + "Subfolder3/file3.txt", "Text to add to the file");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Directory.Delete(_folderpath, true);
        }
    }
}