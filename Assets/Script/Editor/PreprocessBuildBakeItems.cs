using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PreprocessBuildBakeItems : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    void IPreprocessBuildWithReport.OnPreprocessBuild(BuildReport report)
    {

    }
}
