using EnvDTE80;
using Microsoft.Test.Apex.VisualStudio;
using Microsoft.Test.Apex.VisualStudio.Solution;

namespace Xamarin.VisualStudio.Apex.Core
{
    public abstract class XamarinTest : VisualStudioHostTest
    {
        protected SolutionService Solution => VisualStudio.ObjectModel.Solution;

        protected ProjectTestExtension CreateProject(string templateId, ProjectLanguage language = ProjectLanguage.CSharp)
        {
            var projectInfo = new NewProjectInfo
            {
                Language = language,
                TemplatePath = GetTemplatePath(templateId, language)
            };

            if (!Solution.IsOpen) Solution.CreateEmptySolution();
            
            var project = Solution.AddProject<ProjectTestExtension>(projectInfo);

            SetupProject(project);

            return project;
        }

        protected virtual void SetupProject (ProjectTestExtension project) { }

        string GetTemplatePath(string templateId, ProjectLanguage language)
        {
            var solution = VisualStudio.Dte.Solution as Solution2;

            return solution.GetProjectTemplate(templateId, language.ToString());
        }
    }
}
