using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using EnhancedUI;
using System;

namespace EnhancedScrollerDemos.ExpandingCells
{
    /// <summary>
    /// This is the view of our cell which handles how the cell looks.
    /// </summary>
    public class CellView : EnhancedScrollerCellView
    {
        public Text dataIndexText;
        public Text headerText;
        public Text descriptionText;
        public Action<int> cellClicked;

        /// <summary>
        /// This function just takes the Demo data and displays it
        /// </summary>
        /// <param name="data"></param>
        public void SetData(Data data, int dataIndex, Action<int> cellClicked)
        {
            this.dataIndex = dataIndex;
            this.cellClicked = cellClicked;

            dataIndexText.text = dataIndex.ToString();
            headerText.text = data.headerText;
            descriptionText.text = data.descriptionText;

            descriptionText.enabled = data.isExpanded;
        }

        public void CellButton_Clicked()
        {
            if (cellClicked != null)
            {
                cellClicked(dataIndex);

                
            }
        }
    }
}
