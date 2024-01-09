using System.Diagnostics;
using System.IO;
using System.Windows;
using BinarySearch.Extension;
using BinarySearch.ViewModel;

namespace BinarySearch;

public partial class MainWindow : Window
{
    private readonly Config _config = new() { InputDirectory = "", InputSuffix = "", InputHex = "" };

    public MainWindow()
    {
        InitializeComponent();

        if (File.Exists("config.json"))
        {
            _config = System.Text.Json.JsonSerializer.Deserialize<Config>(File.ReadAllText("config.json")) ?? _config;
        }
        else
        {
            File.WriteAllText("config.json", System.Text.Json.JsonSerializer.Serialize(_config));
        }

        DataContext = new Data
        {
            Config = _config
        };
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        File.WriteAllText("config.json", System.Text.Json.JsonSerializer.Serialize(_config));
    }

    private async void Search(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_config.InputDirectory))
        {
            MessageBox.Show("Please select a directory to search.");
            return;
        }

        if (string.IsNullOrEmpty(_config.InputHex))
        {
            MessageBox.Show("Please enter a pattern to search for.");
            return;
        }

        if (_config.InputHex.Replace(" ", "").Length % 2 != 0)
        {
            MessageBox.Show("Please enter a valid pattern to search for.");
            return;
        }

        var strings = Directory.GetFiles(_config.InputDirectory, "*", SearchOption.AllDirectories)
            .Where(s => _config.InputSuffix.Split(';').Any(s.EndsWith)).ToArray();

        Progress.Maximum = strings.Length;
        Progress.Value = 0;
        Result.Items.Clear();

        foreach (var s in strings)
        {
            var content = _config.InputHex;

            var bytes = await File.ReadAllBytesAsync(s);

            if (Search(bytes, content.ToHexArray()))
            {
                Result.Items.Add(new ResultItem(s));
            }

            Progress.Value++;
        }
    }

    private static bool Search(IEnumerable<byte> bytes, byte[] content)
    {
        var count = 0;

        foreach (var t in bytes)
        {
            if (t == content[count])
            {
                count++;
                if (count == content.Length)
                {
                    return true;
                }
            }
            else
            {
                count = 0;
            }
        }

        return false;
    }

    private void OpenInExplorer(object sender, RoutedEventArgs e)
    {
        if (Result.SelectedItem is ResultItem item)
        {
            Process.Start("explorer.exe", $"/select, \"{item.Path}\"");
        }
    }
}