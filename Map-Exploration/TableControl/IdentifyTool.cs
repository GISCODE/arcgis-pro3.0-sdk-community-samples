/*

   Copyright 2022 Esri

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
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableControl
{
  internal class IdentifyTool : MapTool
  {
    public IdentifyTool()
    {
      IsSketchTool = true;
      SketchType = SketchGeometryType.Rectangle;
      SketchOutputMode = SketchOutputMode.Map;
    }

    protected override Task OnToolActivateAsync(bool active)
    {
      return base.OnToolActivateAsync(active);
    }

    protected override async Task<bool> OnSketchCompleteAsync(Geometry geometry)
    {
      if (Module1.SelectedMapMember is BasicFeatureLayer layer)
      {
        // create a new spatial filter
        //  to find features intersecting the sketch geometry
        var filter = new SpatialQueryFilter();
        filter.FilterGeometry = geometry;
        filter.SpatialRelationship = SpatialRelationship.Intersects;

        long oid = -1;
        await QueuedTask.Run(() =>
        {
          // search the layer using the filter
          //   finding the first objectId
          using (var cursor = layer.Search(filter))
          {
            if (cursor.MoveNext())
            {
              using (var row = cursor.Current)
              {
                oid = row.GetObjectID();
              }
            }
          }
        });

        // if an objectID was found
        if (oid != -1)
        {
          // find the dockpane viewmodel
          var vm = FrameworkApplication.DockPaneManager.Find("TableControl_TableControlDockpane") as TableControlDockpaneViewModel;
          // call MoveTo
          if (vm != null)
            _ = vm.MoveTo(oid);
        }
      }
      return true;
    }
  }
}
