using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace WinDroid_Universal_HTC_Toolkit.Models
{
    public sealed class LogModel : INotifyPropertyChanged
    {
        private readonly StringBuilder _LogBuilder = new StringBuilder();

        public string LogText
        {
            get { return _LogBuilder.ToString(); }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged Members

        public void AddLogItem(string text, string tag)
        {
            _LogBuilder.AppendFormat("{0}[{1}]:{2}\n", DateTime.Now.ToLongTimeString(), tag.ToUpperInvariant(), text);
            OnPropertyChanged("LogText");
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}