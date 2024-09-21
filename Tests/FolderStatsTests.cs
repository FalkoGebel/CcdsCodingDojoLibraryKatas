using _03_FolderStats;
using FluentAssertions;

namespace Tests
{
    [TestClass]
    public class FolderStatsTests
    {
        private static readonly string _rootPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/FolderStatsKata/";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Directory.CreateDirectory(_rootPath);
            Directory.CreateDirectory(_rootPath + "Subfolder1");
            Directory.CreateDirectory(_rootPath + "Subfolder2");
            Directory.CreateDirectory(_rootPath + "Subfolder3");
            File.WriteAllText(_rootPath + "Subfolder1/EmptyFile1.txt", string.Empty);
            File.WriteAllText(_rootPath + "Subfolder1/EmptyFile2.txt", string.Empty);
            File.WriteAllText(_rootPath + "Subfolder1/EmptyFile3.txt", string.Empty);
            File.WriteAllText(_rootPath + "Subfolder1/TextFile1.txt", "This is a one liner.");
            File.WriteAllText(_rootPath + "Subfolder1/TextFile2.txt", "This is a two liner.\nReally!");
            File.WriteAllText(_rootPath + "Subfolder1/TextFile3b.txt", "This is a three liner.\nReally!\nI swear.");

            File.WriteAllText(_rootPath + "Subfolder2/EmptyFile1.txt", string.Empty);
            File.WriteAllText(_rootPath + "Subfolder2/emptyfile3.txt", string.Empty);
            File.WriteAllText(_rootPath + "Subfolder2/TextFile1.txt", "This is a one liner.");
            File.WriteAllText(_rootPath + "Subfolder2/TextFile2.txt", "This is a two liner.\nReally!");
            File.WriteAllText(_rootPath + "Subfolder2/TextFile4b.txt", "This is a 2? Byte file.");
            File.WriteAllText(_rootPath + "Subfolder2/LargeFile.txt", "This is a very large file…a really large file...large than the other files in the pot.\nI swear.");

            File.WriteAllText(_rootPath + "Subfolder3/TextFile1.txt", "This is a one liner.");
            File.WriteAllText(_rootPath + "Subfolder3/TextFile3.txt", "This is a three liner.\nReally!\nI swear.");
            File.WriteAllText(_rootPath + "Subfolder3/TextFile3b.txt", "This is a three liner.\nReally!\nI swear.");
            File.WriteAllText(_rootPath + "Subfolder3/TextFile4.txt", "This is a 23 Byte file.");
            File.WriteAllText(_rootPath + "Subfolder3/TextFile4b.txt", "This is a 2? Byte file.");
        }

        [TestMethod]
        public void New_Folder_Stats_Instance()
        {
            FolderStats folderStats = new();
            folderStats.Should().NotBeNull();
        }

        [TestMethod]
        public void New_Folder_Stats_Instance_has_waiting_status()
        {
            FolderStats folderStats = new();
            folderStats.Status.Should().Be(Statuses.Waiting);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void No_start_if_not_connected()
        {
            FolderStats folderStats = new();
            folderStats.Start();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void No_pause_if_not_running()
        {
            FolderStats folderStats = new();
            folderStats.Pause();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void No_resume_if_not_paused()
        {
            FolderStats folderStats = new();
            folderStats.Resume();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void No_stop_if_not_running_or_paused()
        {
            FolderStats folderStats = new();
            folderStats.Stop();
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("abc")]
        [DataRow("123")]
        [DataRow(@"C:\invalid_folder\")]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_connect_with_invalid_paths(string rootPath)
        {
            FolderStats folderStats = new();
            folderStats.Connect(rootPath);
        }

        [TestMethod]
        public void After_connect_has_connected_status_and_correct_root_path_and_correct_number_of_folders()
        {
            FolderStats folderStats = new();
            folderStats.Connect(_rootPath);
            folderStats.Status.Should().Be(Statuses.Connected);
            folderStats.RootPath.Should().Be(_rootPath);
            folderStats.Folders.Count().Should().Be(4);
            folderStats.Folders.Where(f => f.Depth == 0).Count().Should().Be(1);
            folderStats.Folders.Where(f => f.Depth == 1).Count().Should().Be(3);
            folderStats.Folders.Where(f => f.Depth == 2).Count().Should().Be(0);
        }

        [TestMethod]
        public void After_processing_example_rootpath_folder_has_number_of_files_and_total_bytes()
        {
            FolderStats folderStats = new();
            folderStats.Connect(_rootPath);
            folderStats.Start();
            while (folderStats.Status != Statuses.Finished)
                Thread.Sleep(100);
            Folder rootFolder = folderStats.Folders.Where(f => f.ParentPath == string.Empty).First();
            rootFolder.NumberOfFiles.Should().Be(17);
            rootFolder.TotalBytes.Should().Be(399);
        }

        [TestMethod]
        public void Test_using_progress_event()
        {
            FolderStats folderStats = new();
            folderStats.Connect(_rootPath);
            int counter = 0;
            folderStats.Progress += () => counter++;
            folderStats.Start();
            while (folderStats.Status != Statuses.Finished)
                Thread.Sleep(100);
            counter.Should().Be(4);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Directory.Delete(_rootPath, true);
        }
    }
}