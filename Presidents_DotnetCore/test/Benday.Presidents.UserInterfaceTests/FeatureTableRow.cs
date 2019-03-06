using System;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace Benday.Presidents.UserInterfaceTests
{
    public partial class SeleniumPresidentsTest
    {
        public class FeatureTableRow
        {
            public FeatureTableRow(IWebElement row)
            {
                if (row == null)
                {
                    throw new ArgumentNullException(nameof(row), $"{nameof(row)} is null.");
                }

                var cells = GetCellsForRow(row);

                if (cells.Count == 4)
                {
                    FeatureName = cells[0].Text;

                    PopulateIsEnabled(cells[1]);
                    PopulateEditLink(cells[3]);

                    IsValid = true;
                }
                else
                {
                    IsValid = false;
                }
            }

            private static ReadOnlyCollection<IWebElement> GetCellsForRow(IWebElement row)
            {
                var result = row.FindElements(By.TagName("td"));

                return result;
            }

            public bool IsValid { get; private set; }
            public bool IsEnabled { get; set; }
            public string FeatureName { get; set; }
            public IWebElement EditLink { get; set; }

            private void PopulateIsEnabled(IWebElement cell)
            {
                var checkbox = cell.FindElement(By.TagName("input"));

                var checkedValue = checkbox.GetAttribute("checked");

                if (checkedValue == "true")
                {
                    IsEnabled = true;
                }
                else
                {
                    IsEnabled = false;
                }
            }
            public void PopulateEditLink(IWebElement cell)
            {
                var link = cell.FindElement(By.LinkText("Edit"));

                EditLink = link;
            }
        }
    }
}
