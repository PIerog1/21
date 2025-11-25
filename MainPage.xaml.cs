using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
namespace MauiApp6
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new TaskViewModel();
        }
    }

    public class TaskViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TaskItem> Tasks { get; } = new();

        private string _taskTitle = "";
        public string TaskTitle
        {
            get => _taskTitle;
            set { _taskTitle = value; OnPropertyChanged(); }
        }

        public int TaskCount => Tasks.Count;

        public ICommand AddTaskCommand { get; }
        public ICommand ToggleCompletedCommand { get; }

        public TaskViewModel()
        {
            AddTaskCommand = new RelayCommand(_ => AddTask());
            ToggleCompletedCommand = new RelayCommand(OnToggleCompleted);
        }

        private void AddTask()
        {
            if (!string.IsNullOrWhiteSpace(TaskTitle))
            {
                Tasks.Add(new TaskItem { Title = TaskTitle });
                TaskTitle = "";
                OnPropertyChanged(nameof(TaskCount));
            }
        }

        private void OnToggleCompleted(object? param)
        {
            if (param is TaskItem task)
                task.IsCompleted = !task.IsCompleted;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public class TaskItem : INotifyPropertyChanged
    {
        private string _title = "";
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }
        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set { _isCompleted = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        public RelayCommand(Action<object?> execute) => _execute = execute;
        public bool CanExecute(object? _) => true;
        public void Execute(object? param) => _execute(param);
        public event EventHandler? CanExecuteChanged;
    }
}