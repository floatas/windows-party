using System;

namespace Testio.Shell
{
    public class ChangeViewMessage
    {
        public Type ViewModelType { get; set; }

        public ChangeViewMessage(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }
    }
}