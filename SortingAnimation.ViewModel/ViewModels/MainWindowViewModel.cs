using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using SortingAnimation.ViewModel.Enums;
using SortingAnimation.ViewModel.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System.Threading.Tasks;
using SortingAnimation.Model.Models;

namespace SortingAnimation.ViewModel.ViewModels
{
    /// <summary>
    ///     Класс ViewModel главного окна
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private bool _isAscending = true;
        private int _sequenceSize = 10;

        private int _playingSpeed = 10;

        private bool _playingBack = false;
        private bool _playingForward = false;

        public int ErrorsCount;

        private int[] _generateArray;

        private LinkedList<int[]>? _linkedList;
        private LinkedListNode<int[]>? _currentLinkedListNode;


        private SortTypeEnum _sortType = SortTypeEnum.BubbleSort;
        
        public ObservableCollection<RectItem> _rectCollection = new();

        public MainWindowViewModel()
        {
            GenerateCommand = new RelayCommand(OnGenerateCommandExecute, OnGenerateCommandCanExecute);
            SortCommand = new RelayCommand(OnSortCommandExecute, OnSortCommandCanExecute);
            PreviousSequenceCommand = new RelayCommand(OnPreviousSequenceCommandExecute, OnPreviousSequenceCommandCanExecute);
            NextSequenceCommand = new RelayCommand(OnNextSequenceCommandExecute, OnNextSequenceCommandCanExecute);
            PauseCommand = new RelayCommand(OnPauseCommandExecute, OnPauseCommandCanExecute);
            PlayBackCommand = new RelayCommand(OnPlayBackCommandExecute, OnPreviousSequenceCommandCanExecute);
            PlayForwardCommand = new RelayCommand(OnPlayForwardCommandExecute, OnNextSequenceCommandCanExecute);
            ResetCommand = new RelayCommand(OnResetCommandExecute, OnResetCommandCanExecute);
        }

        public int NumberOfSteps => LinkedList != null ? LinkedList.Count : 0;

        public LinkedListNode<int[]>? CurrentLinkedListNode
        {
            get => _currentLinkedListNode;
            set => OnCurrentLinkedListNodeChanged(CurrentLinkedListNode, value);
        }

        public SortTypeEnum SortType
        {
            get => _sortType;
            set => OnSortTypeChanged(SortType, value);
        }

        public bool IsAscending
        {
            get => _isAscending;
            set => OnIsAscendingChanged(IsAscending, value);
        }
        
        public int PlayingSpeed
        {
            get => _playingSpeed;
            set => OnPlayingSpeedChanged(value);
        }
        
        public bool IsStill
        {
            get => !PlayingForward && !PlayingBack;
        }

        public int SequenceSize
        {
            get => _sequenceSize;
            set => OnSequenceSizeChanged(SequenceSize, value);
        }

        public bool PlayingForward
        {
            get => _playingForward;
            set => OnPlayingForwardChanged(value);
        }

        public bool PlayingBack
        {
            get => _playingBack;
            set => OnPlayingBackChanged(value);
        }

        public ObservableCollection<RectItem> RectCollection
        {
            get => _rectCollection;
            set => OnRectCollectionChanged(RectCollection, value);
        }

        public RelayCommand GenerateCommand { get; }
        public RelayCommand PreviousSequenceCommand { get; }
        public RelayCommand NextSequenceCommand { get; }
        public RelayCommand PlayBackCommand { get; }
        public RelayCommand ResetCommand { get; }
        public RelayCommand PauseCommand { get; }
        public RelayCommand PlayForwardCommand { get; }
        public RelayCommand SortCommand { get; }

        public LinkedList<int[]> LinkedList
        {
            get => _linkedList;
            set => OnLinkedListChanged(LinkedList, value);
        }

        public string Error { get; }

        /// <summary>
        ///     Метод проверки изменяемых значений
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                var result = columnName switch
                {
                    nameof(SequenceSize) when SequenceSize is <= 0 or > 100 => "Введите корректное значение",
                    _ => string.Empty
                };

                return result;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnSortTypeChanged(SortTypeEnum oldValue, SortTypeEnum newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            _sortType = newValue;

            OnPropertyChanged(nameof(SortType));
        }

        private void OnPlayingForwardChanged(bool value)
        {
            _playingForward = value;
            
            OnPropertyChanged(nameof(PlayingForward));
            OnPropertyChanged(nameof(IsStill));
            CommandManager.InvalidateRequerySuggested();
        }

        private void OnPlayingSpeedChanged(int value)
        {
            _playingSpeed = value;

            OnPropertyChanged(nameof(PlayingSpeed));
        }

        private void OnPlayingBackChanged(bool value)
        {
            _playingBack = value;
            OnPropertyChanged(nameof(PlayingBack));
            OnPropertyChanged(nameof(IsStill));
            CommandManager.InvalidateRequerySuggested();
        }
        
        private void OnIsAscendingChanged(bool oldValue, bool newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            _isAscending = newValue;

            OnPropertyChanged(nameof(IsAscending));
        }

        private void OnLinkedListChanged(LinkedList<int[]> oldValue, LinkedList<int[]> newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            _linkedList = newValue;

            OnPropertyChanged(nameof(LinkedList));
            OnPropertyChanged(nameof(NumberOfSteps));
        }

