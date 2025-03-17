using TaskManagerApp.HomePage;

public class MainViewModelTests
{
    [TestMethod()]
    public async Task LoadDataAsync_CreatesDefaultLists_WhenFileNotFound()
    {
        var viewModel = new MainViewModel();
        var initialCount = viewModel.ListOfListCollection.Count;

        await Task.Delay(500); // Allow async operations to complete

        Assert.IsTrue(viewModel.ListOfListCollection.Count > initialCount);
    }

    [TestMethod()]
    public void AddTaskList_ShouldIncreaseCount_WhenValidNameProvided()
    {
        var viewModel = new MainViewModel();
        var initialCount = viewModel.ListOfListCollection.Count;

        viewModel.AddTaskList("Test List");

        Assert.AreEqual(initialCount + 1, viewModel.ListOfListCollection.Count);
    }

    [TestMethod()]
    public async Task SaveToFileAsync_ShouldCreateFile()
    {
        var viewModel = new MainViewModel();
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "TestFile.xml");

        await MainViewModel.SaveToFileAsync(viewModel.ListOfListCollection.ToList(), filePath);

        Assert.IsTrue(File.Exists(filePath));

        File.Delete(filePath);
    }
}