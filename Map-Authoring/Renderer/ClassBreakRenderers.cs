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
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using Renderer.Helpers;

namespace Renderer
{
    internal static class ClassBreakRenderers
    {
        #region Snippet Class Breaks renderer with graduated colors.
        /// <summary>
        /// Renders a feature layer using graduated colors to draw quantities.
        /// ![cb-colors.png](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/cb-colors.png "Graduated colors with natural breaks renderer.")
        /// </summary>
        /// <returns>
        /// </returns>
        internal static Task CBRendererGraduatedColors()
        {
            //Check feature layer name
            //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
            var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
            if (featureLayer == null)
            {
              MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
              return Task.FromResult(0);
            }
            return QueuedTask.Run(() =>
            {
                GraduatedColorsRendererDefinition gcDef = new GraduatedColorsRendererDefinition()
                {                    
                    ClassificationField = SDKHelpers.GetNumericField(featureLayer),
                    ClassificationMethod = ClassificationMethod.NaturalBreaks,
                    BreakCount = 5,
                    ColorRamp = SDKHelpers.GetColorRamp(),
                };
                CIMClassBreaksRenderer renderer = (CIMClassBreaksRenderer)featureLayer.CreateRenderer(gcDef);
                featureLayer?.SetRenderer(renderer);
            });
        }
        #endregion
        #region Snippet Class Breaks renderer with graduated colors and outline
        /// <summary>
        /// Renders a feature layer using graduated colors to draw quantities. The outline width is varied based on attributes.
        /// ![graduatedColorOutline.png](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/graduatedColorOutline.png "Graduated colors with natural breaks renderer.") 
        /// </summary>
        /// <returns></returns>
        internal static Task CBRendererGraduatedColorsOutlineAsync()
        {
            //Check feature layer name
            //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
            var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
            if (featureLayer == null)
            {
              MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
              return Task.FromResult(0);
            }
            return QueuedTask.Run(() =>
            {
                //Gets the first numeric field of the feature layer
                var firstNumericFieldOfFeatureLayer = SDKHelpers.GetNumericField(featureLayer);
                //Gets the min and max value of the field
                var minMax = SDKHelpers.GetFieldMinMax(featureLayer, firstNumericFieldOfFeatureLayer);
                GraduatedColorsRendererDefinition gcDef = new GraduatedColorsRendererDefinition()
                {
                     ClassificationField = SDKHelpers.GetNumericField(featureLayer),
                     ClassificationMethod = ClassificationMethod.NaturalBreaks,
                     BreakCount = 5,
                     ColorRamp = SDKHelpers.GetColorRamp()
                     
                };
                CIMClassBreaksRenderer renderer = (CIMClassBreaksRenderer)featureLayer.CreateRenderer(gcDef);
                //Create array of CIMVisualVariables to hold the outline information.
                var visualVariables = new CIMVisualVariable[] {
                    new CIMSizeVisualVariable
                    {
                        ValueExpressionInfo = new CIMExpressionInfo
                        {
                           Title = "Custom",
                           Expression = "$feature.AREA",
                           ReturnType = ExpressionReturnType.Default
                        },
                        AuthoringInfo = new CIMVisualVariableAuthoringInfo
                        {
                            MinSliderValue = Convert.ToDouble(minMax.Item1),
                            MaxSliderValue = Convert.ToDouble(minMax.Item2),         
                            ShowLegend = false,
                            Heading = firstNumericFieldOfFeatureLayer
                        },
                        VariableType = SizeVisualVariableType.Graduated,
                        Target = "outline",
                        MinSize = 1,
                        MaxSize = 13,
                        MinValue = Convert.ToDouble(minMax.Item1),
                        MaxValue = Convert.ToDouble(minMax.Item2)
                    },
                    
                };
                renderer.VisualVariables = visualVariables;
                featureLayer?.SetRenderer(renderer);
        });

        }
#endregion

        #region Snippet Class Breaks renderer with graduated symbols.
        /// <summary>
        /// Renders a feature layer using graduated symbols and natural breaks to draw quantities.
        /// ![cb-symbols.png](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/cb-symbols.png "Graduated symbols with natural breaks renderer.")
        /// </summary>
        /// <returns>        
        /// </returns>
        internal static Task CBRendererGraduatedSymbols()
        {
          //Check feature layer name
          //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
          var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
          if (featureLayer == null)
          {
            MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
            return Task.FromResult(0);
          }
          return QueuedTask.Run(() =>
            {
                GraduatedSymbolsRendererDefinition gsDef = new GraduatedSymbolsRendererDefinition()
                {                   
                    ClassificationField = SDKHelpers.GetNumericField(featureLayer), //getting the first numeric field
                    ClassificationMethod = ClassificationMethod.NaturalBreaks,
                    SymbolTemplate = SymbolFactory.Instance.ConstructPointSymbol(CIMColor.CreateRGBColor(76,230,0)).MakeSymbolReference(),
                    MinimumSymbolSize = 4,
                    MaximumSymbolSize = 50,
                    BreakCount = 5,
                    ColorRamp = SDKHelpers.GetColorRamp(), //getting a color ramp
                };
                CIMClassBreaksRenderer renderer = (CIMClassBreaksRenderer)featureLayer.CreateRenderer(gsDef);
                featureLayer?.SetRenderer(renderer);
            });
        }
        #endregion

