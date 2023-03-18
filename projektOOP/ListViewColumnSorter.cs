using System.Collections;
using System.Windows.Forms;

namespace projektOOP
{

    //Taken from: https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/sort-listview-by-column
    //no clue why this isn't just integrated into the winForms package
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;

        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;

        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor. Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        long getNumBytes(string readeable)
        {
            string[] parts = readeable.Split(' ');
            if (parts[1] == "GB")
            {
                return int.Parse(parts[0]) * 1000000000;
            }
            else if (parts[1] == "MB")
            {
                return int.Parse(parts[0]) * 1000000;
            }
            else if (parts[1] == "KB")
            {
                return int.Parse(parts[0]) * 1000;
            }
            else
            {
                return int.Parse(parts[0]);
            }
        }

        /// <summary>
        /// This method is inherited from the IComparer interface. It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            //Modified to accomodate sorting by filesize

            int compareResult = 0;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            if(ColumnToSort == 1) //handle if we are sorting by filesize
            {
                if(listviewX.SubItems[ColumnToSort].Text == "<DIR>") //In any matchup directory will be portrayed as having a larger size
                {
                    compareResult = 1;
                }
                else if(listviewY.SubItems[ColumnToSort].Text == "<DIR>")
                {
                    compareResult = -1;
                }
                else
                {
                    long bytesX = getNumBytes(listviewX.SubItems[ColumnToSort].Text);
                    long bytesY = getNumBytes(listviewY.SubItems[ColumnToSort].Text);
                    if (bytesX < bytesY)
                    {
                        compareResult = -1;
                    }
                    else if (bytesX > bytesY)
                    {
                        compareResult = 1;
                    }
                }
            }
            else //else just sort alphabeticly
            {
                // Compare the two items
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
            }

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
    }
}
