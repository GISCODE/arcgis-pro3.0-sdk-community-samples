//   Copyright 2019 Esri
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       https://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using System.Threading.Tasks;

namespace UpdateAttributesWithSketch
{
  /// <summary>
  /// This sample creates a sketch tool that can be used to update the attributes of features that it intersects. It shows an example of creating a sketch tool, finding intersecting features, updating attributes and performing the edit through an edit operation.
  /// </summary>
  /// <remarks>
  /// 1. Examine the code within AttributeWithSketch.cs.
  /// 1. This code finds a field named "PARCEL_ID" and replaces the value with "42".
  /// 1. Build or debug start the sample through Visual Studio.
  /// 1. The project used for this sample is 'C:\Data\FeatureTest\FeatureTest.aprx'
  /// 1. Select the layer in the table of contents who's attributes you wish to update with this tool. Create a field called "PARCEL_ID" in this layer.
  /// 1. Select the "AttributeWithSketch" tool in the Add-In Tab.
  /// 1. Sketch a line across features in the selected layer.
  /// 1. Features that intersect the sketch will have their attributes in the PARCEL_ID field updated.
  /// ![UI](Screenshots/Screen.png)    
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
                return _this ?? (_this = (Module1)FrameworkApplication.FindModule("UpdateAttributesWithSketch_Module"));
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

        /// <summary>
        /// Generic implementation of ExecuteCommand to allow calls to
        /// <see cref="FrameworkApplication.ExecuteCommand"/> to execute commands in
        /// your Module.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override Func<Task> ExecuteCommand(string id)
        {

            //TODO: replace generic implementation with custom logic
            //etc as needed for your Module
            var command = FrameworkApplication.GetPlugInWrapper(id) as ICommand;
            if (command == null)
                return () => Task.FromResult(0);
            if (!command.CanExecute(null))
                return () => Task.FromResult(0);

            return () =>
            {
                command.Execute(null); // if it is a tool, execute will set current tool
                return Task.FromResult(0);
            };
        }
        #endregion Overrides

    }
}