        #region Snippet UnClassed graduated color renderer.
        /// <summary>
        /// Renders a feature layer using an unclassed color gradient.
        /// ![cb-unclassed.png](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/cb-unclassed.png "Class breaks unclassed renderer.")
        /// </summary>
        /// <returns>        
        /// </returns>
        internal static Task UnclassedRenderer()
        {
            //Check feature layer name
            //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
            var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "U.S. National Transportation Atlas Interstate Highways");
            if (featureLayer == null)
            {
              MessageBox.Show("This renderer works with the U.S. National Transportation Atlas Interstate Highways feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
              return Task.FromResult(0);
            }
            return QueuedTask.Run(() =>
            {                
                //Gets the first numeric field of the feature layer
                var firstNumericFieldOfFeatureLayer = SDKHelpers.GetNumericField(featureLayer);
                //Gets the min and max value of the field
                var labels = SDKHelpers.GetFieldMinMax(featureLayer, firstNumericFieldOfFeatureLayer);              
                UnclassedColorsRendererDefinition ucDef = new UnclassedColorsRendererDefinition()
                {
                    Field = firstNumericFieldOfFeatureLayer,
                    ColorRamp = SDKHelpers.GetColorRamp(),
                    LowerColorStop = Convert.ToDouble(labels.Item1),
                    UpperColorStop = Convert.ToDouble(labels.Item2),                    
                    UpperLabel = labels.Item2,
                    LowerLabel = labels.Item1,
                };
                CIMClassBreaksRenderer renderer = (CIMClassBreaksRenderer)featureLayer.CreateRenderer(ucDef);
                featureLayer?.SetRenderer(renderer);
            });
        }
        #endregion

        #region Snippet Class Breaks renderer with Manual Intervals
        /// <summary>
        /// Renders a feature layer using graduated colors and manual intervals to draw quantities. 
        ///  ![cb-colors-manual-breaks.png](http://Esri.github.io/arcgis-pro-sdk/images/Renderers/cb-colors-manual-breaks.png "Graduated colors with manual intervals renderer.")
        /// </summary>
        /// <returns></returns>
        internal static Task CBGraduatedColorsManualBreaks()
        {
            //Check feature layer name
            //Code works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data
            var featureLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(f => f.Name == "USDemographics");
            if (featureLayer == null)
            {
              MessageBox.Show("This renderer works with the USDemographics feature layer available with the ArcGIS Pro SDK Sample data", "Data missing");
              return Task.FromResult(0);
            }
            //Change these class breaks to be appropriate for your data. These class breaks defined below apply to the US States feature class
            List<CIMClassBreak> listClassBreaks = new List<CIMClassBreak>
            {
                new CIMClassBreak
                {
                    Symbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.RedRGB).MakeSymbolReference(),
                    UpperBound = 24228 
                },
                new CIMClassBreak
                {
                    Symbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.GreenRGB).MakeSymbolReference(),
                    UpperBound = 67290
                },
                new CIMClassBreak
                {
                    Symbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.BlueRGB).MakeSymbolReference(),
                    UpperBound = 121757
                },
                 new CIMClassBreak
                {
                    Symbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.GreyRGB).MakeSymbolReference(),
                    UpperBound = 264435
                },
                  new CIMClassBreak
                {
                    Symbol = SymbolFactory.Instance.ConstructPolygonSymbol(ColorFactory.Instance.WhiteRGB).MakeSymbolReference(),
                    UpperBound = 576594
                }
            };
            return QueuedTask.Run(() =>
            {
                CIMClassBreaksRenderer cimClassBreakRenderer = new CIMClassBreaksRenderer
                {
                    ClassBreakType = ClassBreakType.GraduatedColor,
                    ClassificationMethod = ClassificationMethod.Manual,
                    Field = SDKHelpers.GetNumericField(featureLayer),
                    //Important to add the Minimum break for your data to be classified correctly.
                    //This is vital especially if you have data with negative values.
                    //MinimumBreak = 
                    Breaks = listClassBreaks.ToArray()
                };
           
                featureLayer?.SetRenderer(cimClassBreakRenderer);
            });

        }
        #endregion
    }
}
