using Editor.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Editor.GameProject
{
    [DataContract]
    public class ProjectTemplate
    {
        [DataMember]
        public string ProjectType { get; set; }
        [DataMember]
        public string ProjectFile { get; set; }
        [DataMember]
        public List<string> Folders { get; set; }
        public byte[] Icon { get; set; }
        public byte[] Image { get; set; }
        public string IconFilePath { get; set; }
        public string ImageFilePath { get; set; }
        public string ProjectFilePath { get; set; }
    }

    class CreateProject : ViewModelBase
    {
        //TODO: get the path from the installation location.
        private readonly string _templatePath = @"C:\GoldScar\Editor\ProjectTemplates";
        private string _ProjectName = "NewProject";
        public string ProjectName
        {
            get => _ProjectName;
            set
            {
                if(_ProjectName != value)
                {
                    _ProjectName = value;
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }

        private string _ProjectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\GoldScarProjects\";
        public string ProjectPath
        {
            get => _ProjectPath;
            set
            {
                if (_ProjectPath != value)
                {
                    _ProjectPath = value;
                    OnPropertyChanged(nameof(Path));
                }
            }
        }

        private ObservableCollection<ProjectTemplate> _ProjectTemplates = new ObservableCollection<ProjectTemplate>();
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates
        {get;}

        public CreateProject()
        {
            ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_ProjectTemplates);
            try
            {
                var TemplateFiles = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
                Debug.Assert(TemplateFiles.Any());
                foreach (var file in TemplateFiles)
                {
                    /*
                    //CODE TO GENERATE TEMPLATE XML (Requires existing blank XML file)...
                    var Template = new ProjectTemplate()
                    {
                        ProjectType = "BlankProject",
                        ProjectFile = "project.goldscar",
                        Folders = new List<string>() { ".GoldScar", "Content", "GameCode"}
                    };

                    Serialiser.ToFile(Template, file);
                    */

                    var template = Serialiser.FromFile<ProjectTemplate>(file);
                    template.IconFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "Icon.png"));
                    template.Icon = File.ReadAllBytes(template.IconFilePath);
                    template.ImageFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "Image.png"));
                    template.Image = File.ReadAllBytes(template.ImageFilePath);
                    template.ProjectFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), template.ProjectFile));
                    _ProjectTemplates.Add(template);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                //TODO: log error.
            }
        }
    }

}
