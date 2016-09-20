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
        public const string IMAGE_File = "data:image/png;base64,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";
        public const string IMAGE_Folder = "data:image/png;base64,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";

        public const string DesktopPath = "$desktop";
        private HTMLElement Element;

        public FileExplorer(HTMLElement element)
        {
            Element = element;
        }        

        private List<FileExplorerNode> LoadedNodes = new List<FileExplorerNode>();

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
                LoadedNodes.Add(FileExplorerNode.CreateNode(Files[i], NodeViewType, true));                
            }

            for (int i = 0; i < Folders.Length; i++)
            {
                LoadedNodes.Add(FileExplorerNode.CreateNode(Folders[i], NodeViewType));
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
                    if (j > 10)
                    {
                        x += 76;
                        y = 0;

                        j = 0;
                    }

                    y += 70;

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

        public HTMLDivElement NodeBase { get; set; }
        private HTMLDivElement NodeImage;
        private HTMLSpanElement NodeText;

        private FileExplorerState nodeState;

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
                        Process.Start(FullPath);
                    }
                });

                NodeBase.AddEventListener(EventType.MouseDown, (ev) =>
                {
                    if (!Form.MidleOfAction())
                    {
                        if(NodeExplorerState == FileExplorerState.HoverFocused || NodeExplorerState == FileExplorerState.Focused)
                        {
                            NodeExplorerState = FileExplorerState.Hover;
                        }else if (NodeExplorerState == FileExplorerState.Selected)
                        {

                        }else
                        {
                            NodeExplorerState = FileExplorerState.HoverFocused;
                        }
                        ev.StopPropagation();
                    }
                });

                NodeBase.AddEventListener(EventType.MouseEnter, (ev) =>
                {
                    if(!Form.MidleOfAction())
                    {
                        if (NodeExplorerState == FileExplorerState.Focused || NodeExplorerState == FileExplorerState.HoverFocused)
                            NodeExplorerState = FileExplorerState.HoverFocused;
                        else if (NodeExplorerState == FileExplorerState.Selected)
                            NodeExplorerState = FileExplorerState.Hover;
                        else
                            NodeExplorerState = FileExplorerState.Hover;
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
                        else
                        {
                            NodeExplorerState = FileExplorerState.None;
                        }
                        ev.StopPropagation();
                    }                    
                });

                if (nodeViewType == NodeViewType.Medium_Icons)
                {
                    jQuery.Select(NodeBase).
                        Css("width", 76).
                        Css("height", 70);
                    
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

                    //position:absolute;
                    //NodeImage.Style.BackgroundSize = "contain";
                    //NodeImage.Style.BackgroundRepeat = BackgroundRepeat.NoRepeat;

                    if (IsFile)
                        img.SetAttribute("src", FileExplorer.IMAGE_File);// NodeImage.Style.Background = FileExplorer.IMAGE_File;
                    else
                        img.SetAttribute("src", FileExplorer.IMAGE_Folder);//NodeImage.Style.Background = FileExplorer.IMAGE_Folder;

                    NodeImage.AppendChild(img);

                    NodeText.InnerHTML = Name;
                    NodeText.Style.FontFamily = "Segoe UI";
                    NodeText.Style.FontSize = "10pt";
                    NodeText.Style.TextAlign = TextAlign.Center;

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

            // image 48x48
            
            switch (nodeState)
            {
                case FileExplorerState.None:
                    NodeBase.Style.BackgroundColor = "";
                    NodeBase.Style.BorderColor = "rgba(255, 255, 255, 0)";
                    break;
                case FileExplorerState.Hover:
                    NodeBase.Style.BackgroundColor = "rgba(255, 255, 255, 0.2)";
                    NodeBase.Style.BorderColor = "rgba(255, 255, 255, 0.5)";
                    break;
                case FileExplorerState.Focused:
                    NodeBase.Style.BackgroundColor = "rgba(255, 255, 255, 0.4)";
                    NodeBase.Style.BorderColor = "rgba(255, 255, 255, 0.5)";
                    break;
                case FileExplorerState.Selected:
                    break;
                case FileExplorerState.HoverFocused:
                    NodeBase.Style.BackgroundColor = "rgba(255, 255, 255, 0.4)";
                    NodeBase.Style.BorderColor = "rgba(255, 255, 255, 0.6)";
                    break;
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
        }

        public static FileExplorerNode CreateNode(string path, NodeViewType nvt, bool IsFile = false)
        {
            var fen = new FileExplorerNode() { IsFile = IsFile, nodeViewType = nvt };
            fen.Name = Path.GetFileName(path);
            fen.Directory = Path.GetDirectoryName(path);
            fen.FullPath = path;

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
