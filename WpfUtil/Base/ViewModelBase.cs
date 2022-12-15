using System.ComponentModel;

namespace WpfUtil;

public class VMBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
}

public class IdVMBase : VMBase
{
    public string Id { get; set; }
}

public class IdNameVMBase : IdVMBase
{
    public string Name { get; set; }
}