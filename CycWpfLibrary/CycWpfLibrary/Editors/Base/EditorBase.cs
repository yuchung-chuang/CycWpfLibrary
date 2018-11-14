using CycWpfLibrary.Media;
using CycWpfLibrary.MVVM;
using System.Collections.Generic;
using System.Windows.Input;

namespace CycWpfLibrary
{
  /// <summary>
  /// 提供物件編輯功能的基底類別。
  /// </summary>
  /// <typeparam name="EditObjectType">要編輯的物件類</typeparam>
  public abstract class EditorBase<EditObjectType> where EditObjectType : class
  {
    private List<EditObjectType> objList = new List<EditObjectType>();
    private int listIndex;
    private int ListIndex
    {
      get => listIndex;
      set
      {
        listIndex = value;
        Object = objList[value];
      }
    }

    protected EditorBase()
    {
      UndoCommand = new RelayCommand(Undo, CanUndo);
      RedoCommand = new RelayCommand(Redo, CanRedo);
      EditCommand = new RelayCommand<object>(Edit, CanEdit);
    }
    protected EditorBase(EditObjectType initObj) : this()
    {
      objList.Add(initObj);
      ListIndex = 0;
    }

    public EditObjectType Object { get; private set; }
    public ICommand UndoCommand { get; set; }
    public ICommand RedoCommand { get; set; }
    public ICommand EditCommand { get; set; }

    public void Undo() => ListIndex--;
    public bool CanUndo() => ListIndex > 0;
    public void Redo() => ListIndex++;
    public bool CanRedo() => ListIndex < objList.Count - 1;
    public void Edit(object newObj)
    {
      objList.RemoveRange(listIndex + 1, objList.Count - listIndex - 1);
      objList.Add(newObj as EditObjectType);
      ListIndex++;
    }
    public abstract bool CanEdit(object parameter);
  }
}
