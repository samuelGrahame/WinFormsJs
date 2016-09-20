using Bridge.Html5;
using Bridge.jQuery2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class FileExplorer
    {
        public NodeViewType NodeViewType { get; set; }        
        private string path;
        //url('
        

        public const string DesktopPath = "$desktop";
        private HTMLElement Element;

        public FileExplorer(HTMLElement element)
        {
            Element = element;
        }        

        public List<FileExplorerNode> LoadedNodes = new List<FileExplorerNode>();

        public string Path
        {
            get { return path; }
            set
            {
                if(path != value)
                {
                    if(IsDirectoryRequestValue(value))
                    {
                        path = value;
                        Refresh();
                    }
                    else
                    {
                        WarnEnduserOfInvalidRequest();
                    }               
                }   
            }
        }

        private void WarnEnduserOfInvalidRequest()
        {

        }

        public static bool IsDirectoryRequestValue(string directory)
        {



            return true;
        }

        private void ClearItems()
        {
            for (int i = 0; i < LoadedNodes.Count; i++)
            {
                if (LoadedNodes[i] != null)
                    LoadedNodes[i].Remove();
            }
            LoadedNodes = new List<FileExplorerNode>();
        }

        public int GetSelectionCount(FileExplorerNode DontInclude = null)
        {
            int x = 0;
            for (int i = 0; i < LoadedNodes.Count; i++)
            {
                if (LoadedNodes[i] != null && LoadedNodes[i] != DontInclude)
                {
                    var htmlNode = LoadedNodes[i].NodeBase;
                    if (htmlNode != null)
                    {
                        if (LoadedNodes[i].NodeExplorerState == FileExplorerNode.FileExplorerState.Selected || LoadedNodes[i].NodeExplorerState == FileExplorerNode.FileExplorerState.HoverSelected)
                            x++;
                    }
                }
            }
            return x;
        }

        public void ClearSelection(FileExplorerNode DontInclude = null)
        {
            for (int i = 0; i < LoadedNodes.Count; i++)
            {
                if (LoadedNodes[i] != null && LoadedNodes[i] != DontInclude)
                {
                    var htmlNode = LoadedNodes[i].NodeBase;
                    if (htmlNode != null)
                    {
                        LoadedNodes[i].NodeExplorerState = FileExplorerNode.FileExplorerState.None;
                    }
                }
            }
        }

        public void SetFocus(int index)
        {
            SetFocus(LoadedNodes[index]);
        }

        public void SetFocus(FileExplorerNode node)
        {
            for (int i = 0; i < LoadedNodes.Count; i++)
            {
                if (LoadedNodes[i] != null)
                {
                    var htmlNode = LoadedNodes[i].NodeBase;
                    if (htmlNode != null)
                    {
                        if(LoadedNodes[i] == node)
                        {
                            if(LoadedNodes[i].NodeExplorerState == FileExplorerNode.FileExplorerState.Hover)
                            {
                                LoadedNodes[i].NodeExplorerState = FileExplorerNode.FileExplorerState.HoverFocused;
                            }
                            else
                            {
                                LoadedNodes[i].NodeExplorerState = FileExplorerNode.FileExplorerState.Focused;
                            }                            
                        }
                        else
                        {
                            LoadedNodes[i].NodeExplorerState = FileExplorerNode.FileExplorerState.None;
                        }
                        
                    }
                }
            }
        }

        public void Refresh()
        {            
            if (LoadedNodes == null)
                LoadedNodes = new List<FileExplorerNode>();
            else if (LoadedNodes.Count > 0)
                ClearItems();

                var nvt = NodeViewType;
            if (Path == DesktopPath)
            {
                // load the locations of the desktop items.
                nvt = NodeViewType.Medium_Icons;
            }
            
            string[] Files = Directory.GetFiles(Path);
            string[] Folders = Directory.GetDirectories(Path);

            for (int i = 0; i < Files.Length; i++)
            {
                LoadedNodes.Add(FileExplorerNode.CreateNode(Files[i], NodeViewType, this, true));                
            }

            for (int i = 0; i < Folders.Length; i++)
            {
                LoadedNodes.Add(FileExplorerNode.CreateNode(Folders[i], NodeViewType, this));
            }

            // get the order type!! #TODO# sort items
            int x = 0;
            int y = 19;

            int j = 0;

            for (int i = 0; i < LoadedNodes.Count; i++)
            {
                if(LoadedNodes[i] != null && LoadedNodes[i].NodeBase != null)
                {
                    jQuery.Select(LoadedNodes[i].NodeBase).
                        Css("left", x).
                        Css("top", y);
                    Element.AppendChild(LoadedNodes[i].NodeBase);
                    j++;

                    y += 70;
                    
                    if (j > 8)
                    {
                        x += 78;
                        y = 0;

                        j = 0;
                    }

                    y += 19;
                }
                    
            }            
        }
    }

    public enum NodeViewType
    {
        Content,
        Tiles,
        Details,
        List,
        Small_Icons,
        Medium_Icons,
        Large_Icons,
        Extra_Large_Icons
    }

    public class FileExplorerNode
    {
        public string Name { get; set; }
        public string Directory { get; set; }
        public string FullPath { get; set; }

        public NodeViewType nodeViewType { get; set; }

        public bool IsFile { get; set; }

        public string Icon { get; set; } = string.Empty;

        public HTMLDivElement NodeBase { get; set; }
        private HTMLDivElement NodeImage;
        private HTMLSpanElement NodeText;

        private FileExplorerState nodeState;

        protected FileExplorer parent;

        public FileExplorerState NodeExplorerState
        {
            get { return nodeState; }
            set
            {
                if(nodeState != value)
                {
                    nodeState = value;
                    if(NodeBase != null)
                    {
                        CreateHtmlNode();
                    }
                }                
            }
        }

        private void CreateHtmlNode()
        {
            if (NodeBase == null)
            {
                NodeBase = new HTMLDivElement();                
                NodeImage = new HTMLDivElement();
                NodeText = new HTMLSpanElement();

                NodeBase.Style.ZIndex = "0";

                NodeBase.Style.Position = Position.Absolute;
                NodeImage.Style.Position = Position.Absolute;
                NodeText.Style.Position = Position.Absolute;

                NodeBase.AddEventListener(EventType.DblClick, (ev) =>
                {
                    if (!Form.MidleOfAction())
                    {
                        parent.ClearSelection();

                        Process.Start(FullPath);
                    }
                });

                NodeBase.AddEventListener(EventType.MouseUp, (ev) => 
                {
                    if (!Form.MidleOfAction())
                    {
                        // did i drag...
                        parent.ClearSelection(this);
                    }
                });

                NodeBase.AddEventListener(EventType.MouseDown, (ev) =>
                {
                    if (!Form.MidleOfAction())
                    {
                        int selectionCount = parent.GetSelectionCount(this);

                        if (NodeExplorerState == FileExplorerState.Selected)
                        {
                            if (selectionCount == 0)
                                parent.ClearSelection(this);
                            NodeExplorerState = FileExplorerState.Focused;
                        }
                        else
                        {
                            if (selectionCount == 0)
                                parent.ClearSelection(this);
                            NodeExplorerState = FileExplorerState.Focused;
                        }
                        ev.StopPropagation();
                    }
                });

                NodeBase.AddEventListener(EventType.MouseEnter, (ev) =>
                {
                    if(!Form.MidleOfAction())
                    {
                        if (NodeExplorerState == FileExplorerState.Focused || NodeExplorerState == FileExplorerState.HoverFocused)
                        {                            
                            NodeExplorerState = FileExplorerState.HoverFocused;
                        }
                        else if (NodeExplorerState == FileExplorerState.Selected || NodeExplorerState == FileExplorerState.HoverSelected)
                        {                            
                            NodeExplorerState = FileExplorerState.HoverSelected;
                        }
                        else
                        {                            
                            NodeExplorerState = FileExplorerState.Hover;
                        }
                        ev.StopPropagation();
                    }
                });

                NodeBase.AddEventListener(EventType.MouseLeave, (ev) =>
                {
                    if (!Form.MidleOfAction())
                    {
                        if (NodeExplorerState == FileExplorerState.HoverFocused || NodeExplorerState == FileExplorerState.Focused)
                        {                            
                            NodeExplorerState = FileExplorerState.Focused;
                        }
                        else if (NodeExplorerState == FileExplorerState.HoverSelected || NodeExplorerState == FileExplorerState.Selected)
                        {                            
                            NodeExplorerState = FileExplorerState.Selected;
                        }
                        else
                        {                            
                            NodeExplorerState = FileExplorerState.None;
                        }
                        ev.StopPropagation();
                    }                    
                });

                if (nodeViewType == NodeViewType.Medium_Icons)
                {                    
                    jQuery.Select(NodeImage).
                        Css("width", 48).
                        Css("height", 48).
                        Css("left", 14).
                        Css("top", 2);

                    NodeBase.Style.BorderStyle = BorderStyle.Solid;
                    NodeBase.Style.BorderWidth = BorderWidth.Thin;

                    HTMLImageElement img = new HTMLImageElement();

                    img.Style.MaxWidth = "100%";
                    img.Style.MaxHeight = "100%";

                    img.Style.Position = Position.Absolute;
                    img.Style.Display = Display.Block;

                    if (IsFile)
                    {
                        if(Icon == string.Empty)
                        {
                            img.SetAttribute("src", IconRepository.IMAGE_File);// NodeImage.Style.Background = FileExplorer.IMAGE_File;
                        }
                        else
                        {
                            img.SetAttribute("src", Icon);// NodeImage.Style.Background = FileExplorer.IMAGE_File;
                        }
                    }
                    else
                        img.SetAttribute("src", IconRepository.IMAGE_Folder);//NodeImage.Style.Background = FileExplorer.IMAGE_Folder;

                    Form.DisableStateDrag(img);

                    NodeImage.AppendChild(img);

                    NodeText.InnerHTML = Name;
                    NodeText.Style.FontFamily = "Segoe UI";
                    NodeText.Style.FontSize = "9.5pt";
                    NodeText.Style.TextAlign = TextAlign.Center;
                    NodeText.Style.Cursor = Cursor.Default;
                    NodeText.Style.TextShadow = "0px 2px 7px rgba(0, 0, 0, 0.5)";

                    // think you are looking for  text-overflow: ellipsis in combination with white-space: nowrap
                    NodeText.Style.TextOverflow = TextOverflow.Ellipsis;
                    NodeText.Style.WhiteSpace = WhiteSpace.NoWrap;
                    NodeText.Style.Overflow = Overflow.Hidden;


                    Form.SetInternalLabel(NodeText);

                    Form.ChangeStateTextSelection(NodeText, false);
                    Form.ChangeStateTextSelection(NodeImage, false);
                    Form.ChangeStateTextSelection(NodeBase, false);
                    Form.ChangeStateTextSelection(img, false);

                    jQuery.Select(NodeText).
                       Css("width", 74).
                       Css("height", 20).
                       Css("left", 2).
                       Css("top", 48);

                    NodeText.Style.Color = "white";

                    NodeBase.AppendChild(NodeImage);
                    NodeBase.AppendChild(NodeText);
                }
            }

            if (nodeViewType == NodeViewType.Medium_Icons)
            {
                if(nodeState == FileExplorerState.Focused ||
                    nodeState == FileExplorerState.HoverFocused)
                {                    
                    NodeText.Style.Overflow = Overflow.Visible;
                    
                    jQuery.Select(NodeBase).
                       Css("width", 76 - NodeText.ClientWidth + NodeText.ScrollWidth).
                       Css("height", 50 + NodeText.ScrollHeight);
                }
                else
                {
                    jQuery.Select(NodeBase).
                       Css("width", 76).
                       Css("height", 70);

                    NodeText.Style.Overflow = Overflow.Hidden;
                }
            }

            // image 48x48

                switch (nodeState)
            {
                case FileExplorerState.None:
                    NodeBase.Style.BackgroundColor = "";
                    NodeBase.Style.BorderColor = "rgba(255, 255, 255, 0)";
                    break;
                case FileExplorerState.HoverSelected:
                case FileExplorerState.HoverFocused:
                case FileExplorerState.Hover:
                    NodeBase.Style.BackgroundColor = "rgba(255, 255, 255, 0.2)";
                    NodeBase.Style.BorderColor = "rgba(255, 255, 255, 0.5)";
                    break;
                case FileExplorerState.Selected:
                case FileExplorerState.Focused:
                    NodeBase.Style.BackgroundColor = "rgba(255, 255, 255, 0.4)";
                    NodeBase.Style.BorderColor = "rgba(255, 255, 255, 0.5)";
                    break;
                //case FileExplorerState.HoverSelected:
                //case FileExplorerState.HoverFocused:
                //    NodeBase.Style.BackgroundColor = "rgba(255, 255, 255, 0.4)";
                //    NodeBase.Style.BorderColor = "rgba(255, 255, 255, 0.6)";
                //    break;
                default:
                    break;
            }            
        }

        public enum FileExplorerState
        {
            None,
            Hover,
            Focused,
            Selected,
            HoverFocused,
            HoverSelected,
        }

        public static FileExplorerNode CreateNode(string path, NodeViewType nvt, FileExplorer parent, bool IsFile = false)
        {
            var fen = new FileExplorerNode() { IsFile = IsFile, nodeViewType = nvt };
            fen.parent = parent;
            fen.Name = Path.GetFileName(path);
            fen.Directory = Path.GetDirectoryName(path);
            fen.FullPath = path;
            fen.Icon = IconRepository.GetIconByFileName(fen.Name);

            fen.CreateHtmlNode();
            
            return fen;
        }
        
        public void Remove()
        {
            if(NodeBase != null)
            {
                NodeBase.Remove();                
            }
        }
    }
}
