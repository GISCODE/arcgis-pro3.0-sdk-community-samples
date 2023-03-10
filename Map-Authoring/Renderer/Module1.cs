/*

   Copyright 2019 Esri

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       https://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

   See the License for the specific language governing permissions and
   limitations under the License.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using System.Threading.Tasks;

namespace Renderer
{
  /// <summary>
  /// This sample renders feature layers with various types of renderers. There are examples for the following types of renderers in this sample:
  ///   * Simple Renderers
  ///   * Unique Value Renderer
  ///   * Class breaks renderers
  ///       * Graduated color to define quantities
  ///       * Graduated symbols to define quantities
  ///       * Unclassed color gradient to define quantities
  ///   * Proportional Renderer
  ///   * Heat map renderer
  ///   * Chart renderer
  ///   * Dot Density Renderer
  ///   * Attribute driven symbology
  /// </summary>
  /// <remarks>
  /// 1. Download the Community Sample data (see under the 'Resources' section for downloading sample data).  The sample data contains a folder called 'C:\Data\Renderers' with sample data required for this solution.  Make sure that the Sample data is unzipped in c:\data and "C:\Data\Renderers" is available.
  /// 1. In Visual Studio click the Build menu. Then select Build Solution.  
  /// 1. Launch the debugger to open ArcGIS Pro.
  /// 1. Create a new blank Map project. 
  /// 1. Add the C:\Data\Renderers\USDemographics.lpkx layer file to the map. 
  /// 1. In Add-in tab, click the "Apply Renderer" button.
  /// 1. The first point feature layer in your project's TOC will be rendered with an "Unique Value Renderer".
  /// To experiment with the various renderers available with this sample, follow the steps below:
  /// 1. Stop debugging Pro.
  /// 1. In Visual Studio's solution explorer, open the ApplyRenderer.cs file. This is the class file for the Apply Renderer button.
  /// 1. The OnClick method in the ApplyRenderer.cs file uncomment the method that calls the renderer you want to use.<br/>
  ///    **You can modify the code in the OnClick method in this file to use one of the various renderers available in this sample.**
  ///     ```c#  
  ///     protected async override void OnClick()  
  ///     {  
  ///          //TODO: Modify this line below to experiment with the different renderers  
  ///          //await DotDensityRenderer.DotDensityRendererAsync();
  ///          //Charts
  ///          //await ChartRenderers.BarChartRendererAsync();
  ///          //etc
  ///     }  
  ///     ```
  /// 1. After modifying the OnClickMethod build the solution and click the start button to open Pro.  
  /// 1. Open any project and test the Apply Renderer button again.
  /// ![UI](screenshots/Renderers.png)
  /// Note: Use the US Cities layer available with the sample data to the Heat map renderer. This feature class is available in the C:\Data\Admin\AdminData.gdb file geodatabase.
  /// #### Attribute Driven Symbology
  /// 1. Create a new Local Scene. Add the C:\Data\Renderers\FlightPathPoints.lyrx layer file to the scene.  This layer draws a point geometry rendered with a helicopter symbol.  The data for this layer holds the Tilt angles (X, Y and Z) for the helicopter.
  /// 1. Use the Navigate button to tilt the view so that you can see the helicopter to display it over the map.
  /// ![Tilt](screenshots/tilt.png)
  /// 1. Click the AttributeDriverSymbology button on the Add-In tab.
  /// 1. Notice that the helicopter is now displayed using the Tilt fields.
  /// ![AttrbuteDriverSymbology](screenshots/AttrbuteDriverSymbology.png)
  /// </remarks>
  internal class Module1 : Module
    {
        private static Module1 _this = null;

        /// <summary>
        /// Retrieve the singleton instance to this module here
        /// </summary>
        public static Module1 Current
        {
            get
            {
                return _this ?? (_this = (Module1)FrameworkApplication.FindModule("Renderer_Module"));
            }
        }

        #region Overrides
        /// <summary>
        /// Called by Framework when ArcGIS Pro is closing
        /// </summary>
        /// <returns>False to prevent Pro from closing, otherwise True</returns>
        protected override bool CanUnload()
        {
            //TODO - add your business logic
            //return false to ~cancel~ Application close
            return true;
        }

        #endregion Overrides

    }
}
