using System;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;

namespace GameCreator
{
    [Serializable]
    internal class TestClass
    {
        string name;
        public string Name { 
            get { return name; }
            set { name = value; }
        }
    }

    public partial class MainWindow
    {

        private RelayCommand _AddItem;
        public RelayCommand AddItem
        {
            get
            {
                if (_AddItem == null)
                    _AddItem = new RelayCommand(async (object parameter) =>
                    {
                        if (parameter == null) return;
                        string name = await this.ShowInputAsync(Application.Current.FindResource("New" + parameter.ToString()).ToString(), Application.Current.FindResource("Name".ToString()) + ":");
                        if (name == null) return;
                        switch (parameter.ToString())
                        {
                            case "Class":
                                MainViewModel.Instance.CurrentGame.Classes.Add(new GC_Class(name));
                                break;
                            case "Image":
                                MainViewModel.Instance.CurrentGame.Images.Add(new GC_Image(name));
                                break;
                            case "Object":
                                MainViewModel.Instance.CurrentGame.Objects.Add(new GC_Object(name));
                                break;
                            case "Level":
                                MainViewModel.Instance.CurrentGame.Levels.Add(new GC_Level(name));
                                break;
                        }
                    });
                return _AddItem;
            }
        }

        private RelayCommand _LoadGame;
        public RelayCommand LoadGame
        {
            get
            {
                if (_LoadGame == null)
                {
                    _LoadGame = new RelayCommand(async (object parameter) =>
                    {
                        if (parameter == null) return;
                        switch (parameter.ToString())
                        {
                            case "New":
                                string name = await this.ShowInputAsync(Application.Current.FindResource("NewGame").ToString(), Application.Current.FindResource("Name".ToString()) + ":");
                                if(name != null)
                                    MainViewModel.Instance.CurrentGame = new Models.Game(name);
                                break;
                        }
                    });
                }
                return _LoadGame;
            }
        }

        private RelayCommand _CloseApplication;
        public RelayCommand CloseApplication
        {
            get
            {
                if (_CloseApplication == null)
                {
                    _CloseApplication = new RelayCommand((object parameter) =>
                    {
                        this.Close();
                    });
                }
                return _CloseApplication;
            }
        }

        private RelayCommand _DeleteAll;
        public RelayCommand DeleteAll
        {
            get
            {
                if (_DeleteAll == null)
                {
                    _DeleteAll = new RelayCommand((object parameter) =>
                    {
                        if (parameter == null)
                        {
                            MainViewModel.Instance.CurrentGame.Classes.Clear();
                            MainViewModel.Instance.CurrentGame.Images.Clear();
                            MainViewModel.Instance.CurrentGame.Objects.Clear();
                            MainViewModel.Instance.CurrentGame.Levels.Clear();
                            return;
                        }
                        switch (parameter.ToString())
                        {
                            case "Class":
                                MainViewModel.Instance.CurrentGame.Classes.Clear();
                                break;
                            case "Image":
                                MainViewModel.Instance.CurrentGame.Images.Clear();
                                break;
                            case "Object":
                                MainViewModel.Instance.CurrentGame.Objects.Clear();
                                break;
                            case "Level":
                                MainViewModel.Instance.CurrentGame.Levels.Clear();
                                break;
                        }
                    });
                }
                return _DeleteAll;
            }
        }

        private RelayCommand _DeleteItem;
        public RelayCommand DeleteItem
        {
            get
            {
                if(_DeleteItem == null)
                {
                    _DeleteItem = new RelayCommand((object item) =>
                    {
                        if(item == null)
                            return;
                        if (item.GetType() == typeof(GC_Class))
                            MainViewModel.Instance.CurrentGame.Classes.Remove(item as GC_Class);
                        else if (item.GetType() == typeof(GC_Image))
                            MainViewModel.Instance.CurrentGame.Images.Remove(item as GC_Image);
                        else if (item.GetType() == typeof(GC_Object))
                            MainViewModel.Instance.CurrentGame.Objects.Remove(item as GC_Object);
                        else if (item.GetType() == typeof(GC_Level))
                            MainViewModel.Instance.CurrentGame.Levels.Remove(item as GC_Level);
                    });
                }
                return _DeleteItem;
            }
        }

        private RelayCommand _EditItem;
        public RelayCommand EditItem
        {
            get
            {
                if(_EditItem == null)
                {
                    _EditItem = new RelayCommand((object item) =>
                    {
                        if (item == null)
                            return;
                        
                        
                    });
                }
                return _EditItem;
            }
        }

