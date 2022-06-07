﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfMvvmApp.ViewModels
{
    /// <summary>
    /// ViewModel과 View를 Glue하기 위한 클래스
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> execute; // 실행처리를 위한 generic
        private readonly Predicate<T> canExecute;   // 실행여부 판단 generic   , <T> : c++에서 Template할 때 함

        // 실행여부에 따라 이벤트 추가&삭제 이벤트 핸들러
        public event EventHandler CanExecuteChanged 
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }

        /// <summary>
        /// execute만 파라미터 받는 생성자
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<T> execute) : this(execute, null) {  }

        /// <summary>
        /// execute, canExecute 파라미터로 받는 생성자
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }
    }
}
