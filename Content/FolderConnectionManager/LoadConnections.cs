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
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace FolderConnectionManager
{
  internal class LoadConnections : ArcGIS.Desktop.Framework.Contracts.Button
  {
    protected async override void OnClick()
    {
      try
      {
        // Read a file for line-delimited directory paths, and attempt to add them to the Project's Folder Connections
        OpenItemDialog openDialog = new OpenItemDialog()
        {
          Title = "Select a Folder Connections file",
          MultiSelect = false,
          BrowseFilter = new BrowseProjectFilter("esri_browseDialogFilters_textFiles")
        };

        // If the user clicks OK in the dialog, load the listed directories from the file to the Project's Folder Connections
        if (openDialog.ShowDialog() == true)
        {
          IEnumerable<Item> selectedItem = openDialog.Items;
          foreach (Item i in selectedItem)
          {
            System.IO.StreamReader file = new System.IO.StreamReader(i.Path);
            string line;
            while ((line = file.ReadLine()) != null)
            {
              // Add all folder connections to the current Project's folder connections
              string notFound = "";
              if (Directory.Exists(line))
              {
                var folderToAdd = ItemFactory.Instance.Create(line);
                await QueuedTask.Run(() => Project.Current.AddItem(folderToAdd as IProjectItem));
              }
              else
              {
                notFound += Environment.NewLine + line;
              }

              // Report any folder connections that could not be found
              if (notFound != "")
              {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("The following directories were not found and could not be added to the Folder Connections: " + notFound);
              }

            }
          }
        }
      }

      catch (Exception ex)
      {
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Error adding a folder to the project: " + ex.ToString());
      }
    }
  }
}
