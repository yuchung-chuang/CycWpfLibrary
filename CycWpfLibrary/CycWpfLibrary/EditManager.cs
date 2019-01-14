using CycWpfLibrary.Media;
using CycWpfLibrary.MVVM;
using System.Collections.Generic;
using System.Windows.Input;

namespace CycWpfLibrary
{
  /// <summary>
  /// 提供物件編輯功能的類別。
  /// </summary>
  /// <typeparam name="EditObjectType">要編輯的物件類</typeparam>
  public class EditManager
  {
    protected List<object> objList = new List<object>();
    protected int listIndex;
    protected int ListIndex
    {
      get => listIndex;
      set
      {
        listIndex = value;
        Object = objList[value];
      }
    }

    /// <summary>
    /// 初始化執行個體
    /// </summary>
    public EditManager()
    {
      UndoCommand = new RelayCommand(Undo, CanUndo);
      RedoCommand = new RelayCommand(Redo, CanRedo);
      EditCommand = new RelayCommand<object>(Edit, CanEdit);
    }

    /// <summary>
    /// 設定編輯物件
    /// </summary>
    public void Init(object iniObj)
    {
      objList.Clear();
      objList.Add(iniObj);
      ListIndex = 0;
      IsInitialized = true;
    }

    public bool IsInitialized { get; private set; } = false;

    public object Object { get; private set; }
    public ICommand UndoCommand { get; set; }
    public ICommand RedoCommand { get; set; }
    public ICommand EditCommand { get; set; }

    public void Undo() => ListIndex--;
    public bool CanUndo() => IsInitialized && ListIndex > 0;
    public void Redo() => ListIndex++;
    public bool CanRedo() => IsInitialized && ListIndex < objList.Count - 1;
    public void Edit(object newObj)
    {
      objList.RemoveRange(listIndex + 1, objList.Count - listIndex - 1);
      objList.Add(newObj);
      ListIndex++;
    }
    public virtual bool CanEdit(object parameter) => IsInitialized;
  }
}
