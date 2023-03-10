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

using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;

namespace ArcGISOnlineConnect
{
  /// <summary>
  /// ArcGISOnlineConnect exercises a collection of programmatic interactions with ArcGIS Online using EsriHttpClient
  /// </summary>
  /// <remarks>
  /// 1. This sample is using the ArcGIS REST API which is published here https://resources.arcgis.com/en/help/arcgis-rest-api  
  /// 1. This solution is using the **Newtonsoft.Json NuGet**.  If needed, you can install the NuGet from the "NuGet Package Manager Console" by using this script: "Install-Package Newtonsoft.Json".
  /// 1. In Visual Studio click the Build menu. Then select Build Solution.
  /// 1. Click Start button to open ArcGIS Pro.
  /// 1. ArcGIS Pro will open. 
  /// 1. Open any project file. Click on the Add-in tab on the ribbon and then on the Show "AgolDockpane" button.
  /// ![UI](Screenshot/AgolInterface.png)  
  /// 1. On top the AgolDockpane (pane) you find the ArcGIS Online Uri used for the interaction with ArcGIS Online (your portal).
  /// 1. Select from the 'AGOL operation' listbox by starting with 'GetRest' (go through the list top to bottom doing the following steps):
  /// 1. Verify the Parameter(s) required for the query you just selected (note: default values are filled in by using return values from previous query results so sequence is important)
  /// 1. Click on the "Run ... ArcGIS Online Query" button to execute the query.
  /// 1. View the results in text box on the bottom of the AgolDockpane.  
  /// 1. Please note that ArcGIS Online queries return json and various content returned in json is deserialized into the respective c# class.
  /// 1. Also note that permissions and content are required for various queries (i.e. content or folder queries)
  /// 1. The 'GetSearch' query requires a search string which by default is set to 'Redlands'. 
  /// ![UI](Screenshot/Query1.png) 
  /// Note: Alternatively, SearchForContentAsync method [topic19135.html](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic19135.html) can be used to perform portal item searches. 
  /// ```cs
  ///  //Create the Query and the params
  ///var pqp = PortalQueryParameters.CreateForItemsOfType(portalItemType, searchString); //overloaded
  /// //Search active portal
  /// PortalQueryResultSet&lt;PortalItem&gt; results = await ArcGISPortalExtensions.SearchForContentAsync(portal, pqp);
  /// //Iterate through the returned items for THE item.
  /// var myPortalItem = results.Results?.OfType&lt;PortalItem&gt;().FirstOrDefault();
  /// ```
  /// 1. The 'GetUserContent' query requires a user name, however, if you performed the 'GetSelf' query before the parameter is filled in automatically for you.  
  /// ![UI](Screenshot/Query2.png)  
  /// Note: Alternatively, GetUserContentAsync method [topic19134.html](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic19134.html) can be used to get the given user's content.
  /// ```cs
  /// PortalQueryResultSet&lt;PortalItem&gt; results = await ArcGISPortalExtensions.GetUserContentAsync(portal, username);
  /// ```
  /// 1. The 'GetUserContentForFolder' query requires a user name and a folder id, however, if you performed the 'GetSelf' and 'GetUserContent' query before, those parameters are filled in automatically for you from previous query results.  Also you need to have a folder under you 'My content' tab in ArcGIS Online.  
  /// ![UI](Screenshot/Query3.png)  
  /// Note: Alternatively, GetUserContentAsync method [topic19134.html](https://pro.arcgis.com/en/pro-app/sdk/api-reference/#topic19134.html) can be used to get the given user's content for a specific folder.
  /// ```cs
  /// PortalQueryResultSet&lt;PortalItem&gt; results = await ArcGISPortalExtensions.GetUserContentAsync(portal, username, folderID);
  /// ```
  /// 1. The 'GetGroupMetadata, and GetGroupContent' queries require a group id, however, if you performed the 'GetSelf' query before, those parameters are filled in automatically for you from previous query results.  
  /// ![UI](Screenshot/Query4.png)  
  /// 1. The 'GetAdditemStatus, GetItem, GetItemPkInfo, GetItemData, and GetItemComments' queries require an item id, however, if you performed the 'GetUserContent' query before, those parameters are filled in automatically for you from previous query results.   
  /// ![UI](Screenshot/Query5.png)   
  /// 1. The 'GetGroupUsers, and GetUserTags' queries require a group id, however, if you performed the 'GetSelf' query before, those parameters are filled in automatically for you from previous query results.  
  /// ![UI](Screenshot/Query6.png) 
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
                return _this ?? (_this = (Module1)FrameworkApplication.FindModule("ArcGISOnlineConnect_Module"));
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
