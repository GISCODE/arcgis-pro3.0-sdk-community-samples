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
using System.Threading.Tasks;
using System.Windows.Input;

using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Editing.Attributes;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Controls;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Mapping.Events;
using ArcGIS.Core.Geometry.Exceptions;

namespace DivideLines
{
  class DivideLinesViewModel : EmbeddableControl
  {
    public DivideLinesViewModel(System.Xml.Linq.XElement options, bool canChangeOptions) : base(options, canChangeOptions) { }

    private bool _polyLineFeatureSelected = false;

    #region EmbeddableControl interface

    private bool _designMode;
    public bool DesignMode
    {
      get { return _designMode; }
      private set { SetProperty(ref _designMode, value, () => DesignMode); }
    }

    public override bool CanCommit { get { return true; } }

    public override Task CommitAsync() { return base.CommitAsync(); }
    public override void Dispose() { base.Dispose(); }
    protected override void OnOptionsChanged()
    {
      base.OnOptionsChanged();

      DesignMode = !String.IsNullOrEmpty(GetOption("designMode"));
    }

    private string GetOption(string name)
    {
      if (Options == null)
        return "";
      else if (Options.Element(name) != null)
        return Options.Element(name).Value.ToString();
      else if (Options.Attribute(name) != null)
        return Options.Attribute(name).Value.ToString();
      else
        return "";
    }

    #endregion

    public override async Task OpenAsync()
    {
      await base.OpenAsync();
      if (DesignMode) return;

      // Make sure tool is active (if not already).
      await FrameworkApplication.SetCurrentToolAsync("esri_sample_divideLinesTool");

      _okRelay = new RelayCommand(() => DivideLinesAsync(IsNumberOfParts, Value), CanDivideLines);
      NotifyPropertyChanged(() => OKCommand);

      MapSelectionChangedEvent.Subscribe(OnSelectionChanged);

      //Run on MCT
      _ = QueuedTask.Run(() =>
      {
        _polyLineFeatureSelected = CheckSelectionAsync(MapView.Active.Map.GetSelection());
      });
    }

    public override Task CloseAsync()
    {
      MapSelectionChangedEvent.Unsubscribe(OnSelectionChanged);
      return base.CloseAsync();
    }

    #region Bindable Properties

    private RelayCommand _okRelay;
    public ICommand OKCommand { get { return _okRelay; } }

    private static bool _isNumberOfParts = true;
    public bool IsNumberOfParts
    {
      get { return _isNumberOfParts; }
      set
      {
        if (SetProperty(ref _isNumberOfParts, value, () => IsNumberOfParts))
          NotifyPropertyChanged(() => IsDistance);
      }
    }

    public bool IsDistance
    {
      get { return !_isNumberOfParts; }
      set { IsNumberOfParts = !value; }
    }

    private static double _value = 2;
    public double Value
    {
      get { return _value; }
      set { SetProperty(ref _value, value, () => Value); }
    }
    #endregion

    #region Implementation

    private void OnSelectionChanged(MapSelectionChangedEventArgs obj)
    {
      if (obj.Map != MapView.Active.Map) return;
      //Run on MCT
      _ = QueuedTask.Run(() =>
      {
        _polyLineFeatureSelected = CheckSelectionAsync(obj.Selection);
      });
    }

    private bool CheckSelectionAsync(SelectionSet sel)
    {
      //Enable only if we have a selected polyline feature that is editable.
      if (sel == null || sel.Count != 1) return false;
      var member = sel.ToDictionary().Keys.FirstOrDefault();
      if (member is IDisplayTable displayTable)
      {
        var canEdit = displayTable.CanEditData();
        if (!canEdit) return false;

        if (member is not FeatureLayer flayer) return false;

        if (flayer.ShapeType != ArcGIS.Core.CIM.esriGeometryType.esriGeometryPolyline) return false;

        return true;
      }
      return false;
    }

    private bool CanDivideLines()
    {
      return _polyLineFeatureSelected;
    }

    /// <summary>
    /// Divide the first selected feature into equal parts or by map unit distance.
    /// </summary>
    /// <param name="numberOfParts">Number of parts to create.</param>
    /// <param name="value">Value for number or parts or distance.</param>
    /// <returns></returns>
    private Task DivideLinesAsync(bool numberOfParts, double value)
    {
      //Run on MCT
      return QueuedTask.Run(() =>
      {
        //get selected feature
        var selectedFeatures = MapView.Active.Map.GetSelection();

        //get the layer of the selected feature
        var dictSelection = selectedFeatures.ToDictionary();
        var featLayer = dictSelection.Keys.First() as FeatureLayer;
        var oid = dictSelection.Values.First().First();

        var feature = featLayer.Inspect(oid);

        //get geometry and length
        var origPolyLine = feature.Shape as Polyline;
        var origLength = GeometryEngine.Instance.Length(origPolyLine);

        //List of mappoint geometries for the split
        var splitPoints = new List<MapPoint>();

        var enteredValue = (numberOfParts) ? origLength / value : value;
        var splitAtDistance = 0 + enteredValue;

        while (splitAtDistance < origLength)
        {
          //create a mapPoint at splitDistance and add to splitpoint list
          MapPoint pt = null;
          try
          {
            pt = GeometryEngine.Instance.MovePointAlongLine(origPolyLine, splitAtDistance, false, 0, SegmentExtensionType.NoExtension);
          }
          catch (GeometryObjectException)
          {
            // line is an arc?
          }

          if (pt != null)
            splitPoints.Add(pt);
          splitAtDistance += enteredValue;
        }

        if (splitPoints.Count == 0)
        {
          ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Divide lines was unable to process your selected line. Please select another.", "Divide Lines");
          return;
        }
        //create and execute the edit operation
        var op = new EditOperation()
        {
          Name = "Divide Lines",
          SelectModifiedFeatures = false,
          SelectNewFeatures = false
        };
        op.Split(featLayer, oid, splitPoints);
        op.Execute();

        //clear selection
        featLayer.ClearSelection();
      });
    }

    #endregion
  }
}
