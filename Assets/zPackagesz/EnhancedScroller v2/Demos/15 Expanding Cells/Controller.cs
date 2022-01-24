using UnityEngine;
using System.Collections;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;

namespace EnhancedScrollerDemos.ExpandingCells
{
    /// <summary>
    /// This example shows how you can expand and collapse cells
    /// </summary>
	public class Controller : MonoBehaviour, IEnhancedScrollerDelegate
    {

        /// <summary>
        /// Internal representation of our data. Note that the scroller will never see
        /// this, so it separates the data from the layout using MVC principles.
        /// </summary>
        private SmallList<Data> _data;


        /// <summary>
        /// This is our scroller we will be a delegate for
        /// </summary>
        public EnhancedScroller scroller;


        /// <summary>
        /// This will be the prefab of each cell in our scroller. Note that you can use more
        /// than one kind of cell, but this example just has the one type.
        /// </summary>
        public EnhancedScrollerCellView cellViewPrefab;

        /// <summary>
        /// Be sure to set up your references to the scroller after the Awake function. The
        /// scroller does some internal configuration in its own Awake function. If you need to
        /// do this in the Awake function, you can set up the script order through the Unity editor.
        /// In this case, be sure to set the EnhancedScroller's script before your delegate.
        ///
        /// In this example, we are calling our initializations in the delegate's Start function,
        /// but it could have been done later, perhaps in the Update function.
        /// </summary>
        void Start()
        {
            // tell the scroller that this script will be its delegate
            scroller.Delegate = this;

            // load in a large set of data
            LoadData();
        }


        /// <summary>
        /// Populates the data with a lot of records
        /// </summary>
        private void LoadData()
        {
            // set up some simple data
            _data = new SmallList<Data>();
            for (var i = 0; i < 50; i++)
            {
                if (i % 2 == 0)
                {
                    _data.Add(new Data()
                    {
                        headerText = "Multiple Expand",
                        descriptionText = "Expanding this cell will not collapse other cells. This allows you to have multiple cells expanded at once.\n\nClick the cell again to collapse.",
                        isExpanded = false,
                        expandedSize = 200f,
                        collapsedSize = 50f
                    });
                }
                else
                {
                    _data.Add(new Data()
                    {
                        headerText = "Single Expand",
                        descriptionText = "Expanding this cell will collapse other cells.\n\nClick the cell again to collapse.",
                        isExpanded = false,
                        expandedSize = 200f,
                        collapsedSize = 50f
                    });
                }
            }

            // tell the scroller to reload now that we have the data
            scroller.ReloadData();
        }


        /// <summary>
        /// This is called when a cell is clicked
        /// </summary>
        /// <param name="dataIndex"></param>
        private void CellClicked(int dataIndex)
        {
            // toggle the expanded value
            _data[dataIndex].isExpanded = !_data[dataIndex].isExpanded;

            if (dataIndex % 2 == 1)
            {
                // single expanded cell. collapse others
                for (var i = 0; i < _data.Count; i++)
                {
                    if (i != dataIndex)
                    {
                        _data[i].isExpanded = false;
                    }
                }
            }

            // reload the data to set up the new positions of each cell.
            // we pass in the normalized position so the scroller appears not
            // to jump, but stays in place.
            scroller.ReloadData(scroller.NormalizedScrollPosition);
        }

        #region EnhancedScroller Handlers

        /// <summary>
        /// This tells the scroller the number of cells that should have room allocated.
        /// For this example, the count is the number of data elements divided by the number of cells per row (rounded up using Mathf.CeilToInt)
        /// </summary>
        /// <param name="scroller">The scroller that is requesting the data size</param>
        /// <returns>The number of cells</returns>
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return _data.Count;
        }

        /// <summary>
        /// This tells the scroller what the size of a given cell will be. Cells can be any size and do not have
        /// to be uniform. For vertical scrollers the cell size will be the height. For horizontal scrollers the
        /// cell size will be the width.
        /// </summary>
        /// <param name="scroller">The scroller requesting the cell size</param>
        /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
        /// <returns>The size of the cell</returns>
        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            // pass the expanded size if the cell is expanded, else pass the collapsed size
            return _data[dataIndex].isExpanded ? _data[dataIndex].expandedSize : _data[dataIndex].collapsedSize;
        }

        /// <summary>
        /// Gets the cell to be displayed. You can have numerous cell types, allowing variety in your list.
        /// Some examples of this would be headers, footers, and other grouping cells.
        /// </summary>
        /// <param name="scroller">The scroller requesting the cell</param>
        /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
        /// <param name="cellIndex">The index of the list. This will likely be different from the dataIndex if the scroller is looping</param>
        /// <returns>The cell for the scroller to use</returns>
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            // first, we get a cell from the scroller by passing a prefab.
            // if the scroller finds one it can recycle it will do so, otherwise
            // it will create a new cell.
            CellView cellView = scroller.GetCellView(cellViewPrefab) as CellView;

            cellView.name = "Cell " + dataIndex.ToString();

            // pass in a reference to our data 
            cellView.SetData(_data[dataIndex], dataIndex, CellClicked);

            // return the cell to the scroller
            return cellView;
        }

        #endregion
    }
}