        private bool OnPreviousSequenceCommandCanExecute(object arg)
        {
            return IsStill && CurrentLinkedListNode?.Previous != null;
        }

        private bool OnResetCommandCanExecute(object arg)
        {
            return IsStill && CurrentLinkedListNode?.Previous != null;
        }

        private bool OnPauseCommandCanExecute(object arg)
        {
            return !IsStill && (CurrentLinkedListNode?.Previous != null && CurrentLinkedListNode?.Next != null);
        }

        private void OnPauseCommandExecute(object obj)
        {
            PlayingBack = false;
            PlayingForward = false;
            RenderCanvas();
        }

        private void OnPreviousSequenceCommandExecute(object obj)
        {
            CurrentLinkedListNode = CurrentLinkedListNode?.Previous;
            RenderCanvas();
        }
        
        private bool OnSortCommandCanExecute(object arg)
        {
            return CurrentLinkedListNode?.Value != null && IsStill;
        }

        private void OnSortCommandExecute(object obj)
        {
            LinkedList = SortType switch
            {
                SortTypeEnum.BubbleSort => SortingAlgoritgms.BubbleSort(_generateArray, IsAscending),
                SortTypeEnum.CycleSort => SortingAlgoritgms.CycleSort(_generateArray, IsAscending),
                SortTypeEnum.GnomeSort => SortingAlgoritgms.GnomeSort(_generateArray, IsAscending),
                SortTypeEnum.HeapSort => SortingAlgoritgms.HeapSort(_generateArray, IsAscending),
                SortTypeEnum.InsertionSort => SortingAlgoritgms.InsertionSort(_generateArray, IsAscending),
                SortTypeEnum.QuickSort => SortingAlgoritgms.QuickSort(_generateArray, IsAscending),
                SortTypeEnum.MergeSort => SortingAlgoritgms.MergeSort(_generateArray, IsAscending),
                _ => throw new ArgumentOutOfRangeException()
            };

            CurrentLinkedListNode = LinkedList?.Last;
            RenderCanvas();
        }

        private bool OnNextSequenceCommandCanExecute(object arg)
        {
            return IsStill && CurrentLinkedListNode?.Next != null;
        }

        private void OnNextSequenceCommandExecute(object obj)
        {
            CurrentLinkedListNode = CurrentLinkedListNode?.Next;
            RenderCanvas();
        }
        
        async private void OnPlayBackCommandExecute(object obj)
        {
            PlayingBack = true;
            PlayingForward = false;
            while (CurrentLinkedListNode?.Previous != null && _playingBack)
            {
                CurrentLinkedListNode = CurrentLinkedListNode.Previous;
                RenderCanvas();
                await Task.Delay(1000 / PlayingSpeed);
            }
            PlayingBack = false;
            PlayingForward = false;
        }

        private void OnResetCommandExecute(object obj)
        {
            while (CurrentLinkedListNode?.Previous != null)
            {
                CurrentLinkedListNode = CurrentLinkedListNode.Previous;
            }
            RenderCanvas();
        }

        async private void OnPlayForwardCommandExecute(object obj)
        {
            PlayingBack = false;
            PlayingForward = true;
            while (CurrentLinkedListNode?.Next != null && _playingForward)
            {
                CurrentLinkedListNode = CurrentLinkedListNode.Next;
                RenderCanvas();
                await Task.Delay(1000 / PlayingSpeed);
            }
            PlayingBack = false;
            PlayingForward = false;
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool OnGenerateCommandCanExecute(object arg)
        {
            return IsStill && string.IsNullOrEmpty(Error) && ErrorsCount == 0;
        }

        private void OnGenerateCommandExecute(object obj)
        {
            _generateArray = new int[SequenceSize];

            var random = new Random();

            for (var index = 0; index < _generateArray.Length; index++)
            {
                _generateArray[index] = random.Next(1, 100);
            }

            LinkedList = null;

            CurrentLinkedListNode = new LinkedListNode<int[]>(_generateArray);

            RenderCanvas();
        }

        private void OnCurrentLinkedListNodeChanged(LinkedListNode<int[]>? oldValue, LinkedListNode<int[]>? newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            _currentLinkedListNode = newValue;

            OnPropertyChanged(nameof(CurrentLinkedListNode));
        }

        private void OnSequenceSizeChanged(int oldValue, int newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            _sequenceSize = newValue;

            OnPropertyChanged(nameof(SequenceSize));
        }

        private void OnRectCollectionChanged(ObservableCollection<RectItem> oldValue, ObservableCollection<RectItem> newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            _rectCollection = newValue;

            OnPropertyChanged(nameof(RectCollection));
        }


        /// <summary>
        ///     Метод отрисовки графики
        /// </summary>
        private void RenderCanvas()
        {
            if (CurrentLinkedListNode == null)
            {
                return;
            }
            int[] array = CurrentLinkedListNode.Value;
            RectCollection.Clear();
            double canvasWidth = 100;
            double canvasHeight = 100;
            int max = array.Max();
            double gap = canvasWidth / array.Length / 5;
            for (var i = 0; i < array.Length; i++)
            {
                RectItem rec = new RectItem()
                {
                    Width = canvasWidth / array.Length - gap,
                    Height = array[i],
                    Y = canvasHeight - array[i],
                    X = (canvasWidth / array.Length) * i,
                };
                RectCollection.Add(rec);
            }
        }
    }
}