        private RelayCommand _CopyItem;
        public RelayCommand CopyItem
        {
            get
            {
                if (_CopyItem == null)
                {
                    _CopyItem = new RelayCommand((object item) =>
                    {
                        if (item == null)
                            return;
                        if(item.GetType() == typeof(GC_Class))
                        {
                            Clipboard.SetData(MainViewModel.CLASS_DATA_FORMAT, (item as GC_Class).Clone());
                        }
                        else if(item.GetType() == typeof(GC_Image))
                        {
                            Clipboard.SetData(MainViewModel.IMAGE_DATA_FORMAT, (item as GC_Image).Clone());
                        }
                        else if(item.GetType() == typeof(GC_Object))
                        {
                            Clipboard.SetData(MainViewModel.OBJECT_DATA_FORMAT, (item as GC_Object).Clone());
                        }
                        else if(item.GetType() == typeof(GC_Level))
                        {
                            Clipboard.SetData(MainViewModel.LEVEL_DATA_FORMAT, (item as GC_Level).Clone());
                        }
                    });
                }
                return _CopyItem;
            }
        }

        private RelayCommand _CutItem;
        public RelayCommand CutItem
        {
            get
            {
                if(_CutItem == null)
                {
                    _CutItem = new RelayCommand((object item) =>
                    {
                        _CopyItem.Execute(item);
                        _DeleteItem.Execute(item);
                    });
                }
                return _CutItem;
            }
        }

        private RelayCommand _PasteItem;
        public RelayCommand PasteItem
        {
            get
            {
                if(_PasteItem == null)
                {
                    _PasteItem = new RelayCommand((object item) =>
                    {
                        //----------------Error-------------------------//
                        if (item == null)
                            return;
                        //--------------Clicked on header OR item---------------------//
                        switch((string)item)
                        {
                            case "Class":
                                if (Clipboard.ContainsData(MainViewModel.CLASS_DATA_FORMAT))
                                {
                                    GC_Class clazz = Clipboard.GetData(MainViewModel.CLASS_DATA_FORMAT) as GC_Class;
                                    if (clazz != null)
                                    {
                                        while (MainViewModel.Instance.CurrentGame.Classes.Select(x => x.Name).Contains(clazz.Name))
                                            clazz.Name += " - " + Application.Current.FindResource("CopyOf").ToString();
                                        MainViewModel.Instance.CurrentGame.Classes.Add(clazz);
                                    }
                                }
                                break;
                            case "Image":
                                if (Clipboard.ContainsData(MainViewModel.IMAGE_DATA_FORMAT))
                                {
                                    GC_Image image = Clipboard.GetData(MainViewModel.IMAGE_DATA_FORMAT) as GC_Image;
                                    if (image != null)
                                    {
                                        while (MainViewModel.Instance.CurrentGame.Images.Select(x => x.Name).Contains(image.Name))
                                            image.Name += " - " + Application.Current.FindResource("CopyOf").ToString();
                                        MainViewModel.Instance.CurrentGame.Images.Add(image);
                                    }
                                }
                                break;
                            case "Object":
                                if (Clipboard.ContainsData(MainViewModel.OBJECT_DATA_FORMAT))
                                {
                                    GC_Object obj = Clipboard.GetData(MainViewModel.OBJECT_DATA_FORMAT) as GC_Object;
                                    if (obj != null)
                                    {
                                        while (MainViewModel.Instance.CurrentGame.Objects.Select(x => x.Name).Contains(obj.Name))
                                            obj.Name += " - " + Application.Current.FindResource("CopyOf").ToString();
                                        MainViewModel.Instance.CurrentGame.Objects.Add(obj);
                                    }  
                                }
                                break;
                            case "Level":
                                if (Clipboard.ContainsData(MainViewModel.LEVEL_DATA_FORMAT))
                                {
                                    GC_Level level = Clipboard.GetData(MainViewModel.LEVEL_DATA_FORMAT) as GC_Level;
                                    if (level != null)
                                    {
                                        while (MainViewModel.Instance.CurrentGame.Levels.Select(x => x.Name).Contains(level.Name))
                                            level.Name += " - " + Application.Current.FindResource("CopyOf").ToString();
                                        MainViewModel.Instance.CurrentGame.Levels.Add(level);
                                    }
                                }
                                break;
                        }
                    });
                }
                return _PasteItem;
            }
        }
    }
}