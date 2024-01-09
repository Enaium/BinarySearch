using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BinarySearch.ViewModel;

public class Config : INotifyPropertyChanged
{
    private readonly string _inputDirectory = null!;
    private readonly string _inputSuffix = null!;
    private readonly string _inputHex = null!;

    public required string InputDirectory
    {
        get => _inputDirectory;
        init
        {
            _inputDirectory = value;
            OnPropertyChanged(() => InputDirectory);
        }
    }

    public required string InputSuffix
    {
        get => _inputSuffix;
        init
        {
            _inputSuffix = value;
            OnPropertyChanged(() => InputSuffix);
        }
    }

    public required string InputHex
    {
        get => _inputHex;
        init
        {
            _inputHex = value;
            OnPropertyChanged(() => InputHex);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OnPropertyChanged<T>(Expression<Func<T>> propExpr)
    {
        var prop = (PropertyInfo)((MemberExpression)propExpr.Body).Member;
        OnPropertyChanged(prop.Name);
    }
}