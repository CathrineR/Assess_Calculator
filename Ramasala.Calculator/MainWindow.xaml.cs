using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ramasala.Calculator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private decimal _currentValue = 0;
    private decimal _memoryValue = 0;
    private string _pendingOperation = "";
    private bool _newNumberEntered = true;
    private readonly CalculatorViewModel _dataContext;

    public MainWindow()
    {
        InitializeComponent();

        _dataContext = new CalculatorViewModel();
        DataContext = _dataContext;
    }

    private void DigitButton_Click(object sender, RoutedEventArgs e)
    {
        string digit = (sender as Button).Content.ToString();
        if (_newNumberEntered)
        {
            _dataContext.DisplayText = digit;
            _newNumberEntered = false;
        }
        else
        {
            _dataContext.DisplayText += digit;
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        DisplayTextBlock.Text = _dataContext.DisplayText;
    }

    private void OperationButton_Click(object sender, RoutedEventArgs e)
    {
        string operation = (sender as Button).Content.ToString();
        if (!string.IsNullOrEmpty(_pendingOperation))
        {
            PerformOperation();
        }
        else
        {
            _currentValue = decimal.Parse(_dataContext.DisplayText);
        }
        _pendingOperation = operation;
        _dataContext.DisplayText += $" {operation} ";
        _newNumberEntered = true;
        UpdateDisplay();
    }

    private void MemorySubtractButton_Click(object sender, RoutedEventArgs e)
    {
        _memoryValue -= decimal.Parse(_dataContext.DisplayText);
    }

    private void MemoryAddButton_Click(object sender, RoutedEventArgs e)
    {
        _memoryValue += decimal.Parse(_dataContext.DisplayText);
    }

    private void EqualsButton_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(_pendingOperation))
        {
            PerformOperation();
            _pendingOperation = "";
        }

        UpdateDisplay();
    }

    private void ClearEntryButton_Click(object sender, RoutedEventArgs e)
    {
        _dataContext.DisplayText = "0";
        _newNumberEntered = true;
        UpdateDisplay();
    }

    private void ClearAllButton_Click(object sender, RoutedEventArgs e)
    {
        _dataContext.DisplayText = "0";
        _currentValue = 0;
        _pendingOperation = "";
        _newNumberEntered = true;
        UpdateDisplay();
    }

    private void DecimalButton_Click(object sender, RoutedEventArgs e)
    {
        if (!_dataContext.DisplayText.Contains('.'))
        {
            _dataContext.DisplayText += ".";
            UpdateDisplay();
        }
    }

    private void MemoryRecallButton_Click(object sender, RoutedEventArgs e)
    {
        _dataContext.DisplayText = _memoryValue.ToString();
        _newNumberEntered = true;
        UpdateDisplay();
    }

    private void HistoryButton_Click(object sender, RoutedEventArgs e)
    {
        if (HistoryListBox.Visibility == Visibility.Visible)
        {
            HistoryListBox.Visibility = Visibility.Collapsed;
        }
        else
        {
            HistoryListBox.Visibility = Visibility.Visible;
        }
    }

    private void PerformOperation()
    {
        decimal secondOperand = decimal.Parse(_dataContext.DisplayText.Split().Last());
        var update =  $"{_currentValue} {_pendingOperation} {_dataContext.DisplayText} = ";

        switch (_pendingOperation)
        {
            case "+":
                _currentValue += secondOperand;
                break;
            case "-":
                _currentValue -= secondOperand;
                break;
            case "*":
                _currentValue *= secondOperand;
                break;
            case "/":
                _currentValue /= secondOperand;
                break;
        }

        _dataContext.DisplayText = $"{_currentValue}";
        
        update += $"{_currentValue}";

        UpdateHistory(update);
        _pendingOperation = "";
        _newNumberEntered = true;
    }


    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            EqualsButton_Click(null, null);
        }
        else if (e.Key == Key.Back)
        {
            ClearEntryButton_Click(null, null);
        }
        else if (e.Key == Key.Delete)
        {
            ClearAllButton_Click(null, null);
        }
        else if (e.Key == Key.OemPeriod)
        {
            DecimalButton_Click(null, null);
        }
        else if (e.Key == Key.Add)
        {
            DoOperation("+");
        }
        else if (e.Key == Key.Subtract)
        {
            DoOperation("-");
        }
        else if (e.Key == Key.Multiply)
        {
            DoOperation("*");
        }
        else if (e.Key == Key.Divide)
        {
            DoOperation("/");
        }
        else if (e.Key == Key.M)
        {
            MemoryAddButton_Click(null, null);
        }
        else if (e.Key == Key.N)
        {
            MemorySubtractButton_Click(null, null);
        }
        else if (e.Key == Key.R)
        {
            MemoryRecallButton_Click(null, null);
        }
        else if (e.Key == Key.H)
        {
            HistoryButton_Click(null, null);
        }
        else if (e.Key >= Key.D0 && e.Key <= Key.D9)
        {
            int digitIndex = (int)e.Key - (int)Key.D0;
            SimulateDigitButtonClick(digitIndex);
        }
        else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
        {
            int digitIndex = (int)e.Key - (int)Key.NumPad0;
            SimulateDigitButtonClick(digitIndex);
        }
    }

    private void DoOperation(string operation)
    {
        if (!string.IsNullOrEmpty(operation))
        {
            _pendingOperation = operation;
            PerformOperation();
        }
        else
        {
            _currentValue = decimal.Parse(_dataContext.DisplayText);
        }

        _pendingOperation = operation;
        _newNumberEntered = true;
    }

    private void SimulateDigitButtonClick(int digitIndex)
    {
        string digit = digitIndex.ToString();
        if (_newNumberEntered)
        {
            _dataContext.DisplayText = digit;
            _newNumberEntered = false;
        }
        else
        {
            _dataContext.DisplayText += digit;
        }
    }

    private void UpdateHistory(string update)
    {
        _dataContext.History.Add(update);
    }
}
