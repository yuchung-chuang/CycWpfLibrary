using CycWpfLibrary;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace CycWpfLibrary
{
  /// <summary>
  /// 提供物件編輯功能的類別。
  /// </summary>
  /// <typeparam name="EditObjectType">要編輯的物件類</typeparam>
  public class EditManager
  {
    private List<object> objList = new List<object>();
    private int listIndex;
    public int ListIndex
    {
      get => listIndex;
      set
      {
        listIndex = value;
        Object = objList[value];
      }
    }

    public event Action ObjectChanged;
    public void OnObjgectChanged()
    {
      ObjectChanged?.Invoke();
    }

    private object mObject;
    public object Object
    {
      get => mObject;
      private set
      {
        mObject = value;
        OnObjgectChanged();
      }
    }

    public bool IsInitialized { get; private set; } = false;

    /// <summary>
    /// 初始化執行個體
    /// </summary>
    public EditManager()
    {
      UndoCommand = new RelayCommand(Undo, CanUndo);
      RedoCommand = new RelayCommand(Redo, CanRedo);
      EditCommand = new RelayCommand<ICloneable>(Edit, CanEdit);
    }

    /// <summary>
    /// 設定編輯物件
    /// </summary>
    public void Init(ICloneable iniObj)
    {
      objList.Clear();
      objList.Add(iniObj.Clone());
      ListIndex = 0;
      IsInitialized = true;
    }

    public ICommand UndoCommand { get; set; }
    public ICommand RedoCommand { get; set; }
    public ICommand EditCommand { get; set; }

    public void Undo() => ListIndex--;
    public bool CanUndo() => IsInitialized && ListIndex > 0;
    public void Redo() => ListIndex++;
    public bool CanRedo() => IsInitialized && ListIndex < objList.Count - 1;
    public void Edit(ICloneable newObj)
    {
      objList.RemoveRange(listIndex + 1, objList.Count - listIndex - 1);
      objList.Add(newObj.Clone());
      ListIndex++; // Update Object
    }
    public virtual bool CanEdit(object parameter) => IsInitialized;
  }
}
