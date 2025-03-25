using TaskManagerApp.HomePage;

namespace TaskManagerAppTests.HomePage
{
    [TestClass()]
    public class MainViewModelTests
    {
        [TestMethod()]
        public async Task LoadDataAsync_CreatesDefaultLists_WhenFileNotFound()
        {
            MainViewModel viewModel = new MainViewModel();
            var initialCount = viewModel.ListOfLists.Count;

            await Task.Delay(500);

            Assert.IsTrue(viewModel.ListOfLists.Count > initialCount, "The ListOfLists count should have increased.");
        }

        [TestMethod()]
        public void AddTaskList_ShouldIncreaseCount_WhenValidNameProvided()
        {
            MainViewModel viewModel = new MainViewModel();
            var initialCount = viewModel.ListOfLists.Count;

            viewModel.AddTaskList("Test List");

            Assert.AreEqual(initialCount + 1, viewModel.ListOfLists.Count, "Adding a new task list should increase the count by 1.");
        }

        [TestMethod()]
        public async Task SaveToFileAsync_ShouldCreateFile()
        {
            MainViewModel viewModel = new MainViewModel();
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "TestFile.xml");

            await MainViewModel.SaveToFileAsync(viewModel.ListOfLists.ToList(), filePath);

            Assert.IsTrue(File.Exists(filePath));

            File.Delete(filePath);
        }
    }
